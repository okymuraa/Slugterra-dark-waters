using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class AssistantsTabController : ShopTabController
	{
		[SerializeField]
		private AssistantItemFacade _itemFacade;

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
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				List<AssistantItem> assistants = shopModel.Assistants;
				Transform parent = View.ShopItemsScrollScontroller.transform;
				int lastUnlockedValue = 0;
				if (missionSetting.Assistant != null && missionSetting.Assistant.Type != AllyType.None)
				{
					for (int num = assistants.Count - 1; num >= 0; num--)
					{
						if (assistants[num].Type == missionSetting.Assistant.Type)
						{
							_toViewValue = (int)assistants[num].Type;
							break;
						}
					}
				}
				for (int num2 = assistants.Count - 1; num2 >= 0; num2--)
				{
					if (!assistants[num2].IsLocked && shopModel.LastUnlocked.Assistant == assistants[num2].Type)
					{
						lastUnlockedValue = num2;
						break;
					}
				}
				if (_toViewValue >= 0)
				{
					for (int num3 = assistants.Count - 1; num3 >= 0; num3--)
					{
						if (_toViewValue == (int)assistants[num3].Type)
						{
							View.LastScrollValue = Vector2.up * num3;
							break;
						}
					}
				}
				_toViewValue = -1;
				if (View.Badge != null && View.Badge.gameObject.activeSelf)
				{
					View.LastUnlockedValue = lastUnlockedValue;
				}
				View.ShopItemsScrollScontroller.ApplyModel(ShopItemController.CreateList<AssistansItemController, AssistantItem>(assistants, parent));
				SetScrollToIssue();
			}
			base.ApplyModel(model);
		}

		protected override int GetUnlockedItemsCount(object model)
		{
			if (model is ShopModel)
			{
				int num = 0;
				{
					foreach (AssistantItem assistant in (model as ShopModel).Assistants)
					{
						if (!assistant.IsLocked)
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

		private void SetIsShown(AllyType ally)
		{
			PlayerPrefs.SetInt(ally.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(AllyType ally)
		{
			return PlayerPrefs.GetInt(ally.ToString() + "IsShown", 0) == 1;
		}

		public override void UpdateCategory()
		{
			base.UpdateCategory();
			SetScrollToIssue();
		}

		public override bool SetScrollToIssue()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			List<AssistantItem> assistants = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants;
			if (missionSetting.Assistant != null && missionSetting.Assistant.Type != AllyType.None && !healthRestoreManager.GetHealthConfiguration(missionSetting.Assistant.Type).IsFullyRestored)
			{
				for (int num = assistants.Count - 1; num >= 0; num--)
				{
					if (assistants[num].Type == missionSetting.Assistant.Type)
					{
						View.LastScrollValue = Vector2.up * num;
						View.ShopItemsScrollScontroller.Controller.SmoothGoto(View.LastScrollValue);
						return true;
					}
				}
			}
			return false;
		}
	}
}
