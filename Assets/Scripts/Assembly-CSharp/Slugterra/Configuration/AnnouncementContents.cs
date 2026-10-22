using System;
using System.Collections.Generic;

namespace Slugterra.Configuration
{
	public class AnnouncementContents : ICloneable
	{
		public string AnnouncementId = string.Empty;

		public AnnouncementType AnnouncementType;

		public AnnouncementPopupListContents ListContents;

		public AnnouncementPopupViewConditions Conditions;

		public AnnounceHeaderCharacter HeaderCharacter;

		public List<AnnounceHeaderBadge> HeaderBadges = new List<AnnounceHeaderBadge>();

		public string TitleTextKey = string.Empty;

		public string ContentTextKey = string.Empty;

		public string InAppPurchaseIdAndroid = string.Empty;

		public string InAppPurchaseIdIos = string.Empty;

		public float DiscountAmount;

		public AnnouncementContents()
		{
			ListContents = new AnnouncementPopupListContents();
			Conditions = new AnnouncementPopupViewConditions();
		}

		public object Clone()
		{
			AnnouncementContents announcementContents = (AnnouncementContents)MemberwiseClone();
			announcementContents.ListContents = (AnnouncementPopupListContents)ListContents.Clone();
			announcementContents.Conditions = (AnnouncementPopupViewConditions)Conditions.Clone();
			return announcementContents;
		}
	}
}
