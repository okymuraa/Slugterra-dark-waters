using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Plugins;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class ArenaView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMainMenuEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRestartEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[SerializeField]
		private tk2dUIToggleButton btnSound;

		[SerializeField]
		private tk2dUIToggleButton btnMusic;

		[SerializeField]
		private GameObject _saleInStore;

		private GameSetting _settings;

		private float _sfxVolume = 0.57f;

		private float _musicVolume = 0.5f;

		public event Action OnPlayEvent
		{
			add
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMainMenuEvent
		{
			add
			{
				Action action = OnMainMenuEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMainMenuEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMainMenuEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMainMenuEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMapEvent
		{
			add
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRestartEvent
		{
			add
			{
				Action action = OnRestartEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestartEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRestartEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestartEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnGameServiceEvent
		{
			add
			{
				Action action = OnBtnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnGameServiceEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnGameServiceEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
		}

		private void OnEnable()
		{
			if (_settings == null)
			{
				_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			}
			if (btnMusic != null)
			{
				btnMusic.IsOn = _settings.MusicVolume > 0f;
			}
			if (btnSound != null)
			{
				btnSound.IsOn = _settings.SoundFxVolume > 0f;
			}
		}

		private void OnBtnMapClick()
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel != null && currentGameLevel.Status == LevelStatus.Completed)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.ExitFromLevel();
			}
			else
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.LevelUncomplete();
			}
			if (OnMapEvent__BackingField != null)
			{
				OnMapEvent__BackingField();
			}
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnBtnMenuClick()
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel != null && currentGameLevel.Status == LevelStatus.Completed)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.ExitFromLevel();
			}
			else
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.LevelUncomplete();
			}
			if (OnMainMenuEvent__BackingField != null)
			{
				OnMainMenuEvent__BackingField();
			}
		}

		private void OnBtnRestartClick()
		{
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.RestartLevel();
			if (OnRestartEvent__BackingField != null)
			{
				OnRestartEvent__BackingField();
			}
		}

		private void OnBtnResumeClick()
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnBtnGameServiceEvent__BackingField != null)
			{
				OnBtnGameServiceEvent__BackingField();
			}
		}

		private void OnToggleMusic(tk2dUIToggleButton button)
		{
			if (!button.IsOn)
			{
				if (_settings.MusicVolume > 0f)
				{
					_musicVolume = _settings.MusicVolume;
				}
				_settings.MusicVolume = 0f;
			}
			else
			{
				_settings.MusicVolume = _musicVolume;
			}
		}

		private void OnToggleSound(tk2dUIToggleButton button)
		{
			if (!button.IsOn)
			{
				if (_settings.SoundFxVolume > 0f)
				{
					_sfxVolume = _settings.SoundFxVolume;
				}
				_settings.SoundFxVolume = 0f;
			}
			else
			{
				_settings.SoundFxVolume = _sfxVolume;
			}
		}
	}
}
