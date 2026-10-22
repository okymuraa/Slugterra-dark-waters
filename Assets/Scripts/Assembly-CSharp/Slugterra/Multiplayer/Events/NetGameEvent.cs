namespace Slugterra.Multiplayer.Events
{
	public enum NetGameEvent : byte
	{
		PlayerReady = 0,
		PlayerReadyForRestart = 1,
		PlayerLeftGame = 2,
		DuelStart = 3
	}
}
