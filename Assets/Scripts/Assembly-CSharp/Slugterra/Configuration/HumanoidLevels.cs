namespace Slugterra.Configuration
{
	public class HumanoidLevels : BaseUnitConfig
	{
		public float MeleeDamage = 1.5f;

		public BlasterType BlasterType = BlasterType.Default;

		public int BlasterLevel;

		public ArmorType ArmorType;

		public int ArmorLevel;

		public float forceRate = -1f;

		public int restorationTimeInMinutes = 30;

		public int restorationPrice = 100;

		public string Avatar;

		public string MaterialPath = string.Empty;
	}
}
