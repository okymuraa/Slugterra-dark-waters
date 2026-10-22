using Newtonsoft.Json;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;

namespace Slugterra.Multiplayer.Lobby.Match
{
	public struct PlayerDataSlug
	{
		[JsonProperty("S")]
		public SlugType Slug;

		[JsonProperty("L")]
		public byte Level;

		public PlayerDataSlug(SlugItem slug)
		{
			Slug = SlugType.None;
			Level = 0;
			if (slug != null)
			{
				Slug = slug.Type;
				Level = (byte)slug.Level;
			}
		}

		public bool IsEmpty()
		{
			return Slug == SlugType.None;
		}
	}
}
