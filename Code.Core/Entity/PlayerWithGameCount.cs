using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class PlayerWithGameCount : Player
    {
        //PlayerId
        public Guid Id { get; set; }

        //Team
        public TeamSelection Team { get; set; }

        //已参加比赛数量
        public int Played { get; set; }

        //罚分情况，0时显示为-
        public int PlayedBelowMinAttending { get; set; }
        public int PlayedAboveMaxAttending { get; set; }


        //胜利数量
        public int Won { get; set; }

        //失败数量
        public int Lost { get; set; }
    }
}
