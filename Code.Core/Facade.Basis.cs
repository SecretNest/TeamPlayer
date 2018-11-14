using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SecretNest.TeamPlayer
{
	public partial class Facade
	{
		public Basis GetBasis()
		{
			return dataFile.Basis;
		}

		public Dictionary<Guid, Race> GetRacesForPlayerDefaultSelection()
		{
			var result = new Dictionary<Guid, Race>();
			foreach (var race in dataFile.Basis.Races)
			{
				if (race.Value.CanBePlayerDefault)
				{
					result.Add(race.Key, race.Value);
				}
			}
			return result;
		}

		public Dictionary<Guid, Race> GetRacesForGameSelection()
		{
			var result = new Dictionary<Guid, Race>();
			foreach (var race in dataFile.Basis.Races)
			{
				if (race.Value.CanUseInGame)
				{
					result.Add(race.Key, race.Value);
				}
			}
			return result;
		}

		public KeyValuePair<Guid, Race> GetRaceForGameConvertingFromPlayerDefault(Guid raceIdFromPlayerDefault)
		{
			if (raceIdFromPlayerDefault == Guid.Empty)
			{
				return new KeyValuePair<Guid, Race>(raceIdFromPlayerDefault, null);
			}

			var defaultRace = dataFile.Basis.Races[raceIdFromPlayerDefault];
			if (defaultRace.CanUseInGame)
			{
				return new KeyValuePair<Guid, Race>(raceIdFromPlayerDefault, defaultRace);
			}

			var gameRace = defaultRace.TargetIdInGameConverting;
			if (gameRace == Guid.Empty)
			{
				return new KeyValuePair<Guid, Race>(gameRace, null);
			}
			else
			{
				return new KeyValuePair<Guid, Race>(gameRace, dataFile.Basis.Races[gameRace]);
			}
		}

		public Dictionary<Guid, Map> GetMaps()
		{
			return dataFile.Basis.Maps;
		}

		public void SetBasisNameAndDescription(string name, string description)
		{
			dataFile.Basis.Name = name;
			dataFile.Basis.Description = description;
			Save();
		}

		public void SetBasisRoundAndGame(int round, int game)
		{
			dataFile.Basis.RoundCount = round;
			dataFile.Basis.GameCount = game;
			dataFile.Games = new List<List<Game>>(round);
			for (var r = 0; r < round; r++)
			{
				var oneRound = new List<Game>(game);
				for (var g = 0; g < game; g++)
				{
					var oneGame = new Game
					{
						PlayerIds = new Dictionary<TeamSelection, Guid>()
					};
					oneGame.PlayerIds.Add(TeamSelection.Team1, Guid.Empty);
					oneGame.PlayerIds.Add(TeamSelection.Team2, Guid.Empty);
					oneGame.RaceIds = new Dictionary<TeamSelection, Guid>
					{
						{TeamSelection.Team1, Guid.Empty}, {TeamSelection.Team2, Guid.Empty}
					};
					oneGame.GameResult = GameResult.NotStarted;
					oneGame.GameTime = DateTime.MinValue;
					oneRound.Add(oneGame);
				}
				dataFile.Games.Add(oneRound);
			}
			Save();
		}

		public void AddBasisRace(Race race)
		{
			if (race.TargetIdInGameConverting != Guid.Empty)
			{
				if (dataFile.Basis.Races.TryGetValue(race.TargetIdInGameConverting, out var targetRace))
				{
					if (!targetRace.CanUseInGame)
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "种族 {0} 对应的实际种族 {1} 不是比赛中可选择的种族。", race.Name, targetRace.Name));
					}
				}
				else
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "种族 {0} 的对应实际种族不存在。", race.Name));
				}
			}

			dataFile.Basis.Races.Add(Guid.NewGuid(), race);
			Save();
		}

		public void DeleteBasisRace(Guid raceId)
		{
			if (dataFile.Basis.RoundCount > 0 && dataFile.Basis.GameCount > 0)
			{
				var allGameRaces = from round in dataFile.Games
								   from game in round
								   from race in game.RaceIds
								   select race.Value;


				if (allGameRaces.Contains(raceId))
				{
					throw new InvalidOperationException("已在比赛中使用的种族不允许删除。");
				}



				var allPlayerRaces = from team in dataFile.Teams
									 from player in team.Value.Players
									 select player.Value.DefaultRace;

				if (allPlayerRaces.Contains(raceId))
				{
					throw new InvalidOperationException("已设置为选手默认的种族不允许删除。");

				}
			}

			dataFile.Basis.Races.Remove(raceId);
			Save();

		}

		public bool SetBasisRaces(Dictionary<Guid, Race> races, out string errorText)
		{
			foreach (var test in races)
			{
				if (test.Value.TargetIdInGameConverting != Guid.Empty)
				{
					if (races.TryGetValue(test.Value.TargetIdInGameConverting, out var target))
					{
						if (!target.CanUseInGame)
						{
							errorText = string.Format("{0}对应的比赛种族({1})未允许在比赛中使用。", test.Value.Name, target.Name);
							return false;
						}
					}
					else
					{
						errorText = string.Format("{0}对应的比赛种族找不到。", test.Value.Name);
						return false;
					}
				}
			}

			foreach (var existed in dataFile.Basis.Races.Keys)
			{
				if (!races.ContainsKey(existed))
				{
					if (dataFile.Basis.RoundCount > 0 && dataFile.Basis.GameCount > 0)
					{
						foreach (var round in dataFile.Games)
						{
							foreach (var game in round)
							{
								if (game.RaceIds[TeamSelection.Team1] == existed || game.RaceIds[TeamSelection.Team2] == existed)
								{
									errorText = "已在比赛中使用的种族不允许删除。";
									return false;
								}
							}
						}
					}
					foreach (var player in dataFile.Teams[TeamSelection.Team1].Players.Values)
					{
						if (existed == player.DefaultRace)
						{
							errorText = "已设置为选手默认的种族不允许删除。";
							return false;
						}
					}
					foreach (var player in dataFile.Teams[TeamSelection.Team2].Players.Values)
					{
						if (existed == player.DefaultRace)
						{
							errorText = "已设置为选手默认的种族不允许删除。";
							return false;
						}
					}
				}
			}

			dataFile.Basis.Races = races;
			Save();
			errorText = null;
			return true;
		}

		public void AddBasisMap(Map map)
		{
			if (map == null)
			{
				throw new ArgumentNullException(nameof(map));
			}

			if (dataFile.Basis.Maps.Any(i => string.Equals(i.Value.Name, map.Name, StringComparison.InvariantCultureIgnoreCase)))
			{
				throw new InvalidOperationException("地图列表中已经存在相同名称的地图。");
			}

			dataFile.Basis.Maps.Add(Guid.NewGuid(), map);
			Save();
		}

		public void DeleteBasisMap(Guid mapId)
		{
			if (dataFile.Basis.RoundCount > 0 && dataFile.Basis.GameCount > 0)
			{
				foreach (var round in dataFile.Games)
				{
					foreach (var game in round)
					{
						if (game.MapId == mapId)
						{
							throw new InvalidOperationException("已被比赛使用的地图不允许删除。");
						}
					}
				}
			}

			dataFile.Basis.Maps.Remove(mapId);
			Save();
		}

		public bool SetBasisMaps(Dictionary<Guid, Map> maps, out string errorText)
		{
			if (dataFile.Basis.RoundCount > 0 && dataFile.Basis.GameCount > 0)
			{
				foreach (var round in dataFile.Games)
				{
					foreach (var game in round)
					{
						if (game.MapId != Guid.Empty && !maps.ContainsKey(game.MapId))
						{
							errorText = "已被比赛使用的地图不允许删除。";
							return false;
						}
					}
				}
			}

			dataFile.Basis.Maps = maps;
			Save();
			errorText = null;
			return true;
		}
	}
}
