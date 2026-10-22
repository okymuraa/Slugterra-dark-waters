using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ArmorsTabController : ShopTabController
	{
		[SerializeField]
		private ArmorItemFacade _itemFacade;

		protected override void _OnEnable()
		{
			base._OnEnable();
			_itemFacade.gameObject.SetActive(true);
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			_itemFacade.gameObject.SetActive(false);
		}

		public override void ApplyModel(object model)
		{
			if (model is ShopModel)
			{
				ShopModel shopModel = model as ShopModel;
				List<ArmorItem> armors = shopModel.Armors;
				Transform parent = View.ShopItemsScrollScontroller.transform;
				List<ArmorItem> list = new List<ArmorItem>();
				for (int i = 0; i < armors.Count; i++)
				{
					ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armors[i].Type);
					if (armorConfiguration.isShowInStore)
					{
						list.Add(armors[i]);
					}
				}
				int lastUnlockedValue = 0;
				for (int num = list.Count - 1; num >= 0; num--)
				{
					if (!list[num].IsLocked && shopModel.LastUnlocked.Armor == list[num].Type)
					{
						lastUnlockedValue = num;
						break;
					}
				}
				if (_toViewValue >= 0)
				{
					for (int num2 = list.Count - 1; num2 >= 0; num2--)
					{
						if (_toViewValue == (int)list[num2].Type)
						{
							View.LastScrollValue = Vector2.up * num2;
							break;
						}
					}
				}
				_toViewValue = -1;
				if (View.Badge != null && View.Badge.gameObject.activeSelf)
				{
					View.LastUnlockedValue = lastUnlockedValue;
				}
				List<ShopItemController> model2 = ShopItemController.CreateList<ArmorItemController, ArmorItem>(list, parent);
				View.ShopItemsScrollScontroller.ApplyModel(model2);
			}
			base.ApplyModel(model);
		}

		protected override int GetUnlockedItemsCount(object model)
		{
			if (model is ShopModel)
			{
				int num = 0;
				{
					foreach (ArmorItem armor in (model as ShopModel).Armors)
					{
						ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armor.Type);
						if (!armor.IsLocked)
						{
							num++;
						}
					}
					return num;
				}
			}
			return 0;
		}

		private void SetIsShown(ArmorType ally)
		{
			PlayerPrefs.SetInt(ally.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(ArmorType ally)
		{
			return PlayerPrefs.GetInt(ally.ToString() + "IsShown", 0) == 1;
		}

		protected override void OnObjectSelected(ShopItemController item)
		{
			base.OnObjectSelected(item);
			if (item != null)
			{
				_itemFacade.ApplyModel(item.Model);
			}
		}
	}
}
