using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Facebook;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.UI.Popup;
using TinyTeam.Debuger;
using UnityEngine;

namespace Slugterra.UI.Settings
{
	public class SettingsMenuView : UIView
	{
		[Serializable]
		private struct ChangePlatformObjects
		{
			public GameObject _objectTVOS;

			public GameObject _objectDefault;

			public ChangePlatformObjects(GameObject objectTVOS, GameObject objectDefault)
			{
				_objectTVOS = objectTVOS;
				_objectDefault = objectDefault;
			}

			public void SetAcriveObjects()
			{
				bool flag = false;
				flag = false;
				if (_objectTVOS != null)
				{
					_objectTVOS.SetActive(flag);
				}
				if (_objectDefault != null)
				{
					_objectDefault.SetActive(!flag);
				}
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnСontactUsEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnHomePageEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCreditsEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRestoreEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNerdCorpsLogoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAppsMinistryLogoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnSkipCreditsEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<bool> OnBtnRecordEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<bool> OnBtnFacebookConnectEvent__BackingField;

		[SerializeField]
		private GameObject recordButtionDisabled;

		[SerializeField]
		private GameObject recordButtionForAndroid;

		[SerializeField]
		private tk2dUIToggleButton recordButtion;

		[SerializeField]
		private CreditsPanelBehaviour _creditsPanel;

		[SerializeField]
		private GameObject[] _hideOnCredits;

		[SerializeField]
		private tk2dUIToggleButton btnSound;

		[SerializeField]
		private tk2dUIToggleButton btnMusic;

		[SerializeField]
		private tk2dUIToggleButton btnFacebook;

		[SerializeField]
		private TextMesh _currentLanguage;

		private GameSetting _settings;

		[SerializeField]
		private GameObject _сontactUs;

		[SerializeField]
		private GameObject _homePage;

		[SerializeField]
		private ChangePlatformObjects _btns;

		private float _sfxVolume = 0.57f;

		private float _musicVolume = 0.5f;

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

		public event Action OnСontactUsEvent
		{
			add
			{
				Action action = OnСontactUsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnСontactUsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnСontactUsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnСontactUsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnHomePageEvent
		{
			add
			{
				Action action = OnHomePageEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHomePageEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnHomePageEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHomePageEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnCreditsEvent
		{
			add
			{
				Action action = OnCreditsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCreditsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCreditsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCreditsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRestoreEvent
		{
			add
			{
				Action action = OnRestoreEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestoreEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRestoreEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestoreEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnGameServiceEvent
		{
			add
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNerdCorpsLogoEvent
		{
			add
			{
				Action action = OnNerdCorpsLogoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNerdCorpsLogoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNerdCorpsLogoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNerdCorpsLogoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnAppsMinistryLogoEvent
		{
			add
			{
				Action action = OnAppsMinistryLogoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAppsMinistryLogoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAppsMinistryLogoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAppsMinistryLogoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnSkipCreditsEvent
		{
			add
			{
				Action action = OnBtnSkipCreditsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnSkipCreditsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnSkipCreditsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnSkipCreditsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<bool> OnBtnRecordEvent
		{
			add
			{
				Action<bool> action = OnBtnRecordEvent__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnRecordEvent__BackingField, (Action<bool>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<bool> action = OnBtnRecordEvent__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnRecordEvent__BackingField, (Action<bool>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<bool> OnBtnFacebookConnectEvent
		{
			add
			{
				Action<bool> action = OnBtnFacebookConnectEvent__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnFacebookConnectEvent__BackingField, (Action<bool>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<bool> action = OnBtnFacebookConnectEvent__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnFacebookConnectEvent__BackingField, (Action<bool>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			_btns.SetAcriveObjects();
		}

		private void OnEnable()
		{
			if (_settings == null)
			{
				_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			}
			btnMusic.IsOn = _settings.MusicVolume > 0f;
			btnSound.IsOn = _settings.SoundFxVolume > 0f;
			btnFacebook.IsOn = DonDestroyMonoSingleton<FacebookController>.Instance.IsLoggedIn();
			_currentLanguage.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_btn_change_lang");
			recordButtion.gameObject.SetActive(false);
			recordButtionDisabled.SetActive(true);
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				recordButtionDisabled.SetActive(true);
				recordButtionForAndroid.SetActive(false);
			}, (string error) =>
			{
				recordButtionDisabled.SetActive(true);
				recordButtionForAndroid.SetActive(false);
			});
			TinyTeam.Debuger.Console.RegisterCommand("unlockAllLevels", CMDUnlockAllLevels, "Unlock All levels");
		}

		private void OnDisable()
		{
			TinyTeam.Debuger.Console.UnRegisterCommand("unlockAllLevels");
			TinyTeam.Debuger.Console.UnRegisterCommand("showMediationTest");
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnBtnСontactUsClick()
		{
			if (OnСontactUsEvent__BackingField != null)
			{
				OnСontactUsEvent__BackingField();
			}
		}

		private void OnBtnHomePageClick()
		{
			if (OnHomePageEvent__BackingField != null)
			{
				OnHomePageEvent__BackingField();
			}
		}

		private void OnBtnCreditsClick()
		{
			if (OnCreditsEvent__BackingField != null)
			{
				OnCreditsEvent__BackingField();
			}
		}

		private void OnBtnRestoreClick()
		{
			if (OnRestoreEvent__BackingField != null)
			{
				OnRestoreEvent__BackingField();
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnGameServiceEvent__BackingField != null)
			{
				OnGameServiceEvent__BackingField();
			}
		}

		private void OnBtnSkipCreditsClick()
		{
			if (OnBtnSkipCreditsEvent__BackingField != null)
			{
				OnBtnSkipCreditsEvent__BackingField();
			}
		}

		private void OnBtnRecord(tk2dUIToggleButton button)
		{
			bool isOn = button.IsOn;
			if (OnBtnRecordEvent__BackingField != null)
			{
				OnBtnRecordEvent__BackingField(isOn);
			}
			AlphaPulseAnimUI component = button.gameObject.GetComponent<AlphaPulseAnimUI>();
			if (component != null)
			{
				component.LaunchAnimation(isOn);
			}
		}

		private void OnBtnFacebookConnect(tk2dUIToggleButton button)
		{
			bool isOn = button.IsOn;
			if (OnBtnFacebookConnectEvent__BackingField != null)
			{
				OnBtnFacebookConnectEvent__BackingField(isOn);
			}
			AlphaPulseAnimUI component = button.gameObject.GetComponent<AlphaPulseAnimUI>();
			if (component != null)
			{
				component.LaunchAnimation(isOn);
			}
		}

		public void SwitchRecordButton(bool on)
		{
			recordButtion.IsOn = on;
			AlphaPulseAnimUI componentInChildren = recordButtion.gameObject.GetComponentInChildren<AlphaPulseAnimUI>();
			if (componentInChildren != null)
			{
				componentInChildren.LaunchAnimation(on);
			}
		}

		public void OnRecordInAndroidBtn()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				Application.OpenURL("market://details?id=com.appsministry.slugterra");
			}, (string error) =>
			{
			});
		}

		public void StartCredits()
		{
			HideObjectsOnCredits(false);
			_creditsPanel.gameObject.SetActive(true);
			_creditsPanel.OnCloseEvent = StopCredits;
		}

		private void OnAppsMinistryClick()
		{
			if (OnAppsMinistryLogoEvent__BackingField != null)
			{
				OnAppsMinistryLogoEvent__BackingField();
			}
		}

		private void OnNerdCorpsLogoClick()
		{
			if (OnNerdCorpsLogoEvent__BackingField != null)
			{
				OnNerdCorpsLogoEvent__BackingField();
			}
		}

		public void StopCredits()
		{
			HideObjectsOnCredits(true);
			_creditsPanel.gameObject.SetActive(false);
		}

		private void HideObjectsOnCredits(bool show)
		{
			if (_hideOnCredits != null)
			{
				GameObject[] hideOnCredits = _hideOnCredits;
				foreach (GameObject gameObject in hideOnCredits)
				{
					gameObject.SetActive(show);
				}
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

		private void OnBtnSwitchLanguage()
		{
			MonoSingleton<Localization>.Instance.ChangeLanguage();
			_currentLanguage.text = MonoSingleton<Localization>.Instance.GetValueByKey("menu_btn_change_lang");
		}

		private void OnBtnShowHelp()
		{
			PopupTvOSManualView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupTvOSManualView>();
			registeredPopup.ShowHelp();
		}

		private void OnSecretBtn1()
		{
		}

		private void OnSecretBtn2()
		{
		}

		private void OnSecretBtn3()
		{
			TinyTeam.Debuger.Console.isConsoleAllowed = !TinyTeam.Debuger.Console.isConsoleAllowed;
		}

		private object CMDUnlockAllLevels(string[] args)
		{
			for (int i = 0; i < MonoSingleton<AppController>.Instance.Model.Levels.Count; i++)
			{
				LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.Levels[i];
				levelItem.Status = LevelStatus.Completed;
			}
			return "Unlock All Levels Success";
		}

		private object CMDShowMediationTest(string[] args)
		{
			return "Show Mediation Test Success";
		}
	}
}
