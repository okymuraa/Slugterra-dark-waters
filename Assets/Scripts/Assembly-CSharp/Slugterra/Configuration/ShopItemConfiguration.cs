using System;
using System.Xml.Serialization;

namespace Slugterra.Configuration
{
	[Serializable]
	public class ShopItemConfiguration
	{
		[XmlElement("RealPrice")]
		public Price RealPrice;

		[XmlElement("GamePrice")]
		public Price GamePrice;

		public ShopItemConfiguration()
		{
			RealPrice.Type = Currency.Slug;
			RealPrice.Value = 5f;
			GamePrice.Type = Currency.Gold;
			GamePrice.Value = 1000f;
		}
	}
}
