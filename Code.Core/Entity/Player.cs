using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.TeamPlayer.Entity
{
    public class Player
    {
        //名称
        public string Name { get; set; }
        //默认种族，Empty表示无，允许更新为Empty
        public Guid DefaultRace { get; set; }
        //最大出席次数，0表示无限制，默认0；在非编辑状态下，无限制显示为-
        public int MaxAttending { get; set; }
        //最少出场次数，0表示无限制，默认0；在非编辑状态下，无限制显示为-
        public int MinAttending { get; set; }
    }
}
