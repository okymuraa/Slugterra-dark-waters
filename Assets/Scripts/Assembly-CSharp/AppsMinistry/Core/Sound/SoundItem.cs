using UnityEngine;

namespace AppsMinistry.Core.Sound
{
	public sealed class SoundItem : MonoPoolItem
	{
		[SerializeField]
		private AudioSource _source;

		public AudioSource Source
		{
			get
			{
				return _source;
			}
		}

		public float Pitch
		{
			get
			{
				return _source.pitch;
			}
			set
			{
				_source.pitch = value;
			}
		}

		public override void OnRelease()
		{
			_source.Stop();
			_source.clip = null;
			base.OnRelease();
		}

		public override void OnActive(bool active)
		{
			if (!active)
			{
				_source.Stop();
			}
			base.OnActive(active);
		}

		public void Stop()
		{
			OnRelease();
		}

		private void Update()
		{
			if (!_source.isPlaying)
			{
				OnRelease();
			}
		}
	}
}
