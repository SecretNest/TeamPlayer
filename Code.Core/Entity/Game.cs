using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecretNest.TeamPlayer.Entity
{
	public class Game
	{
		//参赛者，Empty表示未选择，允许更新为Empty
		public Dictionary<TeamSelection, Guid> PlayerIds { get; set; }

		//种族选择，Empty表示未选择，允许更新为Empty
		public Dictionary<TeamSelection, Guid> RaceIds { get; set; }

		//比赛日期与时间，设置为最小值表示未设置（不显示）；如果不包括时间，时间设置为0点
		public DateTime GameTime { get; set; }
		//GameTime是否包含时间（假则显示时忽略时间）
		public bool IsTimeIncluded { get; set; }

		//比赛地图，Empty表示未选择，允许更新为Empty
		public Guid MapId { get; set; }

		//比赛状态
		public GameResult GameResult { get; set; }

		//其他信息
		public string Description { get; set; }

		public bool IsFinished
		{
			get
			{
				switch (GameResult)
				{
					case GameResult.Team1Win:
					case GameResult.Team2Win:
						return true;
					default:
						return false;
				}
			}
		}

		public TeamSelection Winner
		{
			get
			{
				switch (GameResult)
				{
					case GameResult.Team1Win:
						return TeamSelection.Team1;
					case GameResult.Team2Win:
						return TeamSelection.Team2;
					default:
						return 0;
				}
			}
		}
	}

	public enum GameResult 
	{
		//未就绪（默认）
		[Display(Name = "尚未开始")]
		NotStarted = 0,
        [Display(Name = "即将开始")]
        Starting = 2,
        //进行中
        [Display(Name = "正在进行")]
		InGame = 1,
		[Display(Name = "队伍一胜")]
		Team1Win,
		[Display(Name = "队伍二胜")]
		Team2Win
	}
}
