using AppsMinistry.Domain;
using Newtonsoft.Json;
using Slugterra.Configuration;

namespace Slugterra.Domain.Game.Units.Equipments
{
	public class Armor : DomainObject
	{
		public readonly float DamageAbsorption;

		public readonly float SpeedRatio;

		[JsonConstructor]
		private Armor()
		{
		}

		public Armor(ArmorLevels armorConfig)
		{
			DamageAbsorption = armorConfig.DamageAbsorption;
			SpeedRatio = armorConfig.SpeedRatio;
		}
	}
}
