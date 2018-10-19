using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer
{
    public partial class Facade
    {
        public Team GetTeam(TeamSelection teamSelection)
        {
            return dataFile.Teams[teamSelection];
        }

        public string GetTeamName(TeamSelection teamSelection)
        {
            return dataFile.Teams[teamSelection].Name;
        }

        public Player GetPlayer(TeamSelection teamSelection, Guid playerId)
        {
            return dataFile.Teams[teamSelection].Players[playerId];
        }

        public void SetTeamName(TeamSelection teamSelection, string name)
        {
            dataFile.Teams[teamSelection].Name = name;
            Save();
        }

        public bool SetTeamPlayers(TeamSelection teamSelection, Dictionary<Guid, Player> players, out string errorText)
        {
            Team selectedTeam = dataFile.Teams[teamSelection];

            if (dataFile.Basis.RoundCount > 0 && dataFile.Basis.GameCount > 0)
            {
                foreach (var existed in selectedTeam.Players.Keys)
                {
                    if (!players.ContainsKey(existed))
                    {
                        foreach(var round in dataFile.Games)
                        {
                            foreach(var game in round)
                            {
                                if (game.PlayerId[teamSelection] == existed)
                                {
                                    errorText = "已参与比赛的选手不允许删除。";
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            selectedTeam.Players = players;
            Save();
            errorText = null;
            return true;
        }

    }
}
