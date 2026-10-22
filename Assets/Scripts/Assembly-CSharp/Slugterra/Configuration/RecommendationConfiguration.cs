using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Configuration
{
	public class RecommendationConfiguration
	{
		private List<RecommendationConfig> Recommendations;

		public void Initialize()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Configs/Recommendations/Recommendations", typeof(TextAsset));
			if (textAsset != null)
			{
				Recommendations = JsonConvert.DeserializeObject<List<RecommendationConfig>>(textAsset.text);
			}
		}

		public RecommendationConfig GetRecommendationById(string id)
		{
			return Recommendations.Find((RecommendationConfig config) => config.Id == id);
		}
	}
}
