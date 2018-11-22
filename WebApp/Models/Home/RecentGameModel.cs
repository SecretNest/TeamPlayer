using SecretNest.TeamPlayer.Entity;
using System.Collections.Generic;

namespace WebApp.Models.Home
{
	public class RecentGameModel
	{
		public GameWithIndex LastGame { get; set; }
		public IEnumerable<GameWithIndex> CurrentGames { get; set; }
	}
}
