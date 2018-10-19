using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class Score
    {
        public Dictionary<TeamSelection, string> TeamName { get; set; }

        //当前确定分值
        public Dictionary<TeamSelection, int> CurrentSource { get; set; }

        //考虑最低出场次数后的分值
        public Dictionary<TeamSelection, int> GuaranteedSource { get; set; }

        //每轮得分
        public List<ScorePerRound> Rounds { get; set; }

        //队员成绩
        public List<PlayerWithGameCount> Players { get; set; }
    }

    public class ScorePerRound
    {
        //轮
        public int Index { get; set; }

        //本轮得分
        public Dictionary<TeamSelection, int> Score { get; set; }

        //每场得分
        public List<ScoreLine> Games { get; set; }
    }

    public class ScoreLine
    {
        //场
        public int Index { get; set; }

        public Dictionary<TeamSelection, Guid> PlayerId { get; set; }
        public Dictionary<TeamSelection, string> PlayerName { get; set; }

        public TeamSelection Winner { get; set; }
    }
}
