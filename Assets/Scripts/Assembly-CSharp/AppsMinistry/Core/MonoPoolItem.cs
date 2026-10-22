using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace AppsMinistry.Core
{
	public abstract class MonoPoolItem : MonoBehaviour, IPoolItem
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<IPoolItem> OnReleaseEvent__BackingField;

		public event Action<IPoolItem> OnReleaseEvent
		{
			add
			{
				Action<IPoolItem> action = OnReleaseEvent__BackingField;
				Action<IPoolItem> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnReleaseEvent__BackingField, (Action<IPoolItem>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<IPoolItem> action = OnReleaseEvent__BackingField;
				Action<IPoolItem> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnReleaseEvent__BackingField, (Action<IPoolItem>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public virtual void OnRelease()
		{
			if (OnReleaseEvent__BackingField != null)
			{
				OnReleaseEvent__BackingField(this);
			}
		}

		public virtual void OnActive(bool active)
		{
			gameObject.SetActive(active);
		}
	}
}
