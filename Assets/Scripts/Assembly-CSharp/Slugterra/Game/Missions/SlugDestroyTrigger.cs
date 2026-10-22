using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Sound;
using Slugterra.Configuration;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Missions
{
	public class SlugDestroyTrigger : BaseMonoBehaviour
	{
		[SerializeField]
		private bool DestroyAllSlugs = true;

		[SerializeField]
		private List<SlugType> _types;

		[SerializeField]
		private GameObject _fxObject;

		[SerializeField]
		private GameObject[] _SpawnObjects;

		[SerializeField]
		private SoundContainer _destroySound;

		private HitMarker _hitMarker;

		protected override void OnCreate()
		{
			base.OnCreate();
			_hitMarker = GetComponent<HitMarker>();
			if (_hitMarker == null)
			{
				_hitMarker = gameObject.AddComponent<HitMarker>();
				Transform transform = new GameObject("HitMarker").transform;
				transform.parent = base.transform;
				transform.localPosition = Vector3.up * 0.5f;
				_hitMarker.Markers.Add(transform);
			}
			if (_destroySound != null)
			{
				_destroySound.RegisterInSoundManager();
			}
		}

		private void OnTriggerEnter(Collider collider)
		{
			SlugController component = collider.transform.GetComponent<SlugController>();
			if ((!(component != null) || !_types.Contains(component.Model.Type)) && (!(component != null) || !DestroyAllSlugs || component.Model.Type == SlugType.Flopper))
			{
				return;
			}
			if (_fxObject != null)
			{
				GameObject gameObject = Object.Instantiate(_fxObject, transform.position, Quaternion.identity);
				gameObject.SetActive(true);
				if (_SpawnObjects != null && _SpawnObjects.Length > 0)
				{
					for (int i = 0; i < _SpawnObjects.Length; i++)
					{
						if (_SpawnObjects[i] != null)
						{
							Object.Instantiate(_SpawnObjects[i], transform.position, transform.rotation);
						}
					}
				}
			}
			if (_destroySound != null)
			{
				_destroySound.Play();
			}
			Object.Destroy(GameObject);
		}
	}
}
