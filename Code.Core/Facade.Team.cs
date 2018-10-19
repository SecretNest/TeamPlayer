using SecretNest.TeamPlayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer
{
    public partial class Facade
    {
        public Team GetTeam(int number)
        {
            if (number == 1)
                return dataFile.Team1;
            else if (number == 2)
                return dataFile.Team2;
            else
                throw new NotSupportedException();
        }

        public string GetTeamName(int number)
        {
            if (number == 1)
                return dataFile.Team1.Name;
            else if (number == 2)
                return dataFile.Team2.Name;
            else
                throw new NotSupportedException();
        }

        public Player GetPlayer(int number, Guid playerId)
        {
            if (number == 1)
                return dataFile.Team1.Players[playerId];
            else if (number == 2)
                return dataFile.Team2.Players[playerId];
            else
                throw new NotSupportedException();
        }

        public void SetTeamName(int number, string name)
        {
            if (number == 1)
                dataFile.Team1.Name = name;
            else if (number == 2)
                dataFile.Team2.Name = name;
            else
                throw new NotSupportedException();
            Save();
        }

        public bool SetTeamPlayers(int number, Dictionary<Guid, Player> players, out string errorText)
        {
            Team selectedTeam;
            if (number == 1)
                selectedTeam = dataFile.Team1;
            else if (number == 2)
                selectedTeam = dataFile.Team2;
            else
                throw new NotSupportedException();

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
                                if ((number == 1 && game.Team1PlayerId == existed) || (number == 2 && game.Team2PlayerId == existed))
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
