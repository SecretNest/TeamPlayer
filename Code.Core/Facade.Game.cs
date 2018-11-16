using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretNest.TeamPlayer
{
	public partial class Facade
	{
		public Game GetGame(int roundIndex, int gameIndex)
		{
			return dataFile.Games[roundIndex][gameIndex];
		}

		public List<GameWithIndex> GetGamesByRound(int roundIndex)
		{
			if (dataFile.Games == null)
			{
				return null;
			}
			else
			{
				var results = new List<GameWithIndex>();
				var round = dataFile.Games[roundIndex];
				for (var gameIndex = 0; gameIndex < round.Count; gameIndex++)
				{
					var game = round[gameIndex];

					var gameWithIndex = new GameWithIndex
					{
						GameResult = game.GameResult,
						GameTime = game.GameTime,
						IsTimeIncluded = game.IsTimeIncluded,
						MapId = game.MapId,
						RaceIds = game.RaceIds,
						PlayerIds = game.PlayerIds,
						RoundIndex = roundIndex,
						GameIndex = gameIndex,
						Description = game.Description
					};
					results.Add(gameWithIndex);

				}
				return results;
			}
		}

		public List<GameWithIndex> GetGamesByPlayer(TeamSelection teamSelection, Guid playerId)
		{
			if (dataFile.Games == null)
			{
				return null;
			}
			else
			{
				var results = new List<GameWithIndex>();
				for (var roundIndex = 0; roundIndex < dataFile.Games.Count; roundIndex++)
				{
					var round = dataFile.Games[roundIndex];
					for (var gameIndex = 0; gameIndex < round.Count; gameIndex++)
					{
						var game = round[gameIndex];
						if (game.PlayerIds[teamSelection] == playerId)
						{
							var gameWithIndex = new GameWithIndex
							{
								GameResult = game.GameResult,
								GameTime = game.GameTime,
								IsTimeIncluded = game.IsTimeIncluded,
								MapId = game.MapId,
								RaceIds = game.RaceIds,
								PlayerIds = game.PlayerIds,
								RoundIndex = roundIndex,
								GameIndex = gameIndex
							};
							results.Add(gameWithIndex);
						}
					}
				}

				return results;
			}
		}

		public List<GameWithIndex> GetGamesByResults(params GameResult[] gameResults)
		{
			if (dataFile.Games == null)
			{
				return null;
			}
			else
			{
				var results = new List<GameWithIndex>();
				for (var roundIndex = 0; roundIndex < dataFile.Games.Count; roundIndex++)
				{
					var round = dataFile.Games[roundIndex];
					for (var gameIndex = 0; gameIndex < round.Count; gameIndex++)
					{
						var game = round[gameIndex];
						if (gameResults.Contains(game.GameResult))
						{
							var gameWithIndex = new GameWithIndex
							{
								GameResult = game.GameResult,
								GameTime = game.GameTime,
								IsTimeIncluded = game.IsTimeIncluded,
								MapId = game.MapId,
								RaceIds = game.RaceIds,
								PlayerIds = game.PlayerIds,
								RoundIndex = roundIndex,
								GameIndex = gameIndex
							};
							results.Add(gameWithIndex);
						}
					}
				}

				return results;
			}
		}

		public List<GameWithIndex> GetFinishedGames()
		{
			return GetGamesByResults(GameResult.Team1Win, GameResult.Team2Win);
		}

		public GameWithIndex GetLatestFinishedGame()
		{
			var all = GetFinishedGames();
			if (all == null)
			{
				return null;
			}
			else if (all.Count == 1)
			{
				return all[0];
			}

			var latestTime = DateTime.MinValue;
			var latestRound = -1;
			var latestGame = -1;
			GameWithIndex match = null;
			foreach (var game in all)
			{
				if (game.GameTime > latestTime)
				{
					latestTime = game.GameTime;
					latestRound = game.RoundIndex;
					latestGame = game.GameIndex;
					match = game;
				}
				else if (game.GameTime == latestTime)
				{
					if (game.RoundIndex > latestRound)
					{
						latestRound = game.RoundIndex;
						latestGame = game.GameIndex;
						match = game;
					}
					else if (game.RoundIndex == latestRound)
					{
						if (game.RoundIndex > latestGame)
						{
							latestGame = game.GameIndex;
							match = game;
						}
					}
				}
			}
			return match;
		}

		public void GetGameForDisplaying(out GameWithIndex latestFinishedGame, out List<GameWithIndex> playingGames)
		{
			latestFinishedGame = GetLatestFinishedGame();
			playingGames = GetGamesByResults(GameResult.InGame);
		}

		public bool SetGame(int roundIndex, int gameIndex, Game game, out string errorText)
		{
			if (game.GameTime != DateTime.MinValue && !game.IsTimeIncluded && game.GameTime.TimeOfDay != TimeSpan.Zero)
			{
				game.GameTime -= game.GameTime.TimeOfDay;
			}

			var round = dataFile.Games[roundIndex];

			var player = game.PlayerIds[TeamSelection.Team1];
			if (player != Guid.Empty)
			{
				for (var g = 0; g < dataFile.Basis.GameCount; g++)
				{
					if (g == gameIndex)
					{
						continue;
					}

					if (round[g].PlayerIds[TeamSelection.Team1] == player)
					{
						errorText = "队伍1选手在本轮已经参加过比赛。";
						return false;
					}
				}
			}
			player = game.PlayerIds[TeamSelection.Team2];
			if (player != Guid.Empty)
			{
				for (var g = 0; g < dataFile.Basis.GameCount; g++)
				{
					if (g == gameIndex)
					{
						continue;
					}

					if (round[g].PlayerIds[TeamSelection.Team2] == player)
					{
						errorText = "队伍2选手在本轮已经参加过比赛。";
						return false;
					}
				}
			}

			dataFile.Games[roundIndex][gameIndex] = game;
			Save();
			errorText = null;
			return true;
		}
	}
}
