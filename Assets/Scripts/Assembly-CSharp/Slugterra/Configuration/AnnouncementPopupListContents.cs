using System;
using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class AnnouncementPopupListContents : ICloneable
	{
		public List<ArmorType> Armors = new List<ArmorType>();

		public List<SlugType> Slugs = new List<SlugType>();

		public List<BlasterType> Blasters = new List<BlasterType>();

		public List<AllyType> Allies = new List<AllyType>();

		public bool IsEmpty()
		{
			return Armors.Count == 0 && Slugs.Count == 0 && Blasters.Count == 0 && Allies.Count == 0;
		}

		public int Count()
		{
			return Armors.Count + Slugs.Count + Blasters.Count + Allies.Count;
		}

		public object Clone()
		{
			AnnouncementPopupListContents announcementPopupListContents = (AnnouncementPopupListContents)MemberwiseClone();
			announcementPopupListContents.Armors = new List<ArmorType>(Armors);
			announcementPopupListContents.Blasters = new List<BlasterType>(Blasters);
			announcementPopupListContents.Allies = new List<AllyType>(Allies);
			announcementPopupListContents.Slugs = new List<SlugType>(Slugs);
			return announcementPopupListContents;
		}
	}
}
