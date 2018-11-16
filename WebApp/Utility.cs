using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using WebApp.Models;

namespace WebApp
{
	/// <summary>
	/// 提供辅助方法。该类型为静态类型。
	/// </summary>
	public static class Utility
	{
		/// <summary>
		/// 获取表示导致异常的最终原因的消息。
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static string GetBaseMessage(this Exception exception)
		{
			return exception.GetBaseException().Message;
		}

		/// <summary>
		/// 获取字典中具有给定键的值。如果值不存在则返回一个默认值。
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictionary"></param>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static TValue GetValueOfDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key,
			TValue defaultValue = default)
		{
			return dictionary.TryGetValue(key, out var result) ? result : defaultValue;
		}

		public static IEnumerable<KeyedItem<TKey, TValue>> ToKeyed<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
		{
			return dictionary.Select(i => new KeyedItem<TKey, TValue>(i.Key, i.Value));
		}

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
			this IEnumerable<KeyedItem<TKey, TValue>> items)
		{

			return new Dictionary<TKey, TValue>(items.Select(i => new KeyValuePair<TKey, TValue>(i.Key, i.Value)));
		}

		/// <summary>
		/// 获取枚举选项的描述性名称。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumItem"></param>
		/// <returns></returns>
		public static string GetDisplayString<T>(this T enumItem) where T : Enum
		{
			var memberName = typeof(T).GetEnumName(enumItem);
			var member = typeof(T).GetField(memberName,
				BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Instance);

			var displayAttr = member.GetCustomAttribute<DisplayAttribute>();

			return displayAttr?.Name ?? memberName;
		}

		/// <summary>
		/// 产生一个双循环序列。循环具有指定的长度，并且按照给定数量打断。
		/// </summary>
		/// <param name="maxCount"></param>
		/// <param name="roundCount"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<int>> DoubleRange(int maxCount, int roundCount)
		{
			if (roundCount <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(roundCount), roundCount, "每个循环的数据个数必须为正整数。");
			}

			var start = 0;
			while (start < maxCount)
			{
				var end = Math.Min(maxCount, start + roundCount);
				yield return Enumerable.Range(start, end - start);
				start = end;
			}
		}
	}
}
