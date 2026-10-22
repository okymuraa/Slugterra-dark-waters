using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.Core.Input;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using Slugterra.UI.Popup;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class PauseController : UIController<PauseView>
	{
		private PopupConfirmView _popup;

		private GameController _gameController;

		public Action<List<ConsumableItemController>> listAlreadyFilled;

		[SerializeField]
		private tk2dUIScrollableArea _scrollableConsumable;

		[SerializeField]
		private GameObject _scrollableContent;

		[SerializeField]
		private GameObject _consumableItemPrefab;

		private Dictionary<string, ConsumableItemController> _consumableItems = new Dictionary<string, ConsumableItemController>();

		private List<ConsumableItemController> _consumablesItemUI = new List<ConsumableItemController>();

		public List<ConsumableItemController> ConsumablesItemUI
		{
			get
			{
				return _consumablesItemUI;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnStart()
		{
			_popup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
		}

		protected override void _OnEnable()
		{
			View.OnMainMenuEvent += OnMainMenu;
			View.OnMapEvent += OnMap;
			View.OnRestartEvent += OnRestart;
			View.OnSurvivalEvent += OnSurvival;
			View.OnHelpEvent += OnHelp;
			View.OnBtnGameServiceEvent += OnGameService;
			OnConsumables();
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		}

		protected override void _OnDisable()
		{
			View.OnMainMenuEvent -= OnMainMenu;
			View.OnMapEvent -= OnMap;
			View.OnRestartEvent -= OnRestart;
			View.OnSurvivalEvent -= OnSurvival;
			View.OnHelpEvent -= OnHelp;
			View.OnBtnGameServiceEvent -= OnGameService;
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			}
		}

		private void OnConsumables()
		{
			for (ConsumableType consumableType = ConsumableType.First; consumableType <= ConsumableType.SlugFood; consumableType++)
			{
				ConsumableItem consumable = MonoSingleton<AppController>.Instance.Model.ShopModel.GetConsumable(consumableType);
				if (_consumableItems.ContainsKey(consumableType.ToString()))
				{
					Debug.LogWarning("already have key: " + consumableType);
					continue;
				}
				ConsumableItemController consumableItemController = CreateNewListConsumableItem(new Vector3(3.5f + (float)consumableType.GetHashCode() * 2.88f, 0f, 0f));
				if (consumableItemController == null)
				{
					return;
				}
				_consumableItems.Add(consumableType.ToString(), consumableItemController);
				_consumablesItemUI.Add(consumableItemController);
				consumableItemController.ShowConsumable(consumableType);
			}
			if (listAlreadyFilled != null)
			{
				listAlreadyFilled(_consumablesItemUI);
			}
		}

		protected ConsumableItemController CreateNewListConsumableItem(Vector3 pos)
		{
			if (_consumableItemPrefab == null)
			{
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(_consumableItemPrefab);
			gameObject.SetActive(true);
			gameObject.transform.parent = _scrollableContent.transform;
			gameObject.transform.localPosition = pos;
			return gameObject.GetComponent<ConsumableItemController>();
		}

		private void OnMainMenu()
		{
			_popup.Show(string.Empty, () =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.VideoAdsAvailable || DonDestroyMonoSingleton<AdsManager>.Instance.InterstitialAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowAds(string.Empty);
				}
				SceneLoader.LoadScene<MainSceneController>();
			});
		}

		private void OnMap()
		{
			_popup.Show(string.Empty, () =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.VideoAdsAvailable || DonDestroyMonoSingleton<AdsManager>.Instance.InterstitialAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowAds(string.Empty);
				}
				SceneLoader.LoadFromPrev<MapSceneController>(true);
			});
		}

		private void OnSurvival()
		{
			_popup.Show(string.Empty, () =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.VideoAdsAvailable || DonDestroyMonoSingleton<AdsManager>.Instance.InterstitialAdsAvailable)
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowAds(string.Empty);
				}
				SceneLoader.LoadScene<SurvivalMapSceneController>();
			});
		}

		private void OnRestart()
		{
			_popup.Show(string.Empty, () =>
			{
				SceneLoader.ReloadScene();
			});
		}

		private void OnHelp()
		{
			PopupTvOSManualView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupTvOSManualView>();
			registeredPopup.ShowHelp();
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
		}
	}
}
