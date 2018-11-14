using System;

namespace WebApp.Models
{
	public class ErrorViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}

	public class KeyedItem<TKey, TValue>
	{
		public TKey Key { get; set; }
		public TValue Value { get; set; }

		public KeyedItem()
		{

		}

		public KeyedItem(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}
	}
}