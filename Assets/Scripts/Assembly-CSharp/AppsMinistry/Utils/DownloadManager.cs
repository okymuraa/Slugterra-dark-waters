using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using AssetBundles;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene;
using Slugterra.Scene.Loading;
using Slugterra.UI.Downloading;
using Slugterra.UI.Popup;
using TinyTeam.Debuger;
using UnityEngine;

namespace AppsMinistry.Utils
{
	public class DownloadManager : DonDestroyMonoSingleton<DownloadManager>
	{
		public Action<string> finishedBundleLoading;

		public Action<string> faliedBundleLoading;

		public Action removeError;

		[SerializeField]
		private LoadAssets _loadAssets;

		[SerializeField]
		private DownloadingController _downloadingContoller;

		public int bundlesToDownload;

		public int numberOfDownloadedBundles;

		public List<string> taskForDownloads;

		public Type nextScene = typeof(MainSceneController);

		public DownloadingController DownloadingController
		{
			set
			{
				_downloadingContoller = value;
			}
		}

		private void OnEnable()
		{
			GameObject gameObject = new GameObject("LoadAssets", typeof(LoadAssets));
			gameObject.transform.parent = transform;
			gameObject.transform.position = Vector3.zero;
			_loadAssets = gameObject.GetComponent<LoadAssets>();
			AssetBundleManager instance = DonDestroyMonoSingleton<AssetBundleManager>.Instance;
			instance.downloadFinished = (Action<string>)Delegate.Combine(instance.downloadFinished, new Action<string>(BundleDownloaded));
			TinyTeam.Debuger.Console.RegisterCommand("loadedAssetBundles", CMDGetDownloadedBundles, "Return Asset Bundles Names");
			TinyTeam.Debuger.Console.RegisterCommand("loadScene", CMDLoadScene, "Load Scene by Name");
			TinyTeam.Debuger.Console.RegisterCommand("loadLevel", CMDLoadLevel, "Load Level by Name");
			TinyTeam.Debuger.Console.RegisterCommand("destroyObject", CMDDestroyObject, "Destroy Object By Name");
			TinyTeam.Debuger.Console.RegisterCommand("isObjectInScene", CMDObjectInScene, "Find Any Object in scene");
			TinyTeam.Debuger.Console.RegisterCommand("getAllActiveVariants", CMDGetAllActiveVariants, "Return all active variants");
			TinyTeam.Debuger.Console.RegisterCommand("getAllVariantsFromAssetBundle", CMDGetAllVariantsFromAssetBundle, "Return all variants from asset bundle");
			TinyTeam.Debuger.Console.RegisterCommand("checkInternet", CMDCheckInternet, "Check internet");
			TinyTeam.Debuger.Console.RegisterCommand("abShowErrors", CMDShowErrors, "Show Download Errors");
			TinyTeam.Debuger.Console.RegisterCommand("abShowOp", CMDShowProgressOperations, "Show All Progress Operations");
			TinyTeam.Debuger.Console.RegisterCommand("abShowLoaded", CMDShowLoadedAssetBundles, "Show Download Errors");
			TinyTeam.Debuger.Console.RegisterCommand("loadFile", CMDLoadFile, "Command for Load File");
		}

		public void LoadBundlesWithDownloadScene(string[] _bundlesToDownload, Type _nextScene)
		{
			taskForDownloads.Clear();
			taskForDownloads.AddRange(_bundlesToDownload);
			nextScene = _nextScene;
			SceneLoader.LoadScene(typeof(DownloadingSceneController));
		}

