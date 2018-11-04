using Microsoft.Extensions.Options;
using SecretNest.TeamPlayer;

namespace WebApp
{
	/// <summary>
	/// 为 Facade 对象提供服务实现。
	/// </summary>
	public class FacadeService
	{

		/// <summary>
		/// 初始化一个 <see cref="FacadeService"/> 对象的新实例。
		/// </summary>
		/// <param name="appSetting">应用程序设置。</param>
		public FacadeService(IOptions<AppSetting> appSetting)
		{
			Facade = new Facade(appSetting.Value.DataFilePath);
		}

		/// <summary>
		/// 获取应用程序服务对象。
		/// </summary>
		public Facade Facade { get; }
	}
}
