using System;
using AppsMinistry.Domain.Configuration;

namespace Slugterra.Configuration
{
	[Serializable]
	[ConfigPath("Configs/Mechanism/Ally/WagonConfig.xml")]
	public class WagonConfig : MechanismConfig
	{
	}
}
