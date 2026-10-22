using System;
using System.Xml.Serialization;
using AppsMinistry.Domain.Configuration;

namespace Slugterra.Configuration
{
	[Serializable]
	[XmlSerializeFormatter]
	[ConfigPath("Configs/Items/EnergyConfig.xml")]
	public class EnergyConfig : Config
	{
		[XmlElement]
		public float Energy = 5f;
	}
}
