using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class OffersConfiguration
	{
		private List<Offer> _offers = new List<Offer>();

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Offers/Offers", typeof(TextAsset));
			if (textAsset != null)
			{
				_offers = JsonConvert.DeserializeObject<List<Offer>>(textAsset.text);
			}
		}

		public Offer GetOffer(string mission)
		{
			return _offers.Find((Offer offer) => offer.Mission == mission);
		}
	}
}
