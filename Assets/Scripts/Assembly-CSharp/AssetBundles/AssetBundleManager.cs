using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.UI.Popup;
using UnityEngine;

namespace AssetBundles
{
	public class AssetBundleManager : DonDestroyMonoSingleton<AssetBundleManager>
	{
		public enum LogMode
		{
			All = 0,
			JustErrors = 1
		}

		public enum LogType
		{
			Info = 0,
			Warning = 1,
			Error = 2
		}

		public delegate string OverrideBaseDownloadingURLDelegate(string bundleName);

		public Action<string> downloadFinished;

		public Action<string> downloadFailed;

		public Action itemsInProgressUpdated;

		public string BUNDLES_URL = string.Empty;

		private LogMode m_LogMode;

		private string m_BaseDownloadingURL = string.Empty;

		private string[] m_ActiveVariants = new string[0];

		private AssetBundleManifest m_AssetBundleManifest;

		private Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();

		private Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();

		private List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();

		private Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

		private List<string> currentDownloading = new List<string>();

		public Dictionary<string, float> itemsProgress = new Dictionary<string, float>();

		private PopupLoadingNewContentView popupNewContent;

		private bool popupNewContentEnable;

		private bool connectionLost;

		private bool isNeedToUseInternetFlag = true;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURL__BackingField;

		private bool oneTimeFlag = true;

		private List<string> keyList;

		public bool IsNeedToUseInternetFlag
		{
			get
			{
				return isNeedToUseInternetFlag;
			}
			set
			{
				isNeedToUseInternetFlag = value;
			}
		}

		public Dictionary<string, string> downloadingErrors
		{
			get
			{
				return m_DownloadingErrors;
			}
		}

		public Dictionary<string, LoadedAssetBundle> loadedAssetBundles
		{
			get
			{
				return m_LoadedAssetBundles;
			}
		}

		public List<AssetBundleLoadOperation> inProgressOperations
		{
			get
			{
				return m_InProgressOperations;
			}
		}

		public LogMode logMode
		{
			get
			{
				return m_LogMode;
			}
			set
			{
				m_LogMode = value;
			}
		}

		public string BaseDownloadingURL
		{
			get
			{
				return m_BaseDownloadingURL;
			}
			set
			{
				m_BaseDownloadingURL = value;
			}
		}

		public string[] ActiveVariants
		{
			get
			{
				return m_ActiveVariants;
			}
			set
			{
				m_ActiveVariants = value;
			}
		}

		public AssetBundleManifest AssetBundleManifestObject
		{
			set
			{
				m_AssetBundleManifest = value;
			}
		}

		public bool isManifestLoaded
		{
			get
			{
				return m_AssetBundleManifest != null;
			}
		}

		public event OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURL
		{
			add
			{
				OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURLDelegate = overrideBaseDownloadingURL__BackingField;
				OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURLDelegate2;
				do
				{
					overrideBaseDownloadingURLDelegate2 = overrideBaseDownloadingURLDelegate;
					overrideBaseDownloadingURLDelegate = Interlocked.CompareExchange(ref overrideBaseDownloadingURL__BackingField, (OverrideBaseDownloadingURLDelegate)Delegate.Combine(overrideBaseDownloadingURLDelegate2, value), overrideBaseDownloadingURLDelegate);
				}
				while ((object)overrideBaseDownloadingURLDelegate != overrideBaseDownloadingURLDelegate2);
			}
			remove
			{
				OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURLDelegate = overrideBaseDownloadingURL__BackingField;
				OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURLDelegate2;
				do
				{
					overrideBaseDownloadingURLDelegate2 = overrideBaseDownloadingURLDelegate;
					overrideBaseDownloadingURLDelegate = Interlocked.CompareExchange(ref overrideBaseDownloadingURL__BackingField, (OverrideBaseDownloadingURLDelegate)Delegate.Remove(overrideBaseDownloadingURLDelegate2, value), overrideBaseDownloadingURLDelegate);
				}
				while ((object)overrideBaseDownloadingURLDelegate != overrideBaseDownloadingURLDelegate2);
			}
		}

