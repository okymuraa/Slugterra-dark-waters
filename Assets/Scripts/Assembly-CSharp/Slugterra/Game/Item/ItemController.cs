using AppsMinistry.Core.Sound;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Item
{
	public class ItemController : MonoBehaviour
	{
		[SerializeField]
		private bool _canRespawn;

		[SerializeField]
		private float _respawnTime;

		[SerializeField]
		private SoundContainer _pickUpSound;

		private bool _needRespawn;

		private float _nextRespawn;

		private Transform _transform;

		protected Transform BaseTransform
		{
			get
			{
				return _transform;
			}
		}

		protected virtual void OnStart()
		{
		}

		private void Start()
		{
			_transform = transform;
			_needRespawn = false;
			OnStart();
			if (_pickUpSound != null)
			{
				_pickUpSound.RegisterInSoundManager();
			}
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
			if (_needRespawn && _nextRespawn <= Time.time)
			{
				_needRespawn = false;
				gameObject.SetActive(true);
			}
		}

		private void Delete()
		{
			gameObject.SetActive(false);
			if (_canRespawn)
			{
				_needRespawn = true;
				_nextRespawn = Time.time + _respawnTime;
			}
		}

		protected virtual void OnPickUp(Unit unit)
		{
		}

		public void PickUp(Unit unit)
		{
			OnPickUp(unit);
			PlayPickUpSound();
			Delete();
		}

		public SoundItem PlayPickUpSound()
		{
			return (!(_pickUpSound != null)) ? null : _pickUpSound.Play();
		}
	}
}
