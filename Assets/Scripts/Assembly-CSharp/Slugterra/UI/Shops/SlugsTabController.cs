using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugsTabController : ShopTabController
	{
		[SerializeField]
		private SlugItemFacade _itemFacade;

		private ShopModel shopModel;

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
				shopModel = model as ShopModel;
				Transform parent = View.ShopItemsScrollScontroller.transform;
				List<SlugItem> slugs = GetSlugs(shopModel);
				int lastUnlockedValue = 0;
				for (int num = slugs.Count - 1; num >= 0; num--)
				{
					if (!slugs[num].IsLocked && shopModel.LastUnlocked.Slug == slugs[num].Type)
					{
						lastUnlockedValue = num;
						break;
					}
				}
				if (_toViewValue >= 0)
				{
					for (int num2 = slugs.Count - 1; num2 >= 0; num2--)
					{
						if (_toViewValue == (int)slugs[num2].Type)
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
				View.ShopItemsScrollScontroller.ApplyModel(ShopItemController.CreateList<SlugsItemController, SlugItem>(slugs, parent));
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
			List<SlugItem> slugs = GetSlugs();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			SlugItem slugItem = null;
			if (healthRestoreManager.GetHealthConfiguration(missionSetting.AttackSlug.Type).IsFullyRestored)
			{
				if (missionSetting.DefenceSlug != null && !healthRestoreManager.GetHealthConfiguration(missionSetting.DefenceSlug.Type).IsFullyRestored)
				{
					slugItem = missionSetting.DefenceSlug;
				}
				else if (missionSetting.SpecialSlug != null && !healthRestoreManager.GetHealthConfiguration(missionSetting.SpecialSlug.Type).IsFullyRestored)
				{
					slugItem = missionSetting.SpecialSlug;
				}
			}
			else
			{
				slugItem = missionSetting.AttackSlug;
			}
			if (slugItem != null)
			{
				for (int num = slugs.Count - 1; num >= 0; num--)
				{
					if (slugs[num].Type == slugItem.Type)
					{
						View.LastScrollValue = Vector2.up * num;
						View.ShopItemsScrollScontroller.Controller.SmoothGoto(View.LastScrollValue);
						return true;
					}
				}
			}
			return false;
		}

		private List<SlugItem> GetSlugs()
		{
			return GetSlugs(MonoSingleton<AppController>.Instance.Model.ShopModel);
		}

		private List<SlugItem> GetSlugs(ShopModel model)
		{
			List<SlugItem> list = new List<SlugItem>();
			List<SlugItem> list2 = new List<SlugItem>();
			list2.AddRange(model.AttackSlugs);
			list2.AddRange(model.DefenceSlugs);
			list2.AddRange(model.SpecialSlugs);
			for (int i = 0; i < list2.Count; i++)
			{
				SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(list2[i].Type);
				if (slugConfiguration.isShowInStore)
				{
					list.Add(list2[i]);
				}
			}
			return list;
		}

		private void SetIsShown(SlugType slug)
		{
			PlayerPrefs.SetInt(slug.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(SlugType slug)
		{
			return PlayerPrefs.GetInt(slug.ToString() + "IsShown", 0) == 1;
		}

		protected override int GetUnlockedItemsCount(object model)
		{
			if (model is ShopModel)
			{
				int num = 0;
				foreach (SlugItem attackSlug in (model as ShopModel).AttackSlugs)
				{
					SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(attackSlug.Type);
					if (slugConfiguration.isShowInStore && !attackSlug.IsLocked)
					{
						num++;
					}
				}
				foreach (SlugItem defenceSlug in (model as ShopModel).DefenceSlugs)
				{
					SlugConfig slugConfiguration2 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(defenceSlug.Type);
					if (slugConfiguration2.isShowInStore && !defenceSlug.IsLocked)
					{
						num++;
					}
				}
				foreach (SlugItem specialSlug in (model as ShopModel).SpecialSlugs)
				{
					SlugConfig slugConfiguration3 = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(specialSlug.Type);
					if (slugConfiguration3.isShowInStore && !specialSlug.IsLocked)
					{
						num++;
					}
				}
				return num + 2;
			}
			return 0;
		}
	}
}
