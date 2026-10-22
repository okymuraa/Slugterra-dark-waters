using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using AssetBundles;
using UnityEngine;

public class LoadAssets : MonoBehaviour
{
	private static string _assetBundleName = "additional01";

	private string variantSceneAssetBundle = "high";

	private static string _sceneName = "2_3_QuietLawn";

	private string localPatchToBandle = "c:/AssetBundles/";

	private string loadingProgress;

	private string[] activeVariants;

	private List<string> assetBundleNames;

	private bool isURLdownload = true;

	private List<string> localBundles = new List<string> { "atlases" };

	private bool isManifestLoaded;

	private void Awake()
	{
		activeVariants = new string[1];
		DonDestroyMonoSingleton<AssetBundleManager>.Instance.ActiveVariants = activeVariants;
	}

	private void InitializeSourceURL()
	{
		DonDestroyMonoSingleton<AssetBundleManager>.Instance.SetSourceAssetBundleURL(DonDestroyMonoSingleton<AssetBundleManager>.Instance.BUNDLES_URL);
	}

	protected string OverrideDownloadingURLForLocalBundles(string baseAssetBundleName)
	{
		return null;
	}

	protected IEnumerator InitializeFromWeb()
	{
		InitializeSourceURL();
		AssetBundleLoadManifestOperation request = DonDestroyMonoSingleton<AssetBundleManager>.Instance.Initialize();
		if (request != null)
		{
			yield return StartCoroutine(request);
		}
	}

	public void DownloadAssetBundle(string assetBundleName)
	{
		_assetBundleName = assetBundleName;
		StartCoroutine(BeginDownloadAssetBundle(assetBundleName));
	}

	public void LoadAssetBundle(string assetBundleName)
	{
		_assetBundleName = assetBundleName;
		StartCoroutine(BeginDownloadAssetBundle(assetBundleName));
	}

	protected IEnumerator BeginDownloadAssetBundle(string assetBundleName)
	{
		_assetBundleName = assetBundleName;
		if (!DonDestroyMonoSingleton<AssetBundleManager>.Instance.isManifestLoaded)
		{
			yield return StartCoroutine(InitializeFromWeb());
		}
		if (DonDestroyMonoSingleton<AssetBundleManager>.Instance.isManifestLoaded)
		{
			DonDestroyMonoSingleton<AssetBundleManager>.Instance.ActiveVariants = activeVariants;
			yield return StartCoroutine(InitializeAssetBundle());
		}
	}

	protected IEnumerator InitializeAssetBundle()
	{
		AssetBundleLoadOperation request = DonDestroyMonoSingleton<AssetBundleManager>.Instance.DownloadAssetBundle(_assetBundleName);
		if (request != null)
		{
			yield return StartCoroutine(request);
		}
	}

	public void LoadLevelFromAssetBundle(string sceneName, string assetBundleName, bool isAsync, bool isAdditive)
	{
		_sceneName = sceneName;
		_assetBundleName = assetBundleName;
		StartCoroutine(BeginLoadLevelFromAssetBundle(sceneName, assetBundleName, isAsync, isAdditive));
	}

	protected IEnumerator BeginLoadLevelFromAssetBundle(string sceneName, string assetBundleName, bool isAsync, bool isAdditive)
	{
		_sceneName = sceneName;
		_assetBundleName = assetBundleName;
		if (!DonDestroyMonoSingleton<AssetBundleManager>.Instance.isManifestLoaded)
		{
			yield return StartCoroutine(InitializeFromWeb());
		}
		DonDestroyMonoSingleton<AssetBundleManager>.Instance.ActiveVariants = activeVariants;
		yield return StartCoroutine(InitializeLevel(_sceneName, isAdditive, isAsync));
	}

	protected IEnumerator InitializeLevel(string levelName, bool isAdditive, bool isAsync)
	{
		AssetBundleLoadOperation request = DonDestroyMonoSingleton<AssetBundleManager>.Instance.LoadLevel(_assetBundleName, levelName, isAdditive, isAsync);
		if (request != null)
		{
			yield return StartCoroutine(request);
		}
	}
}
