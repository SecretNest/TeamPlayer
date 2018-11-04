using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Account
{
	/// <summary>
	/// 登录使用数据模型。
	/// </summary>
	public class LogOnViewModel
	{
		/// <summary>
		/// 登录的用户名。
		/// </summary>
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "用户名")]
		public string UserName { get; set; }

		/// <summary>
		/// 登录的密码。
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "密码")]
		public string Password { get; set; }
	}
}
