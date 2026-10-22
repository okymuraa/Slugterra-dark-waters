using System.Collections.Generic;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class FoesTabController : ShopTabController
	{
		[SerializeField]
		private FoeItemFacade _itemFacade;

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
				List<FoeItem> foes = shopModel.Foes;
				Transform parent = View.ShopItemsScrollScontroller.transform;
				int num = 0;
				for (int num2 = foes.Count - 1; num2 >= 0; num2--)
				{
					if (!foes[num2].IsLocked && shopModel.LastUnlocked.Foe == foes[num2].Type)
					{
						num = num2;
						break;
					}
				}
				View.ShopItemsScrollScontroller.ApplyModel(ShopItemController.CreateList<FoeItemController, FoeItem>(foes, parent));
			}
			base.ApplyModel(model);
		}

		private void SetIsShown(string slug)
		{
			PlayerPrefs.SetInt(slug + "IsShown", 1);
		}

		private bool GetIsShown(string slug)
		{
			return PlayerPrefs.GetInt(slug + "IsShown", 0) == 1;
		}

		protected override int GetUnlockedItemsCount(object model)
		{
			if (model is ShopModel)
			{
				int num = 0;
				{
					foreach (FoeItem foe in (model as ShopModel).Foes)
					{
						if (!foe.IsLocked)
						{
							num++;
						}
					}
					return num;
				}
			}
			return 0;
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
