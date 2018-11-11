using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretNest.TeamPlayer.Entity;

namespace WebApp.Controllers
{
	/// <summary>
	/// 提供管理操作。
	/// </summary>
	[Authorize(Policies.Manage)]
	public class ManageController : Controller
	{
		public ManageController(FacadeService facadeService)
		{
			FacadeService = facadeService;
		}

		/// <summary>
		/// 后台数据服务。
		/// </summary>
		private FacadeService FacadeService { get; }

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
			if (ModelState.IsValid)
			{
				FacadeService.Facade.SetBasisNameAndDescription(model.Name, model.Description);
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
			if (ModelState.IsValid)
			{
				FacadeService.Facade.SetBasisRoundAndGame(model.RoundCount, model.GameCount);
			}

			return View("Basis", model);
		}
	}
}