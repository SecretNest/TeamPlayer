using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp
{
	/// <summary>
	/// 应用程序的启动类型。
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// 初始化一个 <see cref="Startup"/> 对象的新实例。
		/// </summary>
		/// <param name="configuration">应用程序配置对象。</param>
		[UsedImplicitly]
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// 获取应用程序的配置对象。
		/// </summary>
		private IConfiguration Configuration { get; }

		
		/// <summary>
		/// 配置应用程序服务。
		/// </summary>
		/// <param name="services">应用程序服务容器。</param>
		[UsedImplicitly]
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		/// <summary>
		/// 配置应用程序设置。
		/// </summary>
		/// <param name="app">应用程序对象。</param>
		/// <param name="env">应用程序宿主环境。</param>
		[UsedImplicitly]
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
