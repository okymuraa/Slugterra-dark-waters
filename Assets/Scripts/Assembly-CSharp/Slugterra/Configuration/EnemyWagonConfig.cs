using System;
using AppsMinistry.Domain.Configuration;

namespace Slugterra.Configuration
{
	[Serializable]
	[ConfigPath("Configs/Mechanism/Enemy/EnemyWagonConfig.xml")]
	public class EnemyWagonConfig : MechanismConfig
	{
	}
}
