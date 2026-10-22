using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AppsMinistry.Core.Sound
{
	public sealed class SoundManager : PoolManager<SoundManager, SoundItem>
	{
		private GameObject _soundPrefab;

		private Dictionary<string, string> _sounds;

		private Dictionary<string, AudioClip> _loadedSound;

		private GameObject soundPrefab
		{
			get
			{
				if (_soundPrefab == null)
				{
					_soundPrefab = Resources.Load("Prefabs/Sounds/SoundItem") as GameObject;
				}
				return _soundPrefab;
			}
		}

		protected override void OnCreate()
		{
			_sounds = new Dictionary<string, string>();
			_loadedSound = new Dictionary<string, AudioClip>();
			_maxSize = 15;
			_dynamic = false;
			base.OnCreate();
		}

		protected override SoundItem CreateItem()
		{
			GameObject gameObject = Object.Instantiate(soundPrefab);
			gameObject.transform.parent = base.gameObject.transform;
			if (gameObject.transform.parent == null)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			SoundItem component = gameObject.GetComponent<SoundItem>();
			if (component == null)
			{
				Debug.LogWarning("Sound item does not have SoundItem.cs");
			}
			return component;
		}

		protected override void OnPushed(IPoolItem item)
		{
			if (!MonoSingleton<SoundManager>.IsQuit)
			{
				((SoundItem)item).transform.parent = gameObject.transform;
			}
		}

		public void RegisterSound(string name, string path)
		{
			if (_sounds.ContainsKey(name))
			{
				Debug.LogWarning("Sound already exist: " + name);
				return;
			}
			_sounds[name] = path;
			_loadedSound[name] = Resources.Load(path) as AudioClip;
		}

		public SoundItem Play(string name)
		{
			return Play(name, 1f, false);
		}

		public SoundItem Play(string name, float volume)
		{
			return Play(name, volume, false);
		}

		public SoundItem Play(string name, bool loop)
		{
			return Play(name, 1f, loop);
		}

		public SoundItem Play(string name, float volume, bool loop)
		{
			return Play(name, volume, 1f, loop, null);
		}

		public SoundItem Play(string name, float volume, float pitch, bool loop, AudioMixerGroup output)
		{
			SoundItem soundItem = Pop();
			if (soundItem != null)
			{
				AudioClip value = null;
				if (_loadedSound.TryGetValue(name, out value))
				{
					soundItem.Source.clip = value;
					soundItem.Source.volume = volume;
					soundItem.Source.loop = loop;
					soundItem.Source.pitch = pitch;
					if (output != null)
					{
						soundItem.Source.outputAudioMixerGroup = output;
					}
					soundItem.Source.Play();
				}
				else
				{
					Debug.LogWarning("Sound not found: " + name);
				}
			}
			return soundItem;
		}
	}
}
