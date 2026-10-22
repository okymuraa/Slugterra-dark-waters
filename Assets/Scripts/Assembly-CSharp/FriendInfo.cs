public class FriendInfo
{
	public string Name { get; protected internal set; }

	public bool IsOnline { get; protected internal set; }

	public string Room { get; protected internal set; }

	public bool IsInRoom
	{
		get
		{
			return IsOnline && !string.IsNullOrEmpty(Room);
		}
	}

	public override string ToString()
	{
		string name = Name;
		string arg;
		if (IsOnline)
		{
			arg = ((!IsInRoom) ? "on master" : "playing");
		}
		else
		{
			arg = "offline";
		}
		return string.Format("{0}\t is: {1}", name, arg);
	}
}
