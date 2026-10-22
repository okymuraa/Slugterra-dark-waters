using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class AnnouncementConfiguration
	{
		public List<AnnouncementContents> GameAnnouncements;

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/InAppAnnouncements/Announcements", typeof(TextAsset));
			if (textAsset != null)
			{
				GameAnnouncements = JsonConvert.DeserializeObject<List<AnnouncementContents>>(textAsset.text);
			}
		}

		public List<AnnouncementContents> GetAnnouncesContainingBlaster(BlasterType blaster)
		{
			List<AnnouncementContents> list = new List<AnnouncementContents>();
			foreach (AnnouncementContents gameAnnouncement in GameAnnouncements)
			{
				if (gameAnnouncement.ListContents.Blasters.Contains(blaster))
				{
					list.Add(gameAnnouncement);
				}
			}
			return list;
		}
	}
}
