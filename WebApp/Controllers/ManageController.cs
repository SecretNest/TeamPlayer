using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sakura.AspNetCore;
using SecretNest.TeamPlayer.Entity;
using System;
using System.Globalization;
using WebApp.Models;

namespace WebApp.Controllers
{
	/// <summary>
	/// 提供管理操作。
	/// </summary>
	[Authorize(Policies.Manage)]
	public class ManageController : Controller
	{
		public ManageController(FacadeService facadeService, IOperationMessageAccessor operationMessageAccessor)
		{
			FacadeService = facadeService;
			OperationMessageAccessor = operationMessageAccessor;
		}

		/// <summary>
		/// 后台数据服务。
		/// </summary>
		private FacadeService FacadeService { get; }

		/// <summary>
		/// 消息服务。
		/// </summary>
		private IOperationMessageAccessor OperationMessageAccessor { get; }

		/// <summary>
		/// 显示管理基本信息界面。
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Basis()
		{
			return View(FacadeService.Facade.GetBasis());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateBasis(Basis model)
		{
			if (string.IsNullOrEmpty(model.Name))
			{
				ModelState.AddModelError(nameof(model.Name), "必须填写比赛名称。");
			}

			if (ModelState.IsValid)
			{
				FacadeService.Facade.SetBasisNameAndDescription(model.Name, model.Description);
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功", "比赛基本信息已经更新。");
				return RedirectToAction("Basis", "Manage");
			}

			return View("Basis", model);
		}

		/// <summary>
		/// 更新比赛轮次。
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateRounds(Basis model)
		{
			if (model.RoundCount <= 0)
			{
				ModelState.AddModelError(nameof(model.RoundCount), "轮数必须为正整数。");
			}

			if (model.GameCount <= 0)
			{
				ModelState.AddModelError(nameof(model.GameCount), "局数必须为正整数。");
			}

			if (ModelState.IsValid)
			{
				FacadeService.Facade.SetBasisRoundAndGame(model.RoundCount, model.GameCount);
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功",
					"比赛轮次信息已经更新。以前的轮次信息已经被清除。");
				return RedirectToAction("Basis", "Manage");
			}

			return View("Basis", model);
		}


		/// <summary>
		/// 显示地图列表。
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Map()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Map(KeyedItem<Guid, Map>[] model)
		{
			// 为所有项目创建新Key。
			foreach (var item in model)
			{
				if (item.Key == Guid.Empty)
				{
					item.Key = Guid.NewGuid();
				}
			}

			if (FacadeService.Facade.SetBasisMaps(model.ToDictionary(), out var error))
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功", "地图列表已经更新。");
			}
			else
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Error, "操作失败", error);
			}

			return RedirectToAction("Map", "Manage");
		}


		/// <summary>
		/// 显示地图列表。
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Race()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Race(KeyedItem<Guid, Race>[] model)
		{
			// 为所有项目创建新Key。
			foreach (var item in model)
			{
				if (item.Key == Guid.Empty)
				{
					item.Key = Guid.NewGuid();
				}
			}


			if (FacadeService.Facade.SetBasisRaces(model.ToDictionary(), out var error))
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功", "种族列表已经更新。");
			}
			else
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Error, "操作失败", error);
			}

			return RedirectToAction("Race", "Manage");
		}


		/// <summary>
		/// 队伍。
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Team(TeamSelection teamSelection)
		{
			var data = FacadeService.Facade.GetTeam(teamSelection);
			ViewBag.TeamSelection = teamSelection;
			return View(data);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateTeamName(TeamSelection teamSelection, Team model)
		{
			if (string.IsNullOrEmpty(model.Name))
			{
				ModelState.AddModelError(nameof(model.Name), "必须提供队伍名称。");
			}

			if (ModelState.IsValid)
			{
				FacadeService.Facade.SetTeamName(teamSelection, model.Name);
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功", "队伍名称已经更新");
				return RedirectToAction("Team", "Manage", new { teamSelection });
			}

			ViewBag.TeamSelection = teamSelection;
			return View("Team", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult UpdateTeamPlayers(TeamSelection teamSelection, KeyedItem<Guid, Player>[] model)
		{
			// 为所有项目创建新Key。
			foreach (var item in model)
			{
				if (item.Key == Guid.Empty)
				{
					item.Key = Guid.NewGuid();
				}
			}


			if (FacadeService.Facade.SetTeamPlayers(teamSelection, model.ToDictionary(), out var error))
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功", "参赛者列表已经更新。");
			}
			else
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Error, "操作失败", error);
			}

			return RedirectToAction("Team", "Manage", new { teamSelection });
		}

		/// <summary>
		/// 获取给定轮次的结果信息。
		/// </summary>
		/// <returns></returns>
		public IActionResult Round(int round = 1)
		{
			ViewBag.Round = round;
			return View(FacadeService.Facade.GetGamesByRound(round - 1));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateResult(int roundIndex, int gameIndex, Game game)
		{
			if (FacadeService.Facade.SetGame(roundIndex, gameIndex, game, out var error))
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功",
					string.Format(CultureInfo.CurrentUICulture, "第 {0} 轮第 {1} 场的比赛信息已经更新", roundIndex + 1, gameIndex + 1));
			}
			else
			{
				OperationMessageAccessor.Messages.Add(OperationMessageLevel.Error, "操作失败",
					string.Format(CultureInfo.CurrentUICulture, "第 {0} 轮第 {1} 场的比赛信息更新失败，原因：{2}", roundIndex + 1, gameIndex + 1, error));

			}

			return RedirectToAction("Round", "Manage", new { round = roundIndex + 1 }, (gameIndex + 1).ToString("D"));
		}
	}
}