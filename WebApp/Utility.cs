﻿using System;
using System.Collections.Generic;
using System.Linq;
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
			TValue defaultValue = default(TValue))
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
	}
}
