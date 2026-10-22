using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core
{
	[AppSingleton]
	public abstract class PoolManager<TType, TItem> : MonoSingleton<TType> where TType : PoolManager<TType, TItem> where TItem : IPoolItem
	{
		[SerializeField]
		protected int _maxSize;

		private int _currentSize;

		[SerializeField]
		protected bool _dynamic;

		private Stack<TItem> _pools;

		protected override void OnCreate()
		{
			_pools = new Stack<TItem>();
			if (!_dynamic)
			{
				GeneratePool();
			}
		}

		private void GeneratePool()
		{
			for (int i = 0; i < _maxSize; i++)
			{
				AddItem();
			}
		}

		private void AddItem()
		{
			TItem t = CreateItem();
			t.OnReleaseEvent += Push;
			t.OnActive(false);
			_pools.Push(t);
			_currentSize++;
		}

		protected abstract TItem CreateItem();

		protected TItem Pop()
		{
			if (_pools.Count == 0 && _currentSize != _maxSize)
			{
				AddItem();
			}
			TItem result = default(TItem);
			if (_pools.Count != 0)
			{
				result = _pools.Pop();
				result.OnActive(true);
			}
			return result;
		}

		protected virtual void OnPushed(IPoolItem item)
		{
		}

		private void Push(IPoolItem item)
		{
			if (!_pools.Contains((TItem)item))
			{
				_pools.Push((TItem)item);
				OnPushed(item);
				item.OnActive(false);
			}
		}
	}
}
