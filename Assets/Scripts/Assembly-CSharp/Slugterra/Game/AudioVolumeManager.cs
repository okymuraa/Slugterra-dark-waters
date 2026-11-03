using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace Slugterra.Game
{
	public class AudioVolumeManager : DonDestroyMonoSingleton<AudioVolumeManager>, IObserver
	{
		[SerializeField]
		private AudioMixer _masterMixer;

		[SerializeField]
		private AnimationCurve _volumeCurve;

		private GameSetting _settings;

		private void Reset()
		{
			_volumeCurve = new AnimationCurve(new Keyframe(0f, -80f), new Keyframe(1f, 20f));
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			_settings.AddObserver(this);
			_settings.CommitAndNotify();
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is GameSetting)
			{
				GameSetting gameSetting = observable as GameSetting;
				SetMusicVolume(gameSetting.MusicVolume);
				SetSoundFxVolume(gameSetting.SoundFxVolume);
			}
		}

		private void EnsureMasterMixer()
		{
			if (_masterMixer == null)
			{
				_masterMixer = Resources.Load<AudioMixer>("sounds/mixers/MasterMixer") ?? Resources.Load<AudioMixer>("Sounds/Mixers/MasterMixer");
			}
			if (_volumeCurve == null || _volumeCurve.length == 0)
			{
				_volumeCurve = new AnimationCurve(new Keyframe(0f, -80f), new Keyframe(1f, 20f));
			}
		}

		private void SetMusicVolume(float volume)
		{
			EnsureMasterMixer();
			if (_masterMixer != null)
			{
				float value = _volumeCurve.Evaluate(volume);
				_masterMixer.SetFloat("MusicVolume", value);
				_masterMixer.SetFloat("mposwoH", value);
			}
		}

		private void SetSoundFxVolume(float volume)
		{
			EnsureMasterMixer();
			if (_masterMixer != null)
			{
				float value = _volumeCurve.Evaluate(volume);
				_masterMixer.SetFloat("AmbienceVolume", value);
				_masterMixer.SetFloat("SOprRTH", value);
				_masterMixer.SetFloat("SfxVolume", value);
				_masterMixer.SetFloat("hkkpiSL", value);
			}
		}

		protected override void OnReleaseResource()
		{
			if (_settings != null)
			{
				_settings.RemoveObserver(this);
			}
		}
	}
}
