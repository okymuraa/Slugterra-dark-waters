using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using Slugterra.Scene.Daily;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class RecommendationsTabController : ShopTabController
	{
		[SerializeField]
		private ArmorItemFacade _armorFacade;

		[SerializeField]
		private BlasterItemFacade _blasterFacade;

		[SerializeField]
		private SlugItemFacade _slugFacade;

		[SerializeField]
		private AssistantItemFacade _allyFacade;

		private List<SlugItem> _recSlugs = new List<SlugItem>();

		private List<ArmorItem> _recArmors = new List<ArmorItem>();

		private List<BlasterItem> _recBlasters = new List<BlasterItem>();

		private List<AssistantItem> _recAllies = new List<AssistantItem>();

		protected override void _OnEnable()
		{
			base._OnEnable();
		}

		protected override void _OnDisable()
		{
			ClearRecommendations();
			base._OnDisable();
			SetFacade(ItemType.None);
		}

		public override void ApplyModel(object model)
		{
			if (model is ShopModel)
			{
				ShopModel shopModel = model as ShopModel;
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				List<string> list = null;
				list = ((!missionSetting.Mission.Name.Contains("Daily")) ? MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelRecommendationsList(MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission).Scene.Name) : MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelRecommendationsList(MonoSingleton<AppController>.Instance.Model.GetLevelItem(DailySceneManager.GetDaily().Logic).Scene.Name));
				if (list.Count == 0)
				{
					Debug.LogWarning("Recommendations list for current level " + MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission).Scene.Name + " is empty!");
					return;
				}
				RecommendationConfig recConfig = MonoSingleton<GameConfiguration>.Instance.RecommendationConfiguration.GetRecommendationById(list[0]);
				Transform parent = View.ShopItemsScrollScontroller.transform;
				List<ShopItemController> list2 = new List<ShopItemController>();
				List<BlasterItem> blasters = shopModel.Blasters;
				List<ArmorItem> armors = shopModel.Armors;
				List<AssistantItem> assistants = shopModel.Assistants;
				List<SlugItem> list3 = new List<SlugItem>();
				list3.AddRange(shopModel.AttackSlugs);
				list3.AddRange(shopModel.DefenceSlugs);
				list3.AddRange(shopModel.SpecialSlugs);
				for (int i = 0; i < recConfig.Items.Count; i++)
				{
					if (recConfig.Items[i].IsRequired)
					{
						continue;
					}
					switch (recConfig.Items[i].Type)
					{
					case ItemType.Ally:
					{
						AssistantItem assistantItem = assistants.Find((AssistantItem item) => item.Type.ToString() == recConfig.Items[i].ItemValue);
						assistantItem.RecommendedLevel = recConfig.Items[i].UpgradeLevel;
						assistantItem.IsRequired = recConfig.Items[i].IsRequired;
						_recAllies.Add(assistantItem);
						list2.Add(ShopItemController.Create<AssistansItemController>(assistantItem, parent));
						break;
					}
					case ItemType.Armor:
					{
						ArmorItem armorItem = armors.Find((ArmorItem item) => item.Type.ToString() == recConfig.Items[i].ItemValue);
						armorItem.RecommendedLevel = recConfig.Items[i].UpgradeLevel;
						armorItem.IsRequired = recConfig.Items[i].IsRequired;
						_recArmors.Add(armorItem);
						list2.Add(ShopItemController.Create<ArmorItemController>(armorItem, parent));
						break;
					}
					case ItemType.Blaster:
					{
						BlasterItem blasterItem = blasters.Find((BlasterItem item) => item.Type.ToString() == recConfig.Items[i].ItemValue);
						blasterItem.RecommendedLevel = recConfig.Items[i].UpgradeLevel;
						blasterItem.IsRequired = recConfig.Items[i].IsRequired;
						_recBlasters.Add(blasterItem);
						list2.Add(ShopItemController.Create<BlasterItemController>(blasterItem, parent));
						break;
					}
					case ItemType.Slug:
					{
						SlugItem slugItem = list3.Find((SlugItem item) => item.Type.ToString() == recConfig.Items[i].ItemValue);
						slugItem.RecommendedLevel = recConfig.Items[i].UpgradeLevel;
						slugItem.IsRequired = recConfig.Items[i].IsRequired;
						_recSlugs.Add(slugItem);
						list2.Add(ShopItemController.Create<SlugsItemController>(slugItem, parent));
						break;
					}
					}
				}
				for (int i2 = 0; i2 < recConfig.Items.Count; i2++)
				{
					if (!recConfig.Items[i2].IsRequired)
					{
						continue;
					}
					switch (recConfig.Items[i2].Type)
					{
					case ItemType.Ally:
					{
						AssistantItem assistantItem2 = assistants.Find((AssistantItem item) => item.Type.ToString() == recConfig.Items[i2].ItemValue);
						assistantItem2.RecommendedLevel = recConfig.Items[i2].UpgradeLevel;
						assistantItem2.IsRequired = recConfig.Items[i2].IsRequired;
						_recAllies.Add(assistantItem2);
						list2.Add(ShopItemController.Create<AssistansItemController>(assistantItem2, parent));
						break;
					}
					case ItemType.Armor:
					{
						ArmorItem armorItem2 = armors.Find((ArmorItem item) => item.Type.ToString() == recConfig.Items[i2].ItemValue);
						armorItem2.RecommendedLevel = recConfig.Items[i2].UpgradeLevel;
						armorItem2.IsRequired = recConfig.Items[i2].IsRequired;
						_recArmors.Add(armorItem2);
						list2.Add(ShopItemController.Create<ArmorItemController>(armorItem2, parent));
						break;
					}
					case ItemType.Blaster:
					{
						BlasterItem blasterItem2 = blasters.Find((BlasterItem item) => item.Type.ToString() == recConfig.Items[i2].ItemValue);
						blasterItem2.RecommendedLevel = recConfig.Items[i2].UpgradeLevel;
						blasterItem2.IsRequired = recConfig.Items[i2].IsRequired;
						_recBlasters.Add(blasterItem2);
						list2.Add(ShopItemController.Create<BlasterItemController>(blasterItem2, parent));
						break;
					}
					case ItemType.Slug:
					{
						SlugItem slugItem2 = list3.Find((SlugItem item) => item.Type.ToString() == recConfig.Items[i2].ItemValue);
						slugItem2.RecommendedLevel = recConfig.Items[i2].UpgradeLevel;
						slugItem2.IsRequired = recConfig.Items[i2].IsRequired;
						_recSlugs.Add(slugItem2);
						list2.Add(ShopItemController.Create<SlugsItemController>(slugItem2, parent));
						break;
					}
					}
				}
				View.ShopItemsScrollScontroller.ApplyModel(list2);
			}
			base.ApplyModel(model);
		}

		protected override int GetUnlockedItemsCount(object model)
		{
			return 0;
		}

		protected override void OnObjectSelected(ShopItemController item)
		{
			if (!(item == null))
			{
				base.OnObjectSelected(item);
				if (item is ArmorItemController)
				{
					SetFacade(ItemType.Armor);
					_armorFacade.ApplyModel(item.Model);
				}
				else if (item is BlasterItemController)
				{
					SetFacade(ItemType.Blaster);
					_blasterFacade.ApplyModel(item.Model);
				}
				else if (item is SlugsItemController)
				{
					SetFacade(ItemType.Slug);
					_slugFacade.ApplyModel(item.Model);
				}
				else if (item is AssistansItemController)
				{
					SetFacade(ItemType.Ally);
					_allyFacade.ApplyModel(item.Model);
				}
			}
		}

		private void SetFacade(ItemType type)
		{
			switch (type)
			{
			case ItemType.Ally:
				_allyFacade.gameObject.SetActive(true);
				break;
			case ItemType.Armor:
				_armorFacade.gameObject.SetActive(true);
				break;
			case ItemType.Blaster:
				_blasterFacade.gameObject.SetActive(true);
				break;
			case ItemType.Slug:
				_slugFacade.gameObject.SetActive(true);
				break;
			}
			_allyFacade.gameObject.SetActive(type == ItemType.Ally);
			_slugFacade.gameObject.SetActive(type == ItemType.Slug);
			_armorFacade.gameObject.SetActive(type == ItemType.Armor);
			_blasterFacade.gameObject.SetActive(type == ItemType.Blaster);
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
		}

		private void ClearRecommendations()
		{
			foreach (ArmorItem recArmor in _recArmors)
			{
				recArmor.RecommendedLevel = -1;
				recArmor.IsRequired = false;
			}
			foreach (BlasterItem recBlaster in _recBlasters)
			{
				recBlaster.RecommendedLevel = -1;
				recBlaster.IsRequired = false;
			}
			foreach (SlugItem recSlug in _recSlugs)
			{
				recSlug.RecommendedLevel = -1;
				recSlug.IsRequired = false;
			}
			foreach (AssistantItem recAlly in _recAllies)
			{
				recAlly.RecommendedLevel = -1;
				recAlly.IsRequired = false;
			}
		}
	}
}
