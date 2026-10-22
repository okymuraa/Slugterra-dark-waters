using System;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class SpawnEffectTriggerAbility : SlugAbility
	{
		[SerializeField]
		private GameObject _spawnTriggerPrefab;

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			if (_spawnTriggerPrefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(_spawnTriggerPrefab, unitTransform.position, Quaternion.identity);
				gameObject.layer = GameObject.layer;
			}
		}
	}
}
