using System;
using System.Xml.Serialization;
using AppsMinistry.Domain.Configuration;

namespace Slugterra.Configuration
{
	[Serializable]
	[XmlSerializeFormatter]
	[ConfigPath("Configs/Items/HeartConfig.xml")]
	public class HeartConfig : Config
	{
		[XmlElement]
		public float Heart = 3f;
	}
}
