using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using AssetBundles;
using UnityEngine;

namespace Slugterra.UI.Downloading
{
	public class DownloadingView : UIView
	{
		public static Action OnBtnRetryEvent;

		[SerializeField]
		private tk2dUIProgressBar progressBar;

		[SerializeField]
		private GameObject loadingContent;

		[SerializeField]
		private TextMesh lblTitle;

		[SerializeField]
		private TextMesh lblTitleShadow;

		[SerializeField]
		private TextMesh lblDescription;

		[SerializeField]
		private TextMesh lblDescriptionShadow;

		[SerializeField]
		private TextMesh lblFailDescription;

		[SerializeField]
		private TextMesh lblFailDescriptionShadow;

		private Dictionary<int, float> _bundlesDownloadProgress = new Dictionary<int, float>();

		[SerializeField]
		private string[] loadingTexts;

		public GameObject dataObject;

		public GameObject failObject;

		private bool internetError;

		private Color descBaseColor = default(Color);

		private Color descErrorColor = new Color(1f, 1f, 0f);

		protected override void OnStart()
		{
			lblTitle.text = MonoSingleton<Localization>.Instance.GetValueByKey("loading_words_00");
			lblTitleShadow.text = MonoSingleton<Localization>.Instance.GetValueByKey("loading_words_00");
			descBaseColor = lblDescription.color;
			lblDescription.gameObject.SetActive(false);
			dataObject.gameObject.SetActive(true);
			failObject.gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			Screen.sleepTimeout = -1;
		}

		private void OnDisable()
		{
			Screen.sleepTimeout = -2;
		}

		private void Update()
		{
			if (PlayerPrefs.GetInt("LoadBundlesEnable", 0) == 1 || DonDestroyMonoSingleton<AssetBundleManager>.Instance.itemsProgress.Count <= 0 || internetError)
			{
				return;
			}
			foreach (KeyValuePair<string, float> item in DonDestroyMonoSingleton<AssetBundleManager>.Instance.itemsProgress)
			{
				if (!item.Key.Contains("Android") && !item.Key.Contains("iOS") && !item.Key.Contains("WebGL"))
				{
					SetProgress(item.Value / 100f);
				}
			}
		}

		private void UpdateItems()
		{
			enabled = DonDestroyMonoSingleton<AssetBundleManager>.Instance.itemsProgress.Count > 0;
		}

		private void OnBtnRetryClick()
		{
			if (OnBtnRetryEvent != null)
			{
				OnBtnRetryEvent();
			}
			internetError = false;
		}

		public void InternetConnectionLost()
		{
			if (PlayerPrefs.GetInt("LoadBundlesEnable", 0) != 1)
			{
				lblDescription.gameObject.SetActive(true);
				lblDescription.color = descErrorColor;
				lblDescription.text = MonoSingleton<Localization>.Instance.GetValueByKey("alert_internet_error_short");
				lblDescriptionShadow.text = MonoSingleton<Localization>.Instance.GetValueByKey("alert_internet_error_short");
				internetError = true;
			}
		}

		public void RemoveInternetError()
		{
			internetError = false;
		}

		public void SetProgress(float progress)
		{
			if (DonDestroyMonoSingleton<DownloadManager>.Instance.bundlesToDownload <= 0)
			{
				return;
			}
			if (!_bundlesDownloadProgress.ContainsKey(DonDestroyMonoSingleton<DownloadManager>.Instance.numberOfDownloadedBundles))
			{
				_bundlesDownloadProgress.Add(DonDestroyMonoSingleton<DownloadManager>.Instance.numberOfDownloadedBundles, progress / (float)DonDestroyMonoSingleton<DownloadManager>.Instance.bundlesToDownload);
			}
			if (_bundlesDownloadProgress.Count > 0)
			{
				_bundlesDownloadProgress[DonDestroyMonoSingleton<DownloadManager>.Instance.numberOfDownloadedBundles] = progress / (float)DonDestroyMonoSingleton<DownloadManager>.Instance.bundlesToDownload;
				List<float> list = new List<float>(_bundlesDownloadProgress.Values);
				float num = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					num += list[i];
				}
				if (!loadingContent.gameObject.activeSelf)
				{
					loadingContent.gameObject.SetActive(true);
				}
				num = ((!(num > 1f)) ? num : 1f);
				progressBar.Value = num;
				int num2 = Mathf.Clamp((int)((float)(loadingTexts.Length - 1) * num), 0, loadingTexts.Length - 1);
				lblTitle.text = MonoSingleton<Localization>.Instance.GetValueByKey("loading_title") + (num * 100f).ToString("#") + "%";
				lblTitleShadow.text = MonoSingleton<Localization>.Instance.GetValueByKey("loading_title") + (num * 100f).ToString("#") + "%";
				if (!lblDescription.gameObject.activeSelf)
				{
					lblDescription.gameObject.SetActive(true);
				}
				lblDescription.color = descBaseColor;
				lblDescription.text = MonoSingleton<Localization>.Instance.GetValueByKey(loadingTexts[num2]);
				lblDescriptionShadow.text = MonoSingleton<Localization>.Instance.GetValueByKey(loadingTexts[num2]);
			}
		}
	}
}
