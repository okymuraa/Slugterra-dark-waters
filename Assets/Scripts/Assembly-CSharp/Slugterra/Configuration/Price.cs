using System;
using System.Xml.Serialization;

namespace Slugterra.Configuration
{
	[Serializable]
	public struct Price
	{
		[XmlAttribute("Currency")]
		public Currency Type;

		[XmlAttribute("Value")]
		public float Value;
	}
}
