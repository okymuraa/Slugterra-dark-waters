using System;
using AppsMinistry.UI;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Settings
{
	public class SoundSettingsView : UIView
	{
		public Action<float> OnSoundVolumeChangeEvent;

		public Action<float> OnMusicVolumeChangeEvent;

		[SerializeField]
		private tk2dUIScrollbar _soundScrollbar;

		[SerializeField]
		private tk2dUIScrollbar _musicScrollbar;

		[SerializeField]
		private tk2dBaseSprite _soundIcon;

		[SerializeField]
		private tk2dBaseSprite _musicIcon;

		public void OnApplyModel(GameSetting settings)
		{
			if (settings != null)
			{
				_soundScrollbar.Value = settings.SoundFxVolume;
				_musicScrollbar.Value = settings.MusicVolume;
			}
		}

		private void OnSoundSliderValueChanged(tk2dUIScrollbar scroll)
		{
			if (OnSoundVolumeChangeEvent != null)
			{
				OnSoundVolumeChangeEvent(scroll.Value);
			}
		}

		private void OnMusicSliderValueChanged(tk2dUIScrollbar scroll)
		{
			if (OnMusicVolumeChangeEvent != null)
			{
				OnMusicVolumeChangeEvent(scroll.Value);
			}
		}
	}
}
