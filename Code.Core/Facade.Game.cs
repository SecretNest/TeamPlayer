using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer
{
    public partial class Facade
    {
        public Game GetGame(int roundIndex, int gameIndex)
        {
            return dataFile.Games[roundIndex][gameIndex];
        }

        public List<GameWithIndex> GetGames(params GameResult[] gameResults)
        {
            if (dataFile.Games == null)
            {
                return null;
            }
            else
            {
                List<GameWithIndex> results = new List<GameWithIndex>();
                for (int roundIndex = 0; roundIndex < dataFile.Games.Count; roundIndex++)
                {
                    var round = dataFile.Games[roundIndex];
                    for (int gameIndex = 0; gameIndex < round.Count; gameIndex++)
                    {
                        var game = round[gameIndex];
                        if (Array.IndexOf(gameResults, game.GameResult) != -1)
                        {
                            var gameWithIndex = new GameWithIndex();
                            gameWithIndex.GameResult = game.GameResult;
                            gameWithIndex.GameTime = game.GameTime;
                            gameWithIndex.IsTimeIncluded = game.IsTimeIncluded;
                            gameWithIndex.MapId = game.MapId;
                            gameWithIndex.Player1RaceId = game.Player1RaceId;
                            gameWithIndex.Player2RaceId = game.Player2RaceId;
                            gameWithIndex.Team1PlayerId = game.Team1PlayerId;
                            gameWithIndex.Team2PlayerId = game.Team2PlayerId;
                            gameWithIndex.RoundIndex = roundIndex;
                            gameWithIndex.GameIndex = gameIndex;
                            results.Add(gameWithIndex);
                        }
                    }
                }

                return results;
            }
        }

        public List<GameWithIndex> GetGames(GameResult gameResult)
        {
            if (dataFile.Games == null)
            {
                return null;
            }
            else
            {
                List<GameWithIndex> results = new List<GameWithIndex>();
                for (int roundIndex = 0; roundIndex < dataFile.Games.Count; roundIndex++)
                {
                    var round = dataFile.Games[roundIndex];
                    for (int gameIndex = 0; gameIndex < round.Count; gameIndex++)
                    {
                        var game = round[gameIndex];
                        if (gameResult == game.GameResult)
                        {
                            var gameWithIndex = new GameWithIndex();
                            gameWithIndex.GameResult = game.GameResult;
                            gameWithIndex.GameTime = game.GameTime;
                            gameWithIndex.IsTimeIncluded = game.IsTimeIncluded;
                            gameWithIndex.MapId = game.MapId;
                            gameWithIndex.Player1RaceId = game.Player1RaceId;
                            gameWithIndex.Player2RaceId = game.Player2RaceId;
                            gameWithIndex.Team1PlayerId = game.Team1PlayerId;
                            gameWithIndex.Team2PlayerId = game.Team2PlayerId;
                            gameWithIndex.RoundIndex = roundIndex;
                            gameWithIndex.GameIndex = gameIndex;
                            results.Add(gameWithIndex);
                        }
                    }
                }

                return results;
            }
        }

        public List<GameWithIndex> GetFinishedGames()
        {
            return GetGames(GameResult.Team1Won, GameResult.Team2Won);
        }

        public GameWithIndex GetLatestFinishedGame()
        {
            var all = GetFinishedGames();
            if (all == null) return null;
            else if (all.Count == 1) return all[0];

            DateTime latestTime = DateTime.MinValue;
            int latestRound = -1;
            int latestGame = -1;
            GameWithIndex match = null;
            foreach(var game in all)
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
            playingGames = GetGames(GameResult.InGame);
        }

        public void SetGame(int roundIndex, int gameIndex, Game game)
        {
            if (game.GameTime != DateTime.MinValue && !game.IsTimeIncluded && game.GameTime.TimeOfDay != TimeSpan.Zero)
            {
                game.GameTime -= game.GameTime.TimeOfDay;
            }
            dataFile.Games[roundIndex][gameIndex] = game;
            Save();
        }
    }
}
