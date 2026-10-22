using System;
using System.Collections.Generic;
using AppsMinistry.Core;

namespace Slugterra.Configuration
{
	[Serializable]
	public class ShopConfig
	{
		public List<AssistantItemConfiguration> AssistantItems;

		public List<PackItemConfiguration> PackItems;

		public ShopConfig()
		{
			AssistantItems = new List<AssistantItemConfiguration>();
			PackItems = new List<PackItemConfiguration>();
			InitItems();
		}

		private void InitItems()
		{
			float gamePrice = 100f;
			for (PackType packType = PackType.First; packType < PackType.Last; packType++)
			{
				PackItems.Add(new PackItemConfiguration(packType, 0f, gamePrice));
			}
			for (AllyType allyType = AllyType.Pronto; allyType <= AllyType.Kord; allyType++)
			{
				for (int i = 0; i < 10; i++)
				{
					HumanoidLevels alliesConfigurationLevel = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(allyType, i);
					AssistantItems.Add(new AssistantItemConfiguration(allyType, i, 0f, alliesConfigurationLevel.priceGold));
				}
			}
		}

		public PackItemConfiguration GetPackConfig(PackType type)
		{
			return PackItems.Find((PackItemConfiguration pack) => pack.Type == type);
		}

		public AssistantItemConfiguration GetAssistantConfig(AllyType type, int level)
		{
			return AssistantItems.Find((AssistantItemConfiguration assistant) => assistant.Type == type && assistant.Level == level);
		}
	}
}