		public void LoadBundle(string bundleName, bool loadingNewContent = false)
		{
			DonDestroyMonoSingleton<AssetBundleManager>.Instance.IsNeedToUseInternetFlag = !IsBundleDownloaded(bundleName);
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (_loadAssets != null)
				{
					_loadAssets.DownloadAssetBundle(bundleName);
				}
				if ((bool)_downloadingContoller)
				{
					_downloadingContoller.gameObject.SetActive(true);
				}
				if (loadingNewContent && !IsBundleDownloaded(bundleName))
				{
					PopupLoadingNewContentView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupLoadingNewContentView>();
					if ((bool)registeredPopup && !registeredPopup.gameObject.activeSelf)
					{
						registeredPopup.Show();
					}
				}
				if (removeError != null)
				{
					removeError();
				}
			}, (string errorMsg) =>
			{
				if (IsBundleDownloaded(bundleName))
				{
					if (_loadAssets != null)
					{
						_loadAssets.DownloadAssetBundle(bundleName);
					}
				}
				else
				{
					if (faliedBundleLoading != null)
					{
						faliedBundleLoading(bundleName);
					}
					PopupConfirmView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
					if (registeredPopup != null)
					{
						registeredPopup.Show("alert_internet_error", null, null, false);
					}
				}
			});
		}

		public void LoadLevel(string sceneName, string assetBundleName, bool isAsync, bool isAdditive)
		{
			if (_loadAssets != null)
			{
				_loadAssets.LoadLevelFromAssetBundle(sceneName, assetBundleName, isAsync, isAdditive);
			}
		}

		public void UnloadAsset(string bundleName)
		{
			DonDestroyMonoSingleton<AssetBundleManager>.Instance.UnloadAssetBundle(bundleName);
		}

		private void BundleDownloaded(string bundleName)
		{
			PlayerPrefs.SetInt(bundleName, 1);
			PlayerPrefs.Save();
			if (finishedBundleLoading != null)
			{
				finishedBundleLoading(bundleName);
			}
		}

		public bool IsBundleDownloaded(string bundleName)
		{
			int num = PlayerPrefs.GetInt(bundleName, 0);
			return num == 1;
		}

		public bool RemoveDownloadedBundle(string bundleName)
		{
			Debug.LogWarningFormat("<b><color=red>DownloadManager::RemoveDownloadedBundle {0}</color></b>", bundleName);
			if (PlayerPrefsSystem.HasKey(bundleName))
			{
				return true;
			}
			return false;
		}

		private object CMDGetDownloadedBundles(string[] args)
		{
			List<string> loadedAssetBundlesNames = DonDestroyMonoSingleton<AssetBundleManager>.Instance.GetLoadedAssetBundlesNames();
			string text = " AssetBundles count " + loadedAssetBundlesNames.Count + " : ";
			for (int i = 0; i < loadedAssetBundlesNames.Count; i++)
			{
				text = text + loadedAssetBundlesNames[i] + "; ";
			}
			return text;
		}

		private object CMDLoadScene(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "Need Level Name";
			}
			string text = args[1];
			Debug.Log("Try to Load scene " + text);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(text);
			if (levelItemByLevelName != null)
			{
				SceneLoader.LoadScene(levelItemByLevelName.Scene);
				return "Load Scene";
			}
			return "Error to Loading scene " + text;
		}

		private object CMDLoadLevel(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "Need Level Name";
			}
			string text = args[1];
			Debug.Log("Try to Load scene " + text);
			Application.LoadLevel(text);
			return "Load Scene";
		}

		private object CMDDestroyObject(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "What do you want to Destroy? Enter name!";
			}
			string text = string.Empty;
			for (int i = 1; i < args.Length; i++)
			{
				text = text + args[i] + " ";
			}
			text = text.Remove(text.Length - 1, 1);
			string result = " Destroy Object " + text;
			GameObject gameObject = GameObject.Find(text);
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
				return result;
			}
			return "Error to Destroy object " + text;
		}

		private object CMDObjectInScene(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "What do you want to search in Scene? Enter name!";
			}
			string text = args[1];
			GameObject gameObject = GameObject.Find(text);
			if (gameObject != null)
			{
				return "Object " + text + " found";
			}
			return "Object " + text + " not found ";
		}

		private object CMDGetAllActiveVariants(string[] args)
		{
			string[] activeVariants = DonDestroyMonoSingleton<AssetBundleManager>.Instance.ActiveVariants;
			string text = "Active Variants: \n";
			if (activeVariants != null && activeVariants.Length > 0)
			{
				for (int i = 0; i < activeVariants.Length; i++)
				{
					text += string.Format(" variants[{0}] : {1};", i, activeVariants[i]);
				}
			}
			else
			{
				text += "***Empty***";
			}
			return text;
		}

		private object CMDGetAllVariantsFromAssetBundle(string[] args)
		{
			string[] allVariantsFromAssetBundle = DonDestroyMonoSingleton<AssetBundleManager>.Instance.GetAllVariantsFromAssetBundle();
			string text = "Variants In Asset Bundle: \n";
			if (allVariantsFromAssetBundle != null && allVariantsFromAssetBundle.Length > 0)
			{
				for (int i = 0; i < allVariantsFromAssetBundle.Length; i++)
				{
					text += string.Format(" variants[{0}] : {1};", i, allVariantsFromAssetBundle[i]);
				}
			}
			else
			{
				text += "***Empty***";
			}
			return text;
		}

		private object CMDShowErrors(string[] args)
		{
			Dictionary<string, string> downloadingErrors = DonDestroyMonoSingleton<AssetBundleManager>.Instance.downloadingErrors;
			if (downloadingErrors != null)
			{
				foreach (KeyValuePair<string, string> item in downloadingErrors)
				{
					Debug.Log("AssetBundle Name: " + item.Key + "Error: " + item.Value);
				}
				return "ShowErrors";
			}
			return "ShowErrors = null";
		}

		private object CMDShowLoadedAssetBundles(string[] args)
		{
			Dictionary<string, LoadedAssetBundle> loadedAssetBundles = DonDestroyMonoSingleton<AssetBundleManager>.Instance.loadedAssetBundles;
			if (loadedAssetBundles != null)
			{
				foreach (KeyValuePair<string, LoadedAssetBundle> item in loadedAssetBundles)
				{
					Debug.Log("LoadedAssetBundles: " + item.Key);
				}
				return "ShowLoadedAssetBundles";
			}
			return "ShowLoadedAssetBundles = null";
		}

		private object CMDShowProgressOperations(string[] args)
		{
			List<AssetBundleLoadOperation> inProgressOperations = DonDestroyMonoSingleton<AssetBundleManager>.Instance.inProgressOperations;
			if (inProgressOperations != null)
			{
				foreach (AssetBundleLoadOperation item in inProgressOperations)
				{
					string bundleName = item.GetBundleName();
					Debug.Log("ProgressOperation | Type: " + item.GetType().ToString() + "| BundleName: " + bundleName + "| IsDone: " + item.IsDone() + "| HaveError: " + item.HaveError());
					AssetBundleDownloadOperation assetBundleDownloadOperation = item as AssetBundleDownloadOperation;
					if (assetBundleDownloadOperation != null)
					{
						Debug.Log("ProgressOperation is download | downloadProgress: " + assetBundleDownloadOperation.downloadProgress + "| Error: " + ((assetBundleDownloadOperation.error != null) ? assetBundleDownloadOperation.error : "none").ToString() + "| assetBundleName: " + assetBundleDownloadOperation.assetBundleName);
					}
				}
				return "ShowProgressOperations";
			}
			return "ShowProgressOperations = null";
		}

		private object CMDCheckInternet(string[] args)
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				Debug.Log("Connected to Internet: TRUE");
			}, (string errorMsg) =>
			{
				Debug.Log("Connected to Internet: FALSE. Error Message " + errorMsg);
			});
			return "CheckInternet";
		}

		private object CMDLoadFile(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "Enter path to the file!";
			}
			string text = args[1];
			AssetBundle assetBundle = AssetBundle.LoadFromFile(text);
			if (assetBundle == null)
			{
				return "File " + text + " was loaded";
			}
			return "File was not loaded";
		}
	}
}
