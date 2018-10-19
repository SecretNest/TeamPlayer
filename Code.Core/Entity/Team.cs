using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class Team
    {
        //队伍名称
        public string Name { get; set; }

        //队员
        public Dictionary<Guid, Player> Players { get; set; }
    }
}
