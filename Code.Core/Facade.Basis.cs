using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

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
            Dictionary<Guid, Race> result = new Dictionary<Guid, Race>();
            foreach(var race in dataFile.Basis.Races)
            {
                if (race.Value.IsInPlayerDefault)
                    result.Add(race.Key, race.Value);
            }
            return result;
        }

        public Dictionary<Guid, Race> GetRacesForGameSelection()
        {
            Dictionary<Guid, Race> result = new Dictionary<Guid, Race>();
            foreach (var race in dataFile.Basis.Races)
            {
                if (race.Value.IsInGame)
                    result.Add(race.Key, race.Value);
            }
            return result;
        }

        public KeyValuePair<Guid, Race> GetRaceForGameConvertingFromPlayerDefault(Guid raceIdFromPlayerDefault)
        {
            if (raceIdFromPlayerDefault == Guid.Empty) return new KeyValuePair<Guid, Race>(raceIdFromPlayerDefault, null);
            var defaultRace = dataFile.Basis.Races[raceIdFromPlayerDefault];
            if (defaultRace.IsInGame) return new KeyValuePair<Guid, Race>(raceIdFromPlayerDefault, defaultRace);
            var gameRace = defaultRace.TargetIdInGameConverting;
            if (gameRace == Guid.Empty) return new KeyValuePair<Guid, Race>(gameRace, null);
            else return new KeyValuePair<Guid, Race>(gameRace, dataFile.Basis.Races[gameRace]);
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
            for (int r = 0; r < round; r++)
            {
                List<Game> oneRound = new List<Game>(game);
                for (int g = 0; g < game; g++)
                {
                    var oneGame = new Game();
                    oneGame.PlayerId = new Dictionary<TeamSelection, Guid>();
                    oneGame.PlayerId.Add(TeamSelection.Team1, Guid.Empty);
                    oneGame.PlayerId.Add(TeamSelection.Team2, Guid.Empty);
                    oneGame.RaceId = new Dictionary<TeamSelection, Guid>();
                    oneGame.RaceId.Add(TeamSelection.Team1, Guid.Empty);
                    oneGame.RaceId.Add(TeamSelection.Team2, Guid.Empty);
                    oneGame.GameResult = GameResult.NotStarted;
                    oneGame.GameTime = DateTime.MinValue;
                    oneRound.Add(oneGame);
                }
                dataFile.Games.Add(oneRound);
            }
            Save();
        }

        public bool SetBasisRaces(Dictionary<Guid, Race> races, out string errorText)
        {
            foreach(var test in races)
            {
                if (test.Value.TargetIdInGameConverting != Guid.Empty)
                {
                    if (races.TryGetValue(test.Value.TargetIdInGameConverting, out var target))
                    {
                        if (!target.IsInGame)
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
                                if (game.RaceId[TeamSelection.Team1] == existed || game.RaceId[TeamSelection.Team2] == existed)
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
