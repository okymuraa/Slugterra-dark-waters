using System;
using System.Xml.Serialization;
using AppsMinistry.Domain.Configuration;

namespace Slugterra.Configuration
{
	[Serializable]
	[XmlSerializeFormatter]
	[ConfigPath("Configs/Items/MoneyConfig.xml")]
	public class MoneyConfig : Config
	{
		[XmlElement]
		public int Money = 1000;
	}
}
