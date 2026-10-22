using Newtonsoft.Json;

namespace Slugterra.Multiplayer.Lobby.Match
{
	public class PlayerData
	{
		[JsonProperty("N")]
		public string Name;

		[JsonProperty("R")]
		public int Rank;

		[JsonProperty("A")]
		public byte AvatarId;

		[JsonProperty("AS")]
		public PlayerDataSlug AttackSlug;

		[JsonProperty("CS")]
		public PlayerDataSlug ControlSlug;

		[JsonProperty("SS")]
		public PlayerDataSlug SpecialSlug;

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static PlayerData FromJson(string value)
		{
			return JsonConvert.DeserializeObject<PlayerData>(value);
		}
	}
}
