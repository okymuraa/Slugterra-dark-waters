using AppsMinistry.Core;
using UnityEngine;

namespace AssetBundles
{
	public class AssetBundleDownloadingOperation : AssetBundleLoadOperation
	{
		protected string m_AssetBundleName;

		protected string m_DownloadingError;

		protected AsyncOperation m_Request;

		public AssetBundleDownloadingOperation(string assetbundleName)
		{
			m_AssetBundleName = assetbundleName;
		}

		public override string GetBundleName()
		{
			return m_AssetBundleName;
		}

		public override bool Update()
		{
			if (m_Request != null)
			{
				return false;
			}
			LoadedAssetBundle loadedAssetBundle = DonDestroyMonoSingleton<AssetBundleManager>.Instance.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
			if (loadedAssetBundle != null)
			{
				return true;
			}
			return false;
		}

		public override bool HaveError()
		{
			return m_DownloadingError != null;
		}

		public override bool IsDone()
		{
			if (m_Request == null && m_DownloadingError != null)
			{
				return true;
			}
			return m_Request != null && m_Request.isDone;
		}
	}
}
