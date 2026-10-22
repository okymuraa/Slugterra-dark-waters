using UnityEngine;
using UnityEngine.Audio;

namespace AppsMinistry.Core.Sound
{
	public class SoundContainer : ScriptableObject
	{
		[SerializeField]
		private SoundInfo[] _sounds;

		[SerializeField]
		[Space(20f)]
		private AudioMixerGroup _output;

		[SerializeField]
		private bool _loop;

		[Range(-3f, 3f)]
		[SerializeField]
		private float _pitch = 1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _volume = 1f;

		private bool _registered;

		public void OnEnable()
		{
			_registered = false;
		}

		public void RegisterInSoundManager()
		{
			if (_sounds != null && !_registered)
			{
				SoundInfo[] sounds = _sounds;
				foreach (SoundInfo soundInfo in sounds)
				{
					MonoSingleton<SoundManager>.Instance.RegisterSound(soundInfo.name, soundInfo.path);
				}
				_registered = true;
			}
		}

		private string GetRandomName()
		{
			string empty = string.Empty;
			if (_sounds != null && _sounds.Length > 0)
			{
				empty = _sounds[Random.Range(0, _sounds.Length)].name;
			}
			else
			{
				Debug.LogWarningFormat("Sound container is empty: {0}", name);
			}
			return empty;
		}

		public SoundItem Play()
		{
			return MonoSingleton<SoundManager>.Instance.Play(GetRandomName(), _volume, _pitch, _loop, _output);
		}
	}
}