		private void OnEnable()
		{
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Combine(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
			CheckInternetService instance2 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance2.onInternetConnectionAppear = (Action)Delegate.Combine(instance2.onInternetConnectionAppear, new Action(InternetConnectionAppear));
		}

		private void OnDisable()
		{
			CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance.onInternetConnectionLost = (Action)Delegate.Remove(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
			CheckInternetService instance2 = DonDestroyMonoSingleton<CheckInternetService>.Instance;
			instance2.onInternetConnectionAppear = (Action)Delegate.Remove(instance2.onInternetConnectionAppear, new Action(InternetConnectionAppear));
		}

		private void InternetConnectionLost()
		{
			if (isNeedToUseInternetFlag)
			{
				connectionLost = true;
			}
		}

		private void InternetConnectionAppear()
		{
			connectionLost = false;
			m_DownloadingErrors.Clear();
		}

		private void StopDownload()
		{
			if (m_InProgressOperations.Count <= 0)
			{
				return;
			}
			int num = 0;
			while (num < m_InProgressOperations.Count)
			{
				AssetBundleLoadOperation assetBundleLoadOperation = m_InProgressOperations[num];
				m_InProgressOperations.RemoveAt(num);
				AssetBundleDownloadOperation assetBundleDownloadOperation = assetBundleLoadOperation as AssetBundleDownloadOperation;
				if (assetBundleDownloadOperation != null)
				{
					currentDownloading.Remove(assetBundleDownloadOperation.assetBundleName);
					assetBundleDownloadOperation.StopDownload();
				}
			}
			m_DownloadingErrors.Clear();
			itemsProgress.Clear();
		}

		private static string GetStreamingAssetsPath()
		{
			if (Application.isEditor)
			{
				return "file://" + Environment.CurrentDirectory.Replace("\\", "/");
			}
#if !UNITY_2018_1_OR_NEWER
			if (Application.isWebPlayer)
			{
				return Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/StreamingAssets";
			}
#endif
			if (Application.isMobilePlatform || Application.isConsolePlatform)
			{
				return Application.streamingAssetsPath;
			}
			return "file://" + Application.streamingAssetsPath;
		}

		public void SetSourceAssetBundleDirectory(string relativePath)
		{
			BaseDownloadingURL = GetStreamingAssetsPath() + relativePath;
		}

		public void SetSourceAssetBundleURL(string absolutePath)
		{
			BaseDownloadingURL = absolutePath + Utility.GetPlatformName() + "/";
		}

		public void SetDevelopmentAssetBundleServer()
		{
			TextAsset textAsset = Resources.Load("AssetBundleServerURL") as TextAsset;
			string text = ((!(textAsset != null)) ? null : textAsset.text.Trim());
			if (text != null && text.Length != 0)
			{
				SetSourceAssetBundleURL(text);
			}
		}

		public List<string> GetLoadedAssetBundlesNames()
		{
			return new List<string>(m_LoadedAssetBundles.Keys);
		}

		public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
		{
			if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
			{
				return null;
			}
			LoadedAssetBundle value = null;
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out value);
			if (value == null)
			{
				return null;
			}
			string[] value2 = null;
			if (!m_Dependencies.TryGetValue(assetBundleName, out value2))
			{
				return value;
			}
			string[] array = value2;
			foreach (string key in array)
			{
				if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
				{
					return value;
				}
				LoadedAssetBundle value3;
				m_LoadedAssetBundles.TryGetValue(key, out value3);
				if (value3 == null)
				{
					return null;
				}
			}
			return value;
		}

		public bool IsAssetBundleDownloaded(string assetBundleName)
		{
			return m_LoadedAssetBundles.ContainsKey(assetBundleName);
		}

		public AssetBundleLoadManifestOperation Initialize()
		{
			return Initialize(Utility.GetPlatformName());
		}

		public AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
		{
			LoadAssetBundle(manifestAssetBundleName, true);
			for (int i = 0; i < m_InProgressOperations.Count; i++)
			{
				if (m_InProgressOperations[i] is AssetBundleLoadManifestOperation)
				{
					return m_InProgressOperations[i] as AssetBundleLoadManifestOperation;
				}
			}
			AssetBundleLoadManifestOperation assetBundleLoadManifestOperation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
			m_InProgressOperations.Add(assetBundleLoadManifestOperation);
			return assetBundleLoadManifestOperation;
		}

		protected void LoadAssetBundle(string assetBundleName)
		{
			LoadAssetBundle(assetBundleName, false);
		}

		protected void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			if ((isLoadingAssetBundleManifest || !(m_AssetBundleManifest == null)) && !LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest) && !isLoadingAssetBundleManifest)
			{
				LoadDependencies(assetBundleName);
			}
		}

