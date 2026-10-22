using System.Xml.Serialization;

namespace Slugterra.Configuration
{
	public class AssistantItemConfiguration : ShopItemConfiguration
	{
		[XmlElement]
		public AllyType Type;

		[XmlElement]
		public int Level;

		public AssistantItemConfiguration()
		{
		}

		public AssistantItemConfiguration(AllyType type, int level, float realPrice, float gamePrice)
		{
			Type = type;
			Level = level;
			RealPrice.Value = realPrice;
			GamePrice.Value = gamePrice;
		}
	}
}
