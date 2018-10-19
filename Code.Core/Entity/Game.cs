using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class Game
    {
        //参赛者，Empty表示未选择，允许更新为Empty
        public Guid Team1PlayerId { get; set; }
        public Guid Team2PlayerId { get; set; }

        //种族选择，Empty表示未选择，允许更新为Empty
        public Guid Player1RaceId { get; set; }
        public Guid Player2RaceId { get; set; }

        //比赛日期与时间，设置为最小值表示未设置（不显示）；如果不包括时间，时间设置为0点
        public DateTime GameTime { get; set; }
        //GameTime是否包含时间（假则显示时忽略时间）
        public bool IsTimeIncluded { get; set; }

        //比赛地图，Empty表示未选择，允许更新为Empty
        public Guid MapId { get; set; }

        //比赛状态
        public GameResult GameResult { get; set; }
    }

    public enum GameResult : int
    {
        //未就绪（默认）
        NotStarted = 0,
        //进行中
        InGame = 1,
        //1队胜利
        Team1Won = 2,
        //2队胜利
        Team2Won = 3,
    }
}
