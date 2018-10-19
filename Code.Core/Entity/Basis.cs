using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class Basis
    {
        //比赛名称
        public string Name { get; set; }
        //比赛描述，多行文本
        public string Description { get; set; }

        //轮，设置会导致重置比赛数据
        public int RoundCount { get; set; }
        //每轮比赛数，设置会导致重置比赛数据
        public int GameCount { get; set; }

        //种族基础数据
        public Dictionary<Guid, Race> Races { get; set; }
        //地图基础数据
        public Dictionary<Guid, Map> Maps { get; set; }
    }
}
