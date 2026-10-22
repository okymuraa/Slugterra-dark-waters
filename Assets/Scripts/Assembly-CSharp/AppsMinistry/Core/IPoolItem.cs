using System;

namespace AppsMinistry.Core
{
	public interface IPoolItem
	{
		event Action<IPoolItem> OnReleaseEvent;

		void OnRelease();

		void OnActive(bool active);
	}
}
