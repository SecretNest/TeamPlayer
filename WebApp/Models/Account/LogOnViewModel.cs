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
		public string UserName { get; set; }

		/// <summary>
		/// 登录的密码。
		/// </summary>
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
