using UnityEngine;

namespace AppsMinistry
{
	public sealed class TagManager_Android : ITagManager
	{
		private AndroidJavaClass mTagManagerClass;

		private AndroidJavaObject mCurrentActivity;

		private AndroidJavaObject mTagManager;

		private AndroidJavaObject mDataLayer;

		private AndroidJavaObject mContainer;

		public TagManager_Android()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				mCurrentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.google.android.gms.tagmanager.TagManager");
				mTagManager = androidJavaClass2.CallStatic<AndroidJavaObject>("getInstance", new object[1] { mCurrentActivity });
				mDataLayer = mTagManager.Call<AndroidJavaObject>("getDataLayer", new object[0]);
			}
		}

		public void LoadContainerDefaultOnly(string containerId, int defaultContainerResourceId)
		{
			AndroidJavaObject androidJavaObject = mTagManager.Call<AndroidJavaObject>("loadContainerDefaultOnly", new object[2] { containerId, defaultContainerResourceId });
		}

		public void LoadContainerPreferFresh(string containerId, int defaultContainerResourceId)
		{
			AndroidJavaObject androidJavaObject = mTagManager.Call<AndroidJavaObject>("loadContainerPreferFresh", new object[2] { containerId, defaultContainerResourceId });
		}

		public void LoadContainerPreferNonDefault(string containerId, int defaultContainerResourceId)
		{
			AndroidJavaObject androidJavaObject = mTagManager.Call<AndroidJavaObject>("loadContainerPreferNonDefault", new object[2] { containerId, defaultContainerResourceId });
		}
	}
}
