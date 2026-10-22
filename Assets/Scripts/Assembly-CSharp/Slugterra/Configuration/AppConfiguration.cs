using AppsMinistry.Core;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	[AppSingleton]
	public class AppConfiguration : MonoSingleton<AppConfiguration>
	{
		public static readonly string Name = "Slugterra";

		public static readonly string Version = "2.0.6";

		public static readonly string Build = "2275";

		public static readonly string FullVersion = Version + " build " + Build;

		public static readonly string FullName = Name + " " + FullVersion;

		private ShopConfig _shopConfig;

		private bool _isLoaded;

		public ShopConfig ShopConfig
		{
			get
			{
				return _shopConfig;
			}
		}

		protected override void OnCreate()
		{
			Load();
		}

		public void Load()
		{
			if (!_isLoaded)
			{
				TextAsset textAsset = (TextAsset)Resources.Load("Configs/Shops/ShopConfig", typeof(TextAsset));
				if (textAsset != null)
				{
					_shopConfig = JsonConvert.DeserializeObject<ShopConfig>(textAsset.text);
				}
				if (_shopConfig == null)
				{
					_shopConfig = new ShopConfig();
				}
			}
		}

		public void Save()
		{
			string stringToSave = JsonConvert.SerializeObject(_shopConfig);
			IOSystem.SaveStrings(Application.dataPath + "/Resources/Configs/Shops", "ShopConfig.txt", stringToSave);
		}
	}
}
