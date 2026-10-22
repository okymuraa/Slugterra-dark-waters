using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Scene.Game;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupSpecialPacksView : PopupView
	{
		[SerializeField]
		private GameObject _contentSpecialPacks;

		[SerializeField]
		private tk2dUIScrollableArea _scrollableSpecialPacks;

		[SerializeField]
		private GameObject _specialPackItemPrefab;

		private Dictionary<string, SpecialPackItemController> _specialPacksItems = new Dictionary<string, SpecialPackItemController>();

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
		}

		public void Update()
		{
		}

		public void OnBtnCloseClick()
		{
			Hide();
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
			OnSpecialPacks();
		}

		public override void Close()
		{
			Hide();
		}

		private void OnSpecialPacks()
		{
			_scrollableSpecialPacks.gameObject.SetActive(true);
			LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission);
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			string levelSpecialPack = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelSpecialPack(levelItem.Scene.Name);
			SpecialPacksConfig specialPackById = MonoSingleton<GameConfiguration>.Instance.SpecialPacksConfiguration.GetSpecialPackById(levelSpecialPack);
			List<SpecialPacksConfig> specialPacksById = MonoSingleton<GameConfiguration>.Instance.SpecialPacksConfiguration.GetSpecialPacksById(GetCurrentLevelId());
			PopupGuideAnnouncementView popupGuideAnnouncementView = new PopupGuideAnnouncementView();
			float num = (float)(specialPacksById.Count - 1) * 7.28f;
			_scrollableSpecialPacks.ContentLength = num;
			_contentSpecialPacks.transform.localPosition = new Vector3(0f - num, 0f, 0f);
			float num2 = 0f;
			foreach (SpecialPacksConfig item in specialPacksById)
			{
				specialPackById = shopModel.GetSpecialPack(item.Id);
				SpecialPackItemController specialPackItemController;
				if (_specialPacksItems.ContainsKey(specialPackById.Id))
				{
					Debug.LogWarning("Already have Key: " + specialPackById.Id);
					specialPackItemController = _specialPacksItems[specialPackById.Id];
				}
				else
				{
					specialPackItemController = CreateNewListSpecialPacksItem(new Vector3(num2, 0f, 0f));
					_specialPacksItems.Add(specialPackById.Id, specialPackItemController);
					num2 += 7.28f;
				}
				specialPackItemController.ShowItems(specialPackById);
			}
		}

		protected int GetCurrentLevelId()
		{
			bool flag = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(QuietLawn_2_1)).Status == LevelStatus.Completed;
			bool flag2 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(DarkPeriphery_3_1)).Status == LevelStatus.Completed;
			bool flag3 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Deadweed_4_1)).Status == LevelStatus.Completed;
			bool flag4 = MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(GhoulDep_5_1)).Status == LevelStatus.Completed;
			int result;
			if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Train_6_1)).Status == LevelStatus.Unlocked || MonoSingleton<AppController>.Instance.Model.GetLevelItem(typeof(Train_6_1)).Status == LevelStatus.Completed)
			{
				result = 6;
			}
			else if (flag4)
			{
				result = 5;
			}
			else if (flag3)
			{
				result = 4;
			}
			else
			{
				result = (flag2 ? 3 : ((!flag) ? 1 : 2));
			}
			return result;
		}

		protected SpecialPackItemController CreateNewListSpecialPacksItem(Vector3 pos)
		{
			GameObject gameObject = Object.Instantiate(_specialPackItemPrefab);
			gameObject.SetActive(true);
			gameObject.transform.parent = _contentSpecialPacks.transform;
			gameObject.transform.localPosition = pos;
			return gameObject.GetComponent<SpecialPackItemController>();
		}
	}
}
