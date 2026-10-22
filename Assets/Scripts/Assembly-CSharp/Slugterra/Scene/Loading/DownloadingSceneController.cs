using System;
using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Scene;
using AppsMinistry.Utils;
using AssetBundles;
using UnityEngine;

namespace Slugterra.Scene.Loading
{
	[SceneName("DownloadingScene", "", false, false)]
	public class DownloadingSceneController : SlugterraSceneController
	{
		[SerializeField]
		private List<string> bundlesToLoading = new List<string> { "base" };

		[SerializeField]
		private List<string> allBundles = new List<string> { "base" };

		private List<string> bundlesToLoadingCopy;

		private Type nextScene = typeof(MainSceneController);

		private bool connectionLost;

		private bool isAlertShow;

		private AndroidMessage androidMessage;

		private string currentLoadingBundle = string.Empty;

		protected override void OnLoaded()
		{
			Debug.Log("DownloadingSceneController::OnLoaded");
			base.OnLoaded();
			bundlesToLoading.Clear();
			nextScene = DonDestroyMonoSingleton<DownloadManager>.Instance.nextScene;
			StartCoroutine(StartLoading());
		}

		private void ProceedWork()
		{
			DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance.finishedBundleLoading = (Action<string>)Delegate.Combine(instance.finishedBundleLoading, new Action<string>(BundleDownloaded));
			DownloadManager instance2 = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance2.faliedBundleLoading = (Action<string>)Delegate.Combine(instance2.faliedBundleLoading, new Action<string>(BundleDownloadFailed));
			bundlesToLoading = DonDestroyMonoSingleton<DownloadManager>.Instance.taskForDownloads;
			nextScene = DonDestroyMonoSingleton<DownloadManager>.Instance.nextScene;
			bundlesToLoadingCopy = CloneList(bundlesToLoading);
			List<string> list = CloneListDeleteDownloaded(bundlesToLoading);
			if (list.Count > 0)
			{
				CheckInternetService instance3 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
				instance3.onInternetConnectionAppear = (Action)Delegate.Combine(instance3.onInternetConnectionAppear, new Action(InternetConnectionAppear));
				CheckInternetService instance4 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
				instance4.onInternetConnectionLost = (Action)Delegate.Combine(instance4.onInternetConnectionLost, new Action(InternetConnectionLost));
				DonDestroyMonoSingleton<AssetBundleManager>.Instance.IsNeedToUseInternetFlag = true;
			}
			else
			{
				DonDestroyMonoSingleton<AssetBundleManager>.Instance.IsNeedToUseInternetFlag = false;
			}
			string text = string.Empty;
			for (int i = 0; i < bundlesToLoading.Count; i++)
			{
				text = text + bundlesToLoading[i] + "; ";
			}
			StartCoroutine(StartLoading());
		}

		protected override void OnClose()
		{
			base.OnClose();
			DownloadManager instance = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance.finishedBundleLoading = (Action<string>)Delegate.Remove(instance.finishedBundleLoading, new Action<string>(BundleDownloaded));
			DownloadManager instance2 = DonDestroyMonoSingleton<DownloadManager>.Instance;
			instance2.faliedBundleLoading = (Action<string>)Delegate.Remove(instance2.faliedBundleLoading, new Action<string>(BundleDownloadFailed));
			CheckInternetService instance3 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance3.onInternetConnectionAppear = (Action)Delegate.Remove(instance3.onInternetConnectionAppear, new Action(InternetConnectionAppear));
			CheckInternetService instance4 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance4.onInternetConnectionLost = (Action)Delegate.Remove(instance4.onInternetConnectionLost, new Action(InternetConnectionLost));
		}

		private void ClearCurentBundles()
		{
			for (int i = 0; i < allBundles.Count; i++)
			{
				PlayerPrefs.DeleteKey(allBundles[i]);
			}
			PlayerPrefs.DeleteKey("LoadBundlesEnable");
			Caching.CleanCache();
		}

		private void BundleDownloadFailed(string bundleName)
		{
			if (!bundlesToLoading.Contains(bundleName))
			{
				bundlesToLoading.Add(bundleName);
			}
		}

