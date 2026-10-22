using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using AssetBundles;
using Slugterra.UI.MapMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slugterra.UI.Popup
{
	public class PopupLoadingNewContentView : PopupView
	{
		public Action stopDownload;

		public tk2dUIProgressBar progressBar;

		public bool internetConnectionLost;

		private LevelItemView[] views;

		public bool mayShowNativeAlert;

		protected override void OnStart()
		{
			progressBar.Value = 0f;
			gameObject.SetActive(false);
			views = UnityEngine.Object.FindObjectsOfType<LevelItemView>();
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Combine(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Remove(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
		}

		public void Update()
		{
			int count = DonDestroyMonoSingleton<AssetBundleManager>.Instance.itemsProgress.Count;
			if (count > 0 && !internetConnectionLost)
			{
				float num = 0f;
				foreach (KeyValuePair<string, float> item in DonDestroyMonoSingleton<AssetBundleManager>.Instance.itemsProgress)
				{
					if (!item.Key.Contains("Android") && !item.Key.Contains("iOS") && !item.Key.Contains("WebGL"))
					{
						num += item.Value / 100f;
					}
				}
				progressBar.Value = num / (float)count;
			}
			else
			{
				Hide();
			}
		}

		public void OnBtnCloseClick()
		{
			Hide();
		}

		public void Hide()
		{
			Screen.sleepTimeout = -2;
			progressBar.Value = 0f;
			if ((SceneManager.GetActiveScene().name == "MapScene" || SceneManager.GetActiveScene().name == "MapScene_HD") && views != null && views.Length > 0)
			{
				for (int i = 0; i < views.Length; i++)
				{
					views[i].enabled = true;
					BoxCollider component = views[i].LevelButton.GetComponent<BoxCollider>();
					if (component != null)
					{
						component.enabled = true;
					}
				}
			}
			gameObject.SetActive(false);
		}

		public void Show()
		{
			Screen.sleepTimeout = -1;
			if ((SceneManager.GetActiveScene().name == "MapScene" || SceneManager.GetActiveScene().name == "MapScene_HD") && views != null && views.Length > 0)
			{
				for (int i = 0; i < views.Length; i++)
				{
					views[i].enabled = false;
					BoxCollider component = views[i].LevelButton.GetComponent<BoxCollider>();
					if (component != null)
					{
						component.enabled = false;
					}
				}
			}
			mayShowNativeAlert = true;
			gameObject.SetActive(true);
		}

		private static void InternetConnectionLost()
		{
			Debug.Log("PopupLoadingNewContentView::InternetConnectionLost");
			PopupLoadingNewContentView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupLoadingNewContentView>();
			if (registeredPopup != null && registeredPopup.mayShowNativeAlert)
			{
				registeredPopup.internetConnectionLost = false;
				registeredPopup.Hide();
				registeredPopup.mayShowNativeAlert = false;
				AndroidMessage androidMessage = AndroidMessage.Create(MonoSingleton<Localization>.Instance["alert_internet_lost_title"], MonoSingleton<Localization>.Instance["alert_internet_error_body"], MonoSingleton<Localization>.Instance["ok_title"]);
			}
		}

		public override void Close()
		{
		}
	}
}
