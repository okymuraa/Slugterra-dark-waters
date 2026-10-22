using System.Collections.Generic;

namespace AppsMinistry.Core
{
	public class LocalizeLanguages
	{
		public Dictionary<string, string> fonts = new Dictionary<string, string>();

		public Dictionary<string, string> fontsCustomMaterials = new Dictionary<string, string>();

		public Dictionary<string, string> locales = new Dictionary<string, string>();

		public Dictionary<string, KeyValueLocalize> localization = new Dictionary<string, KeyValueLocalize>();
	}
}
