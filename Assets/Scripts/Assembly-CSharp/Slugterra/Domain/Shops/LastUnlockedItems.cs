using AppsMinistry.Domain;
using Slugterra.Configuration;

namespace Slugterra.Domain.Shops
{
	public class LastUnlockedItems : DomainObject
	{
		public ArmorType Armor { get; set; }

		public BlasterType Blaster { get; set; }

		public SlugType Slug { get; set; }

		public AllyType Assistant { get; set; }

		public string Foe { get; set; }

		public LastUnlockedItems()
		{
			Armor = ArmorType.Default;
			Blaster = BlasterType.Default;
			Slug = SlugType.Infurnus;
			Assistant = AllyType.None;
			Foe = string.Empty;
		}
	}
}
