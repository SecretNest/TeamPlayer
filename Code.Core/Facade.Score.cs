using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SecretNest.TeamPlayer
{
    public partial class Facade
    {
        public Score GetScore()
        {
			var result = new Score
			{
				TeamName = new Dictionary<TeamSelection, string>(),
				CurrentSource = new Dictionary<TeamSelection, int>(),
				GuaranteedSource = new Dictionary<TeamSelection, int>(),
				Rounds = new List<ScorePerRound>()
			};
			var players = new Dictionary<Guid, PlayerWithGameCount>();

            InitialScore(result, TeamSelection.Team1, players);
            InitialScore(result, TeamSelection.Team2, players);

            var gameFinished = true;

            if (dataFile.Games != null)
            {
                for (var roundIndex = 0; roundIndex < dataFile.Basis.RoundCount; roundIndex++)
                {
                    var round = dataFile.Games[roundIndex];
					var roundResult = new ScorePerRound
					{
						Index = roundIndex,
						Games = new List<ScoreLine>(),
						Score = new Dictionary<TeamSelection, int>()
					};
					roundResult.Score.Add(TeamSelection.Team1, 0);
                    roundResult.Score.Add(TeamSelection.Team2, 0);
                    for (var gameIndex = 0; gameIndex < dataFile.Basis.GameCount; gameIndex++)
                    {
                        var game = round[gameIndex];
                        if (game.GameResult != GameResult.Finished)
                        {
                            gameFinished = false;
                            continue;
                        }
						var gameResult = new ScoreLine
						{
							Index = gameIndex,
							PlayerId = game.PlayerId,
							PlayerName = new Dictionary<TeamSelection, string>()
						};
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

            var notOrderedPlayers = new List<PlayerWithGameCount>(players.Values);

            foreach (var round in result.Rounds)
            {
                result.CurrentSource[TeamSelection.Team1] += round.Score[TeamSelection.Team1];
                result.CurrentSource[TeamSelection.Team2] += round.Score[TeamSelection.Team2];
            }
            foreach (var player in notOrderedPlayers)
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
            foreach (var player in notOrderedPlayers)
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

            result.Players = notOrderedPlayers.OrderBy(i => i.Team).ThenByDescending(i => i.Played).ThenByDescending(i => i.Won).ToList();
            return result;
        }

        void InitialScore(Score score, TeamSelection teamSelection, Dictionary<Guid, PlayerWithGameCount> players)
        {
            score.TeamName.Add(teamSelection, dataFile.Teams[teamSelection].Name);
            score.CurrentSource.Add(teamSelection, 0);
            foreach (var player in dataFile.Teams[teamSelection].Players)
            {
				var item = new PlayerWithGameCount
				{
					Id = player.Key,
					Team = teamSelection,
					//item.Played = 0;
					//item.Won = 0;
					//item.Lost = 0;
					Name = player.Value.Name,
					DefaultRace = player.Value.DefaultRace,
					MaxAttending = player.Value.MaxAttending,
					MinAttending = player.Value.MinAttending
				};
				players.Add(player.Key, item);
            }
        }
    }
}
