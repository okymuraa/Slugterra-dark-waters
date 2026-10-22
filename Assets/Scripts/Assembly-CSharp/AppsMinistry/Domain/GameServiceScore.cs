using System;
using UnityEngine.SocialPlatforms;

namespace AppsMinistry.Domain
{
	public class GameServiceScore : DomainObject
	{
		public string Category;

		public long Value;

		public ulong Context;

		public DateTime Date;

		public string PlayerId;

		public int Rank;

		public bool IsFriend;

		public string Alias;

		public int MaxRange;

		public string LeaderBoardId;

		public string FormattedValue
		{
			get
			{
				return string.Format("{0}", Convert.ToInt32(Value));
			}
		}

		public GameServiceScore()
		{
		}

		public GameServiceScore(GPScore score)
		{
			PlayerId = score.PlayerId;
			Rank = score.Rank;
			Value = score.LongScore;
			LeaderBoardId = score.LeaderboardId;
		}

		public GameServiceScore(IScore score)
		{
			PlayerId = score.userID;
			Rank = score.rank;
			Value = score.value;
			LeaderBoardId = score.leaderboardID;
		}
	}
}
