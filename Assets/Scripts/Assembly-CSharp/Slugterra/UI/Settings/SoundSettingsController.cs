using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;

namespace Slugterra.UI.Settings
{
	public class SoundSettingsController : UIController<SoundSettingsView>
	{
		private GameSetting _settings;

		protected override void OnCreate()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
		}

		protected override void _OnEnable()
		{
			View.OnApplyModel(_settings);
			SoundSettingsView view = View;
			view.OnMusicVolumeChangeEvent = (Action<float>)Delegate.Combine(view.OnMusicVolumeChangeEvent, new Action<float>(OnMusicVolumChange));
			SoundSettingsView view2 = View;
			view2.OnSoundVolumeChangeEvent = (Action<float>)Delegate.Combine(view2.OnSoundVolumeChangeEvent, new Action<float>(OnSoundVolumeChange));
		}

		protected override void _OnDisable()
		{
			SoundSettingsView view = View;
			view.OnMusicVolumeChangeEvent = (Action<float>)Delegate.Remove(view.OnMusicVolumeChangeEvent, new Action<float>(OnMusicVolumChange));
			SoundSettingsView view2 = View;
			view2.OnSoundVolumeChangeEvent = (Action<float>)Delegate.Remove(view2.OnSoundVolumeChangeEvent, new Action<float>(OnSoundVolumeChange));
		}

		private void OnMusicVolumChange(float value)
		{
			if (_settings != null)
			{
				_settings.MusicVolume = value;
			}
		}

		private void OnSoundVolumeChange(float value)
		{
			if (_settings != null)
			{
				_settings.SoundFxVolume = value;
			}
		}
	}
}
