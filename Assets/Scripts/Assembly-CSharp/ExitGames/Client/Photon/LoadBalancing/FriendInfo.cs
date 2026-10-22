namespace ExitGames.Client.Photon.LoadBalancing
{
	public class FriendInfo
	{
		public string Name { get; protected internal set; }

		public bool IsOnline { get; protected internal set; }

		public string Room { get; protected internal set; }

		public bool IsInRoom
		{
			get
			{
				return string.IsNullOrEmpty(Room);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}\t({1})\tin room: '{2}'", Name, (!IsOnline) ? "off" : "on", Room);
		}
	}
}
