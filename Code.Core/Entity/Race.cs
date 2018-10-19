using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class Race
    {
        //种族名称
        public string Name { get; set; }
        //是否可以用于队员默认种族的选择
        public bool IsInPlayerDefault { get; set; }
        //是否可以用于比赛种族的选择
        public bool IsInGame { get; set; }
        //如果IsInPlayerDefault真，IsInGame假，则在比赛内将此种族转换为此设置的目标种族（必须存在且IsInGame真）
        public Guid TargetIdInGameConverting { get; set; } 
    }
}
