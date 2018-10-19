using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer
{
    public partial class Facade
    {
        public Score GetScore()
        {
            Score result = new Score();

            result.TeamName = new Dictionary<TeamSelection, string>();
            result.CurrentSource = new Dictionary<TeamSelection, int>();
            result.GuaranteedSource = new Dictionary<TeamSelection, int>();
            result.Rounds = new List<ScorePerRound>();
            Dictionary<Guid, PlayerWithGameCount> players = new Dictionary<Guid, PlayerWithGameCount>();

            InitialScore(result, TeamSelection.Team1, players);
            InitialScore(result, TeamSelection.Team2, players);

            bool gameFinished = true;

            if (dataFile.Games != null)
            {
                for (int roundIndex = 0; roundIndex < dataFile.Basis.RoundCount; roundIndex++)
                {
                    var round = dataFile.Games[roundIndex];
                    ScorePerRound roundResult = new ScorePerRound();
                    roundResult.Index = roundIndex;
                    roundResult.Games = new List<ScoreLine>();
                    roundResult.Score = new Dictionary<TeamSelection, int>();
                    roundResult.Score.Add(TeamSelection.Team1, 0);
                    roundResult.Score.Add(TeamSelection.Team2, 0);
                    for (int gameIndex = 0; gameIndex < dataFile.Basis.GameCount; gameIndex++)
                    {
                        var game = round[gameIndex];
                        if (game.GameResult != GameResult.Finished)
                        {
                            gameFinished = false;
                            continue;
                        }
                        ScoreLine gameResult = new ScoreLine();
                        gameResult.Index = gameIndex;
                        gameResult.PlayerId = game.PlayerId;
                        gameResult.PlayerName = new Dictionary<TeamSelection, string>();
                        gameResult.PlayerName.Add(TeamSelection.Team1, GetPlayer(TeamSelection.Team1, gameResult.PlayerId[TeamSelection.Team1]).Name);
                        gameResult.PlayerName.Add(TeamSelection.Team2, GetPlayer(TeamSelection.Team2, gameResult.PlayerId[TeamSelection.Team2]).Name);
                        gameResult.Winner = game.Winner;
                        roundResult.Games.Add(gameResult);
                        players[game.PlayerId[TeamSelection.Team1]].Played++;
                        players[game.PlayerId[TeamSelection.Team2]].Played++;
                        if (gameResult.Winner == TeamSelection.Team1)
                        {
                            players[game.PlayerId[TeamSelection.Team1]].Won++;
                            players[game.PlayerId[TeamSelection.Team2]].Lost++;
                            roundResult.Score[TeamSelection.Team1]++;
                        }
                        else
                        {
                            players[game.PlayerId[TeamSelection.Team2]].Won++;
                            players[game.PlayerId[TeamSelection.Team1]].Lost++;
                            roundResult.Score[TeamSelection.Team2]++;
                        }
                    }
                    if (roundResult.Games.Count > 0)
                        result.Rounds.Add(roundResult);
                }
            }

            result.Players = new List<PlayerWithGameCount>(players.Values);

            foreach (var round in result.Rounds)
            {
                result.CurrentSource[TeamSelection.Team1] += round.Score[TeamSelection.Team1];
                result.CurrentSource[TeamSelection.Team2] += round.Score[TeamSelection.Team2];
            }
            foreach (var player in result.Players)
            {
                if (player.MaxAttending == 0) continue;
                var pass = player.Played - player.MaxAttending;
                if (pass > 0)
                {
                    player.PlayedAboveMaxAttending = pass;
                    result.CurrentSource[player.Team] -= pass;
                }
            }
            result.GuaranteedSource.Add(TeamSelection.Team1, result.CurrentSource[TeamSelection.Team1]);
            result.GuaranteedSource.Add(TeamSelection.Team2, result.CurrentSource[TeamSelection.Team2]);
            foreach (var player in result.Players)
            {
                if (player.MinAttending == 0) continue;
                var pass = player.MinAttending - player.Played;
                if (pass > 0)
                {
                    player.PlayedBelowMinAttending = pass;
                    result.GuaranteedSource[player.Team] -= pass;
                }
            }

            if (gameFinished)
            {
                result.CurrentSource = result.GuaranteedSource;
                result.GuaranteedSource = null;
            }

            return result;
        }

        void InitialScore(Score score, TeamSelection teamSelection, Dictionary<Guid, PlayerWithGameCount> players)
        {
            score.TeamName.Add(teamSelection, dataFile.Teams[teamSelection].Name);
            score.CurrentSource.Add(teamSelection, 0);
            foreach (var player in dataFile.Teams[teamSelection].Players)
            {
                PlayerWithGameCount item = new PlayerWithGameCount();
                item.Id = player.Key;
                item.Team = teamSelection;
                //item.Played = 0;
                //item.Won = 0;
                //item.Lost = 0;
                item.Name = player.Value.Name;
                item.DefaultRace = player.Value.DefaultRace;
                item.MaxAttending = player.Value.MaxAttending;
                item.MinAttending = player.Value.MinAttending;
                players.Add(player.Key, item);
            }
        }
    }
}
