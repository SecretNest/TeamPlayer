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
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Basis(Basis model)
		{
			if (ModelState.IsValid)
			{
				FacadeService.Facade.SetBasisNameAndDescription(model.Name, model.Description);
				return RedirectToAction("Index", "Home");
			}

			return View();
		}
	}
}