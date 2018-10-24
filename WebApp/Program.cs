using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApp
{
	/// <summary>
	/// 应用程序的主类型。
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// 应用程序的入口方法。
		/// </summary>
		/// <param name="args">应用程序的启动参数。</param>
		[UsedImplicitly]
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		/// <summary>
		/// 创建 Web 宿主对象。
		/// </summary>
		/// <param name="args">应用程序的启动参数。</param>
		/// <returns>表示 Web 宿主的 <see cref="IWebHostBuilder"/> 对象。</returns>
		private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
