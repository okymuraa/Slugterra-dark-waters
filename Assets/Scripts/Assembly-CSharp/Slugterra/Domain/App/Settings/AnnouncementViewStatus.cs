using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Slugterra.Domain.App.Settings
{
	[Serializable]
	public class AnnouncementViewStatus
	{
		public AnnounceStatus CurrentStatus;

		public List<string> LevelViews = new List<string>();

		[JsonIgnore]
		public string ViewedSessionLevelName = string.Empty;

		public AnnouncementViewStatus(AnnounceStatus status)
		{
			CurrentStatus = status;
		}

		public void UpdateStatus(AnnounceStatus newStatus, string levelName = "")
		{
			if (newStatus == AnnounceStatus.Done || newStatus == AnnounceStatus.Ignored)
			{
				if (!LevelViews.Contains(levelName))
				{
					LevelViews.Add(levelName);
				}
				ViewedSessionLevelName += levelName;
			}
			CurrentStatus = newStatus;
		}
	}
}
