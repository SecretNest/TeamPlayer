using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models.Account;

namespace WebApp.Controllers
{
	public class AccountController : Controller
	{
		public AccountController(IOptions<AppSetting> appSetting)
		{
			AppSetting = appSetting.Value;
		}

		/// <summary>
		/// 应用程序设置。
		/// </summary>
		private AppSetting AppSetting { get; }

		/// <summary>
		/// 显示登录界面。
		/// </summary>
		/// <param name="returnUrl">登录后要返回的地址。</param>
		/// <returns>操作结果。</returns>
		[HttpGet]
		public IActionResult LogOn(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOn(LogOnViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (AppSetting.ManageAccounts.Any(i => i.UserName == model.UserName && i.Password == model.Password))
				{
					var claims = new[]
					{
						new Claim(ClaimTypes.Name, model.UserName),
						new Claim(ClaimTypes.NameIdentifier, model.UserName),
					};

					var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme, ClaimTypes.Name, ClaimTypes.Role);
					var principal = new ClaimsPrincipal(identity);
					await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

					if (!Url.IsLocalUrl(returnUrl))
					{
						returnUrl = Url.Action("Index", "Home");
					}

					return Redirect(returnUrl);
				}
			}

			return View(model);
		}

		/// <summary>
		/// 执行注销操作。
		/// </summary>
		/// <returns>表示异步操作的任务。</returns>
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOff()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}