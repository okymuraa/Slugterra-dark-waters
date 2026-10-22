using System;
using System.Collections;
using Slugterra.Scene;
using UnityEngine;
using UnityEngine.Audio;

namespace Slugterra
{
	[RequireComponent(typeof(AudioSource))]
	public class CutScenesMusicManager : MonoBehaviour
	{
		[Serializable]
		public class CutSceneMusic
		{
			public string CutSceneName;

			public string MusicName;
		}

		public const string ResourcesPath = "Sounds/music/";

		[SerializeField]
		private AudioMixerSnapshot _backgroundMusic;

		[SerializeField]
		private AudioMixerSnapshot _cutSceneMusic;

		[SerializeField]
		private float _snapshotTransitionTime = 0.5f;

		[SerializeField]
		private CutSceneMusic[] _music;

		private AudioSource _audioSource;

		private void Awake()
		{
			if (transform.parent == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
			_audioSource = GetComponent<AudioSource>();
			SlugterraCutScene.OnPlayCutSceneEvent += OnPlayCutScene;
		}

		private AudioClip GetClip(string cutSceneName)
		{
			string text = string.Empty;
			if (_music != null && _music.Length > 0)
			{
				CutSceneMusic cutSceneMusic = Array.Find(_music, (CutSceneMusic x) => x.CutSceneName == cutSceneName);
				if (cutSceneMusic != null)
				{
					text = cutSceneMusic.MusicName;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				return Resources.Load<AudioClip>("Sounds/music/" + text);
			}
			return null;
		}

		private void OnPlayCutScene(object cutScene, bool isPlaying)
		{
			if (isPlaying)
			{
				AudioClip audioClip = null;
				if (cutScene != null)
				{
					audioClip = GetClip(cutScene.GetType().Name);
				}
				if (audioClip != null)
				{
					_audioSource.Stop();
					_audioSource.clip = audioClip;
					_audioSource.Play();
					if (_cutSceneMusic != null)
					{
						_cutSceneMusic.TransitionTo(_snapshotTransitionTime);
					}
				}
			}
			else
			{
				if (_backgroundMusic != null)
				{
					_backgroundMusic.TransitionTo(_snapshotTransitionTime);
				}
				StartCoroutine("StopAudioSource", _snapshotTransitionTime);
			}
		}

		private IEnumerator StopAudioSource(float delay)
		{
			yield return new WaitForSeconds(delay);
			_audioSource.Stop();
		}
	}
}