		protected string GetAssetBundleBaseDownloadingURL(string bundleName)
		{
			if (overrideBaseDownloadingURL__BackingField != null)
			{
				Delegate[] invocationList = overrideBaseDownloadingURL__BackingField.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURLDelegate = (OverrideBaseDownloadingURLDelegate)invocationList[i];
					string text = overrideBaseDownloadingURLDelegate(bundleName);
					if (text != null)
					{
						return text;
					}
				}
			}
			return m_BaseDownloadingURL;
		}

		protected bool UsesExternalBundleVariantResolutionMechanism(string baseAssetBundleName)
		{
			return false;
		}

		protected string RemapVariantName(string assetBundleName)
		{
			if (assetBundleName == null || m_AssetBundleManifest == null)
			{
				return string.Empty;
			}
			string[] allAssetBundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();
			string text = "Variants In Manifest: ";
			if (allAssetBundlesWithVariant != null && allAssetBundlesWithVariant.Length > 0)
			{
				for (int i = 0; i < allAssetBundlesWithVariant.Length; i++)
				{
					text += string.Format(" variants[{0}] : {1};", i, allAssetBundlesWithVariant[i]);
				}
			}
			string text2 = assetBundleName.Split('.')[0];
			if (UsesExternalBundleVariantResolutionMechanism(text2))
			{
				return text2;
			}
			int num = int.MaxValue;
			int num2 = -1;
			for (int j = 0; j < allAssetBundlesWithVariant.Length; j++)
			{
				string[] array = allAssetBundlesWithVariant[j].Split('.');
				string text3 = array[0];
				string value = array[1];
				if (!(text3 != text2))
				{
					int num3 = Array.IndexOf(m_ActiveVariants, value);
					if (num3 == -1)
					{
						num3 = 2147483646;
					}
					if (num3 < num)
					{
						num = num3;
						num2 = j;
					}
				}
			}
			if (num == 2147483646)
			{
				UnityEngine.Debug.LogWarning("Ambigious asset bundle variant chosen because there was no matching active variant: " + allAssetBundlesWithVariant[num2]);
			}
			if (num2 != -1)
			{
				return allAssetBundlesWithVariant[num2];
			}
			return assetBundleName;
		}

		protected bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			LoadedAssetBundle value = null;
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out value);
			if (value != null)
			{
				value.m_ReferencedCount++;
				return true;
			}
			if (currentDownloading.Contains(assetBundleName))
			{
				return true;
			}
			string assetBundleBaseDownloadingURL = GetAssetBundleBaseDownloadingURL(assetBundleName);
			DownloadedBandleDesc downloadedBandleDesc = new DownloadedBandleDesc();
			downloadedBandleDesc.m_IsManifest = isLoadingAssetBundleManifest;
			if (assetBundleBaseDownloadingURL.ToLower().StartsWith("odr://"))
			{
				new ApplicationException("Can't load bundle " + assetBundleName + " through ODR: this Unity version or build target doesn't support it.");
			}
			else if (assetBundleBaseDownloadingURL.ToLower().StartsWith("res://"))
			{
				new ApplicationException("Can't load bundle " + assetBundleName + " through asset catalog: this Unity version or build target doesn't support it.");
			}
			else
			{
				WWW download = null;
				string url = assetBundleBaseDownloadingURL + assetBundleName;
				if (isLoadingAssetBundleManifest)
				{
					bool IsFileExist = IOSystem.IsFileExist(Application.persistentDataPath, "lastManifest" + assetBundleName);
					Uri uri = new Uri("file://" + Application.persistentDataPath + "/lastManifest" + assetBundleName);
					string filePath = uri.AbsoluteUri;
					DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
					{
						download = new WWW(url);
					}, (string error) =>
					{
						if (IsFileExist)
						{
							download = new WWW(filePath);
						}
						else
						{
							if (downloadFailed != null)
							{
								downloadFailed(MonoSingleton<Localization>.Instance.GetValueByKey("alert_internet_error_short"));
							}
							if (popupNewContent != null)
							{
								PopupLoadingNewContentView popupLoadingNewContentView2 = popupNewContent;
								popupLoadingNewContentView2.stopDownload = (Action)Delegate.Remove(popupLoadingNewContentView2.stopDownload, new Action(StopDownload));
								popupNewContentEnable = false;
							}
						}
					});
				}
				else
				{
					Hash128 assetBundleHash = m_AssetBundleManifest.GetAssetBundleHash(assetBundleName);
					download = WWW.LoadFromCacheOrDownload(url, assetBundleHash, 0u);
				}
				if (download == null)
				{
					if (!m_DownloadingErrors.ContainsKey(assetBundleName))
					{
						m_DownloadingErrors.Add(assetBundleName, string.Format("{0} can't download by WWW", assetBundleName));
						if (downloadFailed != null)
						{
							downloadFailed(MonoSingleton<Localization>.Instance.GetValueByKey("alert_internet_error_short"));
						}
						if (popupNewContent != null)
						{
							PopupLoadingNewContentView popupLoadingNewContentView = popupNewContent;
							popupLoadingNewContentView.stopDownload = (Action)Delegate.Remove(popupLoadingNewContentView.stopDownload, new Action(StopDownload));
							popupNewContentEnable = false;
						}
					}
					return false;
				}
				m_InProgressOperations.Add(new AssetBundleDownloadFromWebOperation(assetBundleName, download, downloadedBandleDesc.m_IsManifest));
			}
			currentDownloading.Add(assetBundleName);
			return false;
		}

		protected void LoadDependencies(string assetBundleName)
		{
			if (m_AssetBundleManifest == null)
			{
				return;
			}
			string[] allDependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
			if (allDependencies.Length != 0)
			{
				for (int i = 0; i < allDependencies.Length; i++)
				{
					allDependencies[i] = RemapVariantName(allDependencies[i]);
				}
				m_Dependencies.Add(assetBundleName, allDependencies);
				for (int j = 0; j < allDependencies.Length; j++)
				{
					LoadAssetBundleInternal(allDependencies[j], false);
				}
			}
		}

		public void UnloadAssetBundle(string assetBundleName)
		{
			UnloadAssetBundleInternal(assetBundleName);
			UnloadDependencies(assetBundleName);
		}

		protected void UnloadDependencies(string assetBundleName)
		{
			string[] value = null;
			if (m_Dependencies.TryGetValue(assetBundleName, out value))
			{
				string[] array = value;
				foreach (string assetBundleName2 in array)
				{
					UnloadAssetBundleInternal(assetBundleName2);
				}
				m_Dependencies.Remove(assetBundleName);
			}
		}

		protected void UnloadAssetBundleInternal(string assetBundleName)
		{
			string error;
			LoadedAssetBundle loadedAssetBundle = GetLoadedAssetBundle(assetBundleName, out error);
			if (loadedAssetBundle != null && --loadedAssetBundle.m_ReferencedCount == 0)
			{
				loadedAssetBundle.m_AssetBundle.Unload(false);
				m_LoadedAssetBundles.Remove(assetBundleName);
			}
		}

		public void ResetOneTimeFlag()
		{
			oneTimeFlag = true;
		}

		private new void Update()
		{
			int num = 0;
			while (num < m_InProgressOperations.Count)
			{
				AssetBundleLoadOperation assetBundleLoadOperation = m_InProgressOperations[num];
				AssetBundleDownloadOperation assetBundleDownloadOperation = assetBundleLoadOperation as AssetBundleDownloadOperation;
				AssetBundleDownloadingOperation assetBundleDownloadingOperation = assetBundleLoadOperation as AssetBundleDownloadingOperation;
				bool flag = false;
				if (assetBundleDownloadOperation == null && assetBundleDownloadingOperation != null)
				{
					flag = IsAssetBundleDownloaded(assetBundleDownloadingOperation.GetBundleName());
				}
				if (assetBundleLoadOperation.Update() && !assetBundleLoadOperation.HaveError() && !flag)
				{
					num++;
					if (assetBundleDownloadOperation == null)
					{
						continue;
					}
					if (!itemsProgress.ContainsKey(assetBundleDownloadOperation.assetBundleName))
					{
						itemsProgress.Add(assetBundleDownloadOperation.assetBundleName, assetBundleDownloadOperation.downloadProgress * 100f);
						if (itemsInProgressUpdated != null)
						{
							itemsInProgressUpdated();
						}
					}
					else
					{
						itemsProgress[assetBundleDownloadOperation.assetBundleName] = assetBundleDownloadOperation.downloadProgress * 100f;
					}
				}
				else
				{
					m_InProgressOperations.RemoveAt(num);
					ProcessFinishedOperation(assetBundleLoadOperation);
				}
			}
			if (m_InProgressOperations.Count == 0 && m_DownloadingErrors.Count > 0 && oneTimeFlag)
			{
				oneTimeFlag = false;
				foreach (KeyValuePair<string, string> downloadingError in m_DownloadingErrors)
				{
					UnityEngine.Debug.LogWarningFormat("AssetBundleManager::Update bundle {0} with Download Error: {1}", downloadingError.Key, downloadingError.Value);
					UnloadAssetBundle(downloadingError.Key);
				}
				m_DownloadingErrors.Clear();
				if (downloadFailed != null)
				{
					downloadFailed(MonoSingleton<Localization>.Instance.GetValueByKey("alert_internet_error_short"));
				}
				if (popupNewContent != null)
				{
					PopupLoadingNewContentView popupLoadingNewContentView = popupNewContent;
					popupLoadingNewContentView.stopDownload = (Action)Delegate.Remove(popupLoadingNewContentView.stopDownload, new Action(StopDownload));
					popupNewContentEnable = false;
				}
			}
			if (m_InProgressOperations.Count > 0 && connectionLost && isNeedToUseInternetFlag)
			{
				int num2 = 0;
				while (num2 < m_InProgressOperations.Count)
				{
					AssetBundleLoadOperation operation = m_InProgressOperations[num2];
					m_InProgressOperations.RemoveAt(num2);
					ProcessFinishedOperation(operation);
				}
				m_DownloadingErrors.Clear();
			}
		}

		private void ProcessFinishedOperation(AssetBundleLoadOperation operation)
		{
			AssetBundleDownloadOperation assetBundleDownloadOperation = operation as AssetBundleDownloadOperation;
			if (assetBundleDownloadOperation == null)
			{
				AssetBundleDownloadingOperation assetBundleDownloadingOperation = operation as AssetBundleDownloadingOperation;
				if (assetBundleDownloadingOperation != null && IsAssetBundleDownloaded(assetBundleDownloadingOperation.GetBundleName()))
				{
					if (popupNewContent != null)
					{
						PopupLoadingNewContentView popupLoadingNewContentView = popupNewContent;
						popupLoadingNewContentView.stopDownload = (Action)Delegate.Remove(popupLoadingNewContentView.stopDownload, new Action(StopDownload));
						popupNewContentEnable = false;
					}
					if (downloadFinished != null)
					{
						downloadFinished(assetBundleDownloadingOperation.GetBundleName());
					}
				}
				return;
			}
			if (itemsProgress.ContainsKey(assetBundleDownloadOperation.assetBundleName))
			{
				itemsProgress.Remove(assetBundleDownloadOperation.assetBundleName);
			}
			if (assetBundleDownloadOperation.error == null)
			{
				if (m_LoadedAssetBundles.ContainsKey(assetBundleDownloadOperation.assetBundleName))
				{
					m_LoadedAssetBundles[assetBundleDownloadOperation.assetBundleName] = assetBundleDownloadOperation.assetBundle;
				}
				else
				{
					m_LoadedAssetBundles.Add(assetBundleDownloadOperation.assetBundleName, assetBundleDownloadOperation.assetBundle);
				}
				if (assetBundleDownloadOperation.assetBundle == null)
				{
					if (m_DownloadingErrors.ContainsKey(assetBundleDownloadOperation.assetBundleName))
					{
						m_DownloadingErrors[assetBundleDownloadOperation.assetBundleName] = string.Format("{0} is not a valid asset bundle.", assetBundleDownloadOperation.assetBundleName);
						m_LoadedAssetBundles.Remove(assetBundleDownloadOperation.assetBundleName);
					}
					else
					{
						m_DownloadingErrors.Add(assetBundleDownloadOperation.assetBundleName, string.Format("{0} is not a valid asset bundle.", assetBundleDownloadOperation.assetBundleName));
						m_LoadedAssetBundles.Remove(assetBundleDownloadOperation.assetBundleName);
					}
					DonDestroyMonoSingleton<DownloadManager>.Instance.RemoveDownloadedBundle(assetBundleDownloadOperation.assetBundleName);
					assetBundleDownloadOperation.StopDownload();
					if (downloadFailed != null)
					{
						downloadFailed(MonoSingleton<Localization>.Instance.GetValueByKey("alert_internet_error_short"));
					}
					if (popupNewContent != null)
					{
						PopupLoadingNewContentView popupLoadingNewContentView2 = popupNewContent;
						popupLoadingNewContentView2.stopDownload = (Action)Delegate.Remove(popupLoadingNewContentView2.stopDownload, new Action(StopDownload));
						popupNewContentEnable = false;
					}
				}
				else
				{
					if (assetBundleDownloadOperation.IsDone())
					{
						if (popupNewContent != null)
						{
							PopupLoadingNewContentView popupLoadingNewContentView3 = popupNewContent;
							popupLoadingNewContentView3.stopDownload = (Action)Delegate.Remove(popupLoadingNewContentView3.stopDownload, new Action(StopDownload));
							popupNewContentEnable = false;
						}
						if (downloadFinished != null)
						{
							downloadFinished(assetBundleDownloadOperation.assetBundleName);
						}
					}
					else
					{
						assetBundleDownloadOperation.StopDownload();
					}
					if (m_AssetBundleManifest == null)
					{
						ResetOneTimeFlag();
					}
				}
			}
			else
			{
				string value = string.Format("Failed downloading bundle {0} from {1}: {2}", assetBundleDownloadOperation.assetBundleName, assetBundleDownloadOperation.GetSourceURL(), assetBundleDownloadOperation.error);
				if (m_DownloadingErrors.ContainsKey(assetBundleDownloadOperation.assetBundleName))
				{
					m_DownloadingErrors[assetBundleDownloadOperation.assetBundleName] = value;
				}
				else
				{
					m_DownloadingErrors.Add(assetBundleDownloadOperation.assetBundleName, value);
				}
				if (downloadFailed != null)
				{
					downloadFailed(MonoSingleton<Localization>.Instance.GetValueByKey("alert_internet_error_short"));
				}
				assetBundleDownloadOperation.StopDownload();
				if (popupNewContent != null)
				{
					PopupLoadingNewContentView popupLoadingNewContentView4 = popupNewContent;
					popupLoadingNewContentView4.stopDownload = (Action)Delegate.Remove(popupLoadingNewContentView4.stopDownload, new Action(StopDownload));
					popupNewContentEnable = false;
				}
			}
			currentDownloading.Remove(assetBundleDownloadOperation.assetBundleName);
		}

		public AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName, Type type)
		{
			AssetBundleLoadAssetOperation assetBundleLoadAssetOperation = null;
			assetBundleName = RemapVariantName(assetBundleName);
			LoadAssetBundle(assetBundleName);
			assetBundleLoadAssetOperation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);
			m_InProgressOperations.Add(assetBundleLoadAssetOperation);
			return assetBundleLoadAssetOperation;
		}

		public AssetBundleLoadOperation LoadLevel(string assetBundleName, string levelName, bool isAdditive, bool isAsync)
		{
			AssetBundleLoadOperation assetBundleLoadOperation = null;
			assetBundleName = RemapVariantName(assetBundleName);
			LoadAssetBundle(assetBundleName);
			assetBundleLoadOperation = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive, isAsync);
			m_InProgressOperations.Add(assetBundleLoadOperation);
			return assetBundleLoadOperation;
		}

		public AssetBundleLoadOperation DownloadAssetBundle(string assetBundleName)
		{
			if (assetBundleName == null)
			{
				return null;
			}
			if (IsAssetBundleDownloaded(assetBundleName))
			{
				if (assetBundleName != null && downloadFinished != null)
				{
					downloadFinished(assetBundleName);
				}
				return null;
			}
			popupNewContent = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupLoadingNewContentView>();
			if ((bool)popupNewContent && !popupNewContentEnable)
			{
				PopupLoadingNewContentView popupLoadingNewContentView = popupNewContent;
				popupLoadingNewContentView.stopDownload = (Action)Delegate.Combine(popupLoadingNewContentView.stopDownload, new Action(StopDownload));
				popupNewContentEnable = true;
			}
			AssetBundleLoadOperation assetBundleLoadOperation = null;
			assetBundleName = RemapVariantName(assetBundleName);
			LoadAssetBundle(assetBundleName);
			assetBundleLoadOperation = new AssetBundleDownloadingOperation(assetBundleName);
			m_InProgressOperations.Add(assetBundleLoadOperation);
			return assetBundleLoadOperation;
		}

		public string[] GetAllVariantsFromAssetBundle()
		{
			return m_AssetBundleManifest.GetAllAssetBundlesWithVariant();
		}

		public static string GetBundlePathForLoadFromFile(string relativePath)
		{
			string path = Application.dataPath + "!assets/";
			return Path.Combine(path, relativePath);
		}

		public static AssetBundle LoadBundleFromStreamingAssets(string relativePath)
		{
			return AssetBundle.LoadFromFile(GetBundlePathForLoadFromFile(relativePath));
		}
	}
}
