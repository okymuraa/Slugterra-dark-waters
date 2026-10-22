using System;

namespace AppsMinistry.Domain
{
	public class GameServiceChallenge : DomainObject
	{
		public string IssuingPlayerID;

		public string ReceivingPlayerID;

		public GameServiceChallengeState State;

		public DateTime IssueDate;

		public DateTime CompletionDate;

		public string Message;

		public string Hash;

		public GameServiceScore Score;

		public GameServiceChallenge()
		{
			Score = new GameServiceScore();
		}
	}
}
