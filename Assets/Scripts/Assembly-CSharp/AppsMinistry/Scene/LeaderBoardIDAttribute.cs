using System;

namespace AppsMinistry.Scene
{
	public class LeaderBoardIDAttribute : Attribute
	{
		private string _leaderBoardId;

		public string LeaderBoardId
		{
			get
			{
				return _leaderBoardId;
			}
		}

		public LeaderBoardIDAttribute(string leaderBoardId)
		{
			_leaderBoardId = leaderBoardId;
		}
	}
}