		private void InternetConnectionAppear()
		{
			if (connectionLost && !isAlertShow)
			{
				connectionLost = false;
			}
		}

		private void InternetConnectionLost()
		{
			connectionLost = true;
			if (!isAlertShow)
			{
				this.androidMessage = AndroidMessage.Create(MonoSingleton<Localization>.Instance["alert_internet_lost_title"], MonoSingleton<Localization>.Instance["alert_internet_error_body"], MonoSingleton<Localization>.Instance["alert_internet_lost_text"]);
				AndroidMessage androidMessage = this.androidMessage;
				androidMessage.OnComplete = (Action)Delegate.Combine(androidMessage.OnComplete, new Action(OnAlertButtonClicked));
			}
			isAlertShow = true;
		}

		private void OnAlertButtonClicked()
		{
			AndroidMessage androidMessage = this.androidMessage;
			androidMessage.OnComplete = (Action)Delegate.Remove(androidMessage.OnComplete, new Action(OnAlertButtonClicked));
			isAlertShow = false;
		}

		private IEnumerator ReLoad()
		{
			bundlesToLoading = CloneListDeleteDownloaded(bundlesToLoadingCopy);
			string whatWeLoad = string.Empty;
			for (int i = 0; i < bundlesToLoading.Count; i++)
			{
				whatWeLoad = whatWeLoad + bundlesToLoading[i] + "; ";
			}
			yield return StartCoroutine(StartLoading());
		}

		private List<string> CloneList(List<string> list)
		{
			List<string> list2 = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(list[i]);
			}
			return list2;
		}

		private List<string> CloneListDeleteDownloaded(List<string> list)
		{
			List<string> list2 = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				if (!DonDestroyMonoSingleton<DownloadManager>.Instance.IsBundleDownloaded(list[i]))
				{
					list2.Add(list[i]);
				}
			}
			return list2;
		}

		private void BundleDownloaded(string bundleName)
		{
			if (currentLoadingBundle != bundleName)
			{
				return;
			}
			string text = ContainsVariantBandle(bundlesToLoading, bundleName);
			if (bundlesToLoading != null && bundlesToLoading.Contains(bundleName))
			{
				bundlesToLoading.Remove(bundleName);
			}
			else if (text != null)
			{
				bundlesToLoading.Remove(text);
			}
			if (bundlesToLoading != null && bundlesToLoading.Count > 0)
			{
				DonDestroyMonoSingleton<DownloadManager>.Instance.numberOfDownloadedBundles++;
				DonDestroyMonoSingleton<DownloadManager>.Instance.LoadBundle(bundlesToLoading[0]);
				currentLoadingBundle = bundlesToLoading[0];
				return;
			}
			List<string> loadedAssetBundlesNames = DonDestroyMonoSingleton<AssetBundleManager>.Instance.GetLoadedAssetBundlesNames();
			string text2 = " AssetBundles count " + loadedAssetBundlesNames.Count + " : ";
			for (int i = 0; i < loadedAssetBundlesNames.Count; i++)
			{
				text2 = text2 + loadedAssetBundlesNames[i] + "; ";
			}
			SceneLoader.LoadScene(nextScene);
		}

		private string ContainsVariantBandle(List<string> bandlesList, string bandleName)
		{
			for (int i = 0; i < bandlesList.Count; i++)
			{
				if (bandlesList[i] + "." + tk2dSystem.CurrentPlatform == bandleName)
				{
					return bandlesList[i];
				}
			}
			return null;
		}

		private IEnumerator StartLoading()
		{
			string whatWeLoad = string.Empty;
			for (int i = 0; i < bundlesToLoading.Count; i++)
			{
				whatWeLoad = whatWeLoad + bundlesToLoading[i] + "; ";
			}
			yield return new WaitForSeconds(0.1f);
			SceneLoader.LoadScene(nextScene);
		}

		private object CMDReLoadBundles(string[] args)
		{
			StartCoroutine(ReLoad());
			return "Reload...";
		}
	}
}
