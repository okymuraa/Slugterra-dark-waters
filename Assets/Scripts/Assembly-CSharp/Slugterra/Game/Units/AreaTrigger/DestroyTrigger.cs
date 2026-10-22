using UnityEngine;

namespace Slugterra.Game.Units.AreaTrigger
{
	public class DestroyTrigger : AreaTrigger
	{
		private void OnEnable()
		{
			OnTriggerEnterEvent += OnDestroyUnit;
		}

		private void OnDisable()
		{
			OnTriggerEnterEvent -= OnDestroyUnit;
		}

		private void OnDestroyUnit(Transform item)
		{
			Object.Destroy(item.gameObject);
			Object.Destroy(gameObject);
		}
	}
}
