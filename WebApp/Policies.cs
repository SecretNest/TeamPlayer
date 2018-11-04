using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
	/// <summary>
	/// 定义策略名称。该类型为静态类型。
	/// </summary>
	public static class Policies
	{
		/// <summary>
		/// 表示执行管理操作的策略。该字段为常量。
		/// </summary>
		public const string Manage = nameof(Manage);
	}
}
