using System;
using System.Xml.Serialization;

namespace Slugterra.Configuration
{
	[Serializable]
	public class PackItemConfiguration : ShopItemConfiguration
	{
		[XmlElement]
		public PackType Type;

		public PackItemConfiguration()
		{
		}

		public PackItemConfiguration(PackType type, float realPrice, float gamePrice)
		{
			Type = type;
			RealPrice.Value = realPrice;
			GamePrice.Value = gamePrice;
		}
	}
}
