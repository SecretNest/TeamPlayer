using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecretNest.TeamPlayer.Entity
{
	public class Team
	{
		//队伍名称
		public string Name { get; set; }

		//队员
		public Dictionary<Guid, Player> Players { get; set; }
	}

	public enum TeamSelection
	{
		[Display(Name = "队伍一")]
		Team1,
		[Display(Name = "队伍二")]
		Team2
	}
}
