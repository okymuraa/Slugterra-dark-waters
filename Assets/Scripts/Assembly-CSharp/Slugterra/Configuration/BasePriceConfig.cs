using Newtonsoft.Json;

namespace Slugterra.Configuration
{
	public class BasePriceConfig
	{
		[JsonProperty("price")]
		public float priceGold;

		public float priceGems;

		public string productId = string.Empty;
	}
}
