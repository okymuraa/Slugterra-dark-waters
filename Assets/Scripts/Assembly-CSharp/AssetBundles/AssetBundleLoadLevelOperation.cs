using AppsMinistry.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetBundles
{
	public class AssetBundleLoadLevelOperation : AssetBundleLoadOperation
	{
		protected string m_AssetBundleName;

		protected string m_LevelName;

		protected bool m_IsAdditive;

		protected bool m_IsAsync;

		protected string m_DownloadingError;

		protected AsyncOperation m_Request;

		public AssetBundleLoadLevelOperation(string assetbundleName, string levelName, bool isAdditive, bool isAsync)
		{
			m_AssetBundleName = assetbundleName;
			m_LevelName = levelName;
			m_IsAdditive = isAdditive;
			m_IsAsync = isAsync;
			Debug.Log(string.Format("AssetBundleLoadLevelOperation START task for downloading {0}, open {1}, isAdditive {2}, isAsync {3}", m_AssetBundleName, m_LevelName, m_IsAdditive, m_IsAsync));
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
				if (m_IsAdditive)
				{
					if (m_IsAsync)
					{
						Debug.Log(string.Format("AssetBundleLoadLevelOperation COMPLETE task for downloading {0}, open {1}, isAdditive {2}, isAsync {3}", m_AssetBundleName, m_LevelName, m_IsAdditive, m_IsAsync));
						m_Request = SceneManager.LoadSceneAsync(m_LevelName, LoadSceneMode.Additive);
					}
					else
					{
						Debug.Log(string.Format("AssetBundleLoadLevelOperation COMPLETE task for downloading {0}, open {1}, isAdditive {2}, isAsync {3}", m_AssetBundleName, m_LevelName, m_IsAdditive, m_IsAsync));
						SceneManager.LoadScene(m_LevelName, LoadSceneMode.Additive);
					}
				}
				else if (m_IsAsync)
				{
					Debug.Log(string.Format("AssetBundleLoadLevelOperation COMPLETE task for downloading {0}, open {1}, isAdditive {2}, isAsync {3}", m_AssetBundleName, m_LevelName, m_IsAdditive, m_IsAsync));
					m_Request = SceneManager.LoadSceneAsync(m_LevelName);
				}
				else
				{
					Debug.Log(string.Format("AssetBundleLoadLevelOperation COMPLETE task for downloading {0}, open {1}, isAdditive {2}, isAsync {3}", m_AssetBundleName, m_LevelName, m_IsAdditive, m_IsAsync));
					SceneManager.LoadScene(m_LevelName);
				}
				return false;
			}
			return true;
		}

		public override bool HaveError()
		{
			return m_DownloadingError != null;
		}

		public override string GetBundleName()
		{
			return m_AssetBundleName;
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
