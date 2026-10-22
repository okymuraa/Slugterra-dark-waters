using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Facebook;
using AppsMinistry.Core.Input;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.InAppPurchases;
using Slugterra.Scene;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Settings
{
	public class SettingsMenuController : UIController<SettingsMenuView>, IObserver
	{
		private GameSetting _model;

		private PopupConfirmView _popupConfirm;

		protected override void OnStart()
		{
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			_model = MonoSingleton<AppController>.Instance.Model.GameSetting;
			_model.AddObserver(this);
			_model.CommitAndNotify();
		}

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBack;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += OnBack;
			View.OnСontactUsEvent += OnСontactUs;
			View.OnCreditsEvent += OnCredist;
			View.OnRestoreEvent += OnRestore;
			View.OnGameServiceEvent += OnGameService;
			View.OnHomePageEvent += OnHomePage;
			View.OnAppsMinistryLogoEvent += OnAppsMinistryLogo;
			View.OnNerdCorpsLogoEvent += OnNerdCorpsLogo;
			View.OnBtnSkipCreditsEvent += OnSkipCredits;
			View.OnBtnRecordEvent += OnRecord;
			View.OnBtnFacebookConnectEvent += OnFacebook;
			bool flag = PlayerPrefs.GetInt("isReplayKitEnabled", 0) != 0;
			View.SwitchRecordButton(flag);
			Debug.Log("SwitchRecordButton, isRecording = " + flag);
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= OnBack;
			View.OnСontactUsEvent -= OnСontactUs;
			View.OnCreditsEvent -= OnCredist;
			View.OnRestoreEvent -= OnRestore;
			View.OnGameServiceEvent -= OnGameService;
			View.OnHomePageEvent -= OnHomePage;
			View.OnAppsMinistryLogoEvent -= OnAppsMinistryLogo;
			View.OnNerdCorpsLogoEvent -= OnNerdCorpsLogo;
			View.OnBtnSkipCreditsEvent -= OnSkipCredits;
			View.OnBtnRecordEvent -= OnRecord;
			View.OnBtnFacebookConnectEvent -= OnFacebook;
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is GameSetting)
			{
				View.ApplyModel(observable as GameSetting);
			}
		}

		private void OnBack()
		{
			SceneLoader.LoadPrevScene();
		}

		private void OnRestore()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.RestorePurchases();
			}, (string errorMsg) =>
			{
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		private void OnHomePage()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				Application.OpenURL("https://www.appsministry.com/app/ios/slugterra/?lang=en");
			}, (string errorMsg) =>
			{
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		private void OnAppsMinistryLogo()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				Application.OpenURL("https://www.appsministry.com/");
			}, (string errorMsg) =>
			{
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		private void OnNerdCorpsLogo()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				Application.OpenURL("http://nerdcorps.com/");
			}, (string errorMsg) =>
			{
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		private void OnCredist()
		{
			View.StartCredits();
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
		}

		private void OnSkipCredits()
		{
			View.StopCredits();
		}

		private void OnSlugControl(SlugControl slugControl)
		{
			_model.SlugControl = slugControl;
		}

		private void OnСontactUs()
		{
			string valueByKey = MonoSingleton<Localization>.Instance.GetValueByKey("email_subject", "email_subject", true);
			string valueByKey2 = MonoSingleton<Localization>.Instance.GetValueByKey("email_template", "email_template", true);
			valueByKey2 = valueByKey2.Replace("\n", "<br/>");
			string text = MonoSingleton<Localization>.Instance.Locale.Substring(0, 2).ToUpper();
			valueByKey = string.Format(valueByKey, AppConfiguration.Name, AppConfiguration.Version, text, 278);
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			TimeSpan utcOffset = currentTimeZone.GetUtcOffset(DateTime.Now);
			string text2 = DateTime.Now.ToString() + " UTC+" + utcOffset.Hours;
			valueByKey2 = string.Format(valueByKey2, SystemInfo.deviceModel, SystemInfo.operatingSystem, Application.systemLanguage, text2);
			PlatformUtil.ETShowMailComposer("support@appsministry.com", valueByKey, valueByKey2);
		}

		private void OnRecord(bool isOn)
		{
			PlayerPrefs.SetInt("isReplayKitEnabled", isOn ? 1 : 0);
		}

		private void OnFacebook(bool isOn)
		{
			if (isOn)
			{
				DonDestroyMonoSingleton<FacebookController>.Instance.Login();
			}
			else
			{
				DonDestroyMonoSingleton<FacebookController>.Instance.Logout();
			}
		}

		protected override void OnReleaseResource()
		{
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
		}
	}
}
