using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class GameWithIndex : Game
    {
        public int RoundIndex { get; set; }
        public int GameIndex { get; set; }
    }
}
