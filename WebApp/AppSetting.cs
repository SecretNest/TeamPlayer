using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
	/// <summary>
	/// 定义应用程序设置。
	/// </summary>
	public class AppSetting
	{
		/// <summary>
		/// 获取或设置管理账号的集合。
		/// </summary>
		public ManageAccount[] ManageAccounts { get; set; }

		/// <summary>
		/// 获取或设置数据文件的地址。
		/// </summary>
		public string DataFilePath { get; set; }
	}


	/// <summary>
	/// 定义一个管理账号。
	/// </summary>
	public class ManageAccount
	{
		/// <summary>
		/// 管理账号的用户名。
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// 管理账号的密码。
		/// </summary>
		public string Password { get; set; }
	}
}
