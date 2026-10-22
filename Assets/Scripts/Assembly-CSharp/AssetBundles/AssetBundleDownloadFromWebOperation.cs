using System;
using AppsMinistry.Utils;
using UnityEngine;

namespace AssetBundles
{
	public class AssetBundleDownloadFromWebOperation : AssetBundleDownloadOperation
	{
		private WWW m_WWW;

		private string m_Url;

		private float _progress;

		protected string m_DownloadingError;

		private bool isManifest;

		public override float downloadProgress
		{
			get
			{
				return _progress;
			}
			protected set
			{
				_progress = value;
			}
		}

		protected override bool downloadIsDone
		{
			get
			{
				return m_WWW == null || m_WWW.isDone;
			}
		}

		public AssetBundleDownloadFromWebOperation(string assetBundleName, WWW www, bool _isManifest)
			: base(assetBundleName)
		{
			if (www == null)
			{
				throw new ArgumentNullException("www");
			}
			m_Url = www.url;
			m_WWW = www;
			m_DownloadingError = www.error;
			isManifest = _isManifest;
		}

		public override bool Update()
		{
			bool result = base.Update();
			if (m_WWW != null)
			{
				_progress = m_WWW.progress;
				m_DownloadingError = m_WWW.error;
			}
			return result;
		}

		public override bool HaveError()
		{
			return m_DownloadingError != null;
		}

		public override string GetBundleName()
		{
			return string.Empty;
		}

		protected override void FinishDownload()
		{
			error = m_WWW.error;
			if (string.IsNullOrEmpty(error))
			{
				AssetBundle assetBundle = m_WWW.assetBundle;
				if (assetBundle == null)
				{
					error = string.Format("{0} is not a valid asset bundle.", assetBundleName);
				}
				else
				{
					base.assetBundle = new LoadedAssetBundle(m_WWW.assetBundle);
				}
				if (isManifest)
				{
					byte[] data = m_WWW.bytes;
					IOSystem.SaveBytes(Application.persistentDataPath, "lastManifest" + assetBundleName, ref data);
				}
				m_WWW.Dispose();
				m_WWW = null;
			}
		}

		public override void StopDownload()
		{
		}

		public override string GetSourceURL()
		{
			return m_Url;
		}
	}
}
