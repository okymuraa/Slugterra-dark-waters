using System;
using System.Xml.Serialization;
using AppsMinistry.Domain.Configuration;

namespace Slugterra.Configuration
{
	[Serializable]
	[XmlSerializeFormatter]
	public class MechanismConfig : Config
	{
		[XmlElement]
		public float Speed = 2f;

		[XmlElement]
		public float Health = 100f;
	}
}
