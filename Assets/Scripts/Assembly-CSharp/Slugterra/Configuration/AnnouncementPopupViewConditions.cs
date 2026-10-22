using System;
using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class AnnouncementPopupViewConditions : ICloneable
	{
		public List<string> FilterLevels;

		public AnnouncementPopupListContents FilterObtainedItems;

		public int MaxViewCount;

		public int ViewPriority;

		public bool ShowAnyway;

		public bool ShowBefore;

		public AnnouncementPopupViewConditions()
		{
			FilterLevels = new List<string>();
			FilterObtainedItems = new AnnouncementPopupListContents();
		}

		public object Clone()
		{
			AnnouncementPopupViewConditions announcementPopupViewConditions = (AnnouncementPopupViewConditions)MemberwiseClone();
			announcementPopupViewConditions.FilterObtainedItems = (AnnouncementPopupListContents)FilterObtainedItems.Clone();
			announcementPopupViewConditions.FilterLevels = new List<string>(FilterLevels);
			return announcementPopupViewConditions;
		}
	}
}
