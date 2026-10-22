using System;

namespace AppsMinistry.Scene
{
	public class BundleSceneAttribute : Attribute
	{
		private string _assetBundleName;

		private bool _loadFromAsset;

		public string AssetBundleName
		{
			get
			{
				return _assetBundleName;
			}
		}

		public bool LoadFromAsset
		{
			get
			{
				return _loadFromAsset;
			}
		}

		public BundleSceneAttribute(string assetBundleName, bool loadFromAsset = false)
		{
			_assetBundleName = assetBundleName;
			_loadFromAsset = loadFromAsset;
		}
	}
}
