using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BlastersTabController : ShopTabController
	{
		[SerializeField]
		private BlasterItemFacade _itemFacade;

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
				Transform parent = View.ShopItemsScrollScontroller.transform;
				List<BlasterItem> blasters = GetBlasters(shopModel);
				int lastUnlockedValue = 0;
				for (int num = blasters.Count - 1; num >= 0; num--)
				{
					if (!blasters[num].IsLocked && shopModel.LastUnlocked.Blaster == blasters[num].Type)
					{
						lastUnlockedValue = num;
						break;
					}
				}
				if (_toViewValue >= 0)
				{
					for (int num2 = blasters.Count - 1; num2 >= 0; num2--)
					{
						if (_toViewValue == (int)blasters[num2].Type)
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
				View.ShopItemsScrollScontroller.ApplyModel(ShopItemController.CreateList<BlasterItemController, BlasterItem>(blasters, parent));
				SetScrollToIssue();
			}
			base.ApplyModel(model);
		}

		protected override void OnObjectSelected(ShopItemController item)
		{
			base.OnObjectSelected(item);
			if (item != null)
			{
				_itemFacade.ApplyModel(item.Model);
			}
		}

		public override void UpdateCategory()
		{
			base.UpdateCategory();
			SetScrollToIssue();
		}

		public override bool SetScrollToIssue()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			List<BlasterItem> blasters = GetBlasters();
			if (!healthRestoreManager.GetHealthConfiguration(missionSetting.Blaster.Type).IsFullyRestored)
			{
				for (int num = blasters.Count - 1; num >= 0; num--)
				{
					if (blasters[num].Type == missionSetting.Blaster.Type)
					{
						View.LastScrollValue = Vector2.up * num;
						View.ShopItemsScrollScontroller.Controller.SmoothGoto(View.LastScrollValue);
						return true;
					}
				}
			}
			return false;
		}

		private List<BlasterItem> GetBlasters()
		{
			return GetBlasters(MonoSingleton<AppController>.Instance.Model.ShopModel);
		}

		private List<BlasterItem> GetBlasters(ShopModel model)
		{
			List<BlasterItem> list = new List<BlasterItem>();
			List<BlasterItem> blasters = model.Blasters;
			for (int i = 0; i < blasters.Count; i++)
			{
				BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blasters[i].Type);
				if (blusterConfiguration.isShowInStore)
				{
					list.Add(blasters[i]);
				}
			}
			return list;
		}

		protected override int GetUnlockedItemsCount(object model)
		{
			if (model is ShopModel)
			{
				int num = 0;
				{
					foreach (BlasterItem blaster in (model as ShopModel).Blasters)
					{
						if (!blaster.IsLocked)
						{
							num++;
						}
					}
					return num;
				}
			}
			return 0;
		}

		private void SetIsShown(BlasterType blaster)
		{
			PlayerPrefs.SetInt(blaster.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(BlasterType blaster)
		{
			return PlayerPrefs.GetInt(blaster.ToString() + "IsShown", 0) == 1;
		}
	}
}
