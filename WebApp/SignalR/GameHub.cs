using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApp.SignalR
{
	/// <summary>
	/// 定义集线器所需要的方法。
	/// </summary>
	public class GameHub : Hub<IGameHubClient>
	{
	}

	/// <summary>
	/// 定义集线器客户端所需要的方法。
	/// </summary>
	public interface IGameHubClient
	{
		/// <summary>
		/// 通知客户端最近比赛已经更新。
		/// </summary>
		/// <returns>表示异步操作的任务。</returns>
		Task RecentGameUpdated();
	}
}
