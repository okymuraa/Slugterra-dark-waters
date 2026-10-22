using UnityEngine;

namespace AssetBundles
{
	public class Utility
	{
		public const string AssetBundlesOutputPath = "AssetBundles";

		public static string GetPlatformName()
		{
			return GetPlatformForAssetBundles(Application.platform);
		}

		private static string GetPlatformForAssetBundles(RuntimePlatform platform)
		{
			switch (platform)
			{
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			case RuntimePlatform.WebGLPlayer:
				return "WebGL";
			case RuntimePlatform.tvOS:
				return "tvOS";
#if !UNITY_2018_1_OR_NEWER
			case RuntimePlatform.OSXWebPlayer:
			case RuntimePlatform.WindowsWebPlayer:
				return "WebPlayer";
#endif
			case RuntimePlatform.WindowsPlayer:
				return "Windows";
			case RuntimePlatform.OSXPlayer:
				return "OSX";
			default:
				return null;
			}
		}
	}
}
