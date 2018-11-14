using System;

namespace SecretNest.TeamPlayer.Entity
{
	public class Race
	{

		/// <summary>
		/// 种族名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 是否可以用于队员默认种族的选择
		/// </summary>
		public bool CanBePlayerDefault { get; set; }

		/// <summary>
		/// 是否可以用于比赛种族的选择
		/// </summary>
		public bool CanUseInGame { get; set; }
		//
		/// <summary>
		/// 如果IsInPlayerDefault真，IsInGame假，则在比赛内将此种族转换为此设置的目标种族（必须存在且IsInGame真）。
		/// </summary>
		public Guid TargetIdInGameConverting { get; set; }
	}
}
