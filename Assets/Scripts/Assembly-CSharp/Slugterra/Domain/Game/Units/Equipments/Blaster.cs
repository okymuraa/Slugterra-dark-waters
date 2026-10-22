using AppsMinistry.Domain;
using Newtonsoft.Json;
using Slugterra.Configuration;

namespace Slugterra.Domain.Game.Units.Equipments
{
	public class Blaster : DomainObject
	{
		public readonly float Range;

		public readonly float SlugReloadTime;

		public readonly float DamagePerHit;

		public readonly float FireRate;

		public readonly float SlugSpeed;

		public readonly int MagazineCapacity;

		public readonly BlasterType Type;

		[JsonConstructor]
		private Blaster()
		{
		}

		public Blaster(BlasterLevels blasterConfig, BlasterType type)
		{
			Range = blasterConfig.AttackRange;
			SlugReloadTime = blasterConfig.SlugReloadTime;
			DamagePerHit = blasterConfig.Damage;
			FireRate = blasterConfig.FireInterval;
			SlugSpeed = blasterConfig.ProjectileSpeed;
			MagazineCapacity = 1;
			Type = type;
			if (Type == BlasterType.PremiumBlakk)
			{
				MagazineCapacity = 3;
			}
		}
	}
}
