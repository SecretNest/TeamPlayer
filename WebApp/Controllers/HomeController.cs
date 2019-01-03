using Microsoft.AspNetCore.Mvc;
using SecretNest.TeamPlayer.Entity;
using System;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Models.Home;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		public HomeController(FacadeService facadeService)
		{
			FacadeService = facadeService;
		}

		/// <summary>
		/// 数据服务对象。
		/// </summary>
		private FacadeService FacadeService { get; }

		[HttpGet]
		public IActionResult Index()
		{
			var data = FacadeService.Facade.GetBasis();
			return View(data);
		}

		[HttpGet("Home/Team/{team}")]
		public IActionResult Team(TeamSelection team)
		{
			ViewBag.TeamSelection = team;
			return View("Team", FacadeService.Facade.GetTeam(team));
		}

		[HttpGet]
		public IActionResult Recent()
		{
			FacadeService.Facade.GetGameForDisplaying(out var lastGame, out var currentGames, out var nextGames);
			return View(new RecentGameModel { LastGame = lastGame, CurrentGames = currentGames, NextGames = nextGames });
		}


		[HttpGet]
		public IActionResult Game(int round = 1)
		{
			ViewBag.Round = round;
			var data = FacadeService.Facade.GetGamesByRound(round - 1);
			return View(data);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		/// <summary>
		/// 显示队员信息。
		/// </summary>
		/// <returns></returns>
		public IActionResult Player(TeamSelection team, Guid id)
		{
			var player = FacadeService.Facade.GetPlayer(team, id);

			if (player == null)
			{
				return NotFound();
			}

			ViewBag.Team = team;
			ViewBag.PlayerId = id;
			return View(player);
		}

		/// <summary>
		/// 总成绩界面。
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Result()
		{
			var data = FacadeService.Facade.GetScore();
			return View(data);
		}
	}
}
