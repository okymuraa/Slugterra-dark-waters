using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.Core.Input;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using Slugterra.UI.Popup;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class ArenaController : UIController<ArenaView>
	{
		private PopupConfirmView _popup;

		private GameController _gameController;

		public Action<List<ConsumableItemController>> listAlreadyFilled;

		[SerializeField]
		private tk2dUIScrollableArea _scrollableArea;

		[SerializeField]
		private GameObject _scrollableContent;

		[SerializeField]
		private GameObject _arenaRivalsItemPrefab;

		[SerializeField]
		private GameObject _backpackPrefab;

		[SerializeField]
		private GameObject _bntNextOn;

		[SerializeField]
		private GameObject _bntNextOff;

		private Dictionary<string, ArenaItemController> _rivalItems = new Dictionary<string, ArenaItemController>();

		private List<ArenaItemController> _rivalItemUI = new List<ArenaItemController>();

		private List<ConsumableItemController> _consumablesItemUI = new List<ConsumableItemController>();

		public GameObject Tutorial;

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
			View.OnBackEvent += OnBack;
			View.OnBtnGameServiceEvent += OnGameService;
			if (_rivalItems.Count > 0)
			{
				foreach (KeyValuePair<string, ArenaItemController> rivalItem in _rivalItems)
				{
					ArenaItemView view = rivalItem.Value.View;
					view.OnUpdateRivals = (Action<string>)Delegate.Combine(view.OnUpdateRivals, new Action<string>(UpdateRivalsByName));
				}
			}
			DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			duelSetting.SetDefaultFoe();
			if (duelSetting.Foe.Type == EnemyType.None.ToString() || duelSetting.Foe == null)
			{
				_bntNextOn.SetActive(false);
				_bntNextOff.SetActive(true);
			}
			else
			{
				_bntNextOn.SetActive(true);
				_bntNextOff.SetActive(false);
			}
			OnRivals();
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		}

		protected override void _OnDisable()
		{
			View.OnMainMenuEvent -= OnMainMenu;
			View.OnMapEvent -= OnMap;
			View.OnRestartEvent -= OnRestart;
			View.OnBackEvent -= OnBack;
			View.OnBtnGameServiceEvent -= OnGameService;
			foreach (KeyValuePair<string, ArenaItemController> rivalItem in _rivalItems)
			{
				ArenaItemView view = rivalItem.Value.View;
				view.OnUpdateRivals = (Action<string>)Delegate.Combine(view.OnUpdateRivals, new Action<string>(UpdateRivalsByName));
			}
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			}
		}

		private void OnBack()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			SceneLoader.LoadPrevScene();
		}

		private void OnRivals()
		{
			List<FoeItem> foes = MonoSingleton<AppController>.Instance.Model.ShopModel.Foes;
			int num = 0;
			foreach (FoeItem item in foes)
			{
				if (_rivalItems.ContainsKey(item.Type.ToString()))
				{
					Debug.LogWarning("already have key: " + item.Type.ToString());
				}
				else
				{
					ArenaItemController arenaItemController = CreateNewListRivalsItem(new Vector3((float)num * 2.88f, 0f, 0f));
					if (arenaItemController == null)
					{
						return;
					}
					_rivalItems.Add(item.Type.ToString(), arenaItemController);
					EnemyType enemyTypeByName = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetEnemyTypeByName(item.Type);
					arenaItemController.ShowEnemy(enemyTypeByName, item);
					ArenaItemView view = arenaItemController.View;
					view.OnUpdateRivals = (Action<string>)Delegate.Combine(view.OnUpdateRivals, new Action<string>(UpdateRivalsByName));
					_rivalItemUI.Add(arenaItemController);
				}
				num++;
			}
			float contentLength = (float)(foes.Count - 1) * 2.88f;
			_scrollableArea.ContentLength = contentLength;
			if (Tutorial != null && Tutorial.activeSelf)
			{
				Tutorial.SendMessage("StartScene", _rivalItemUI);
			}
		}

		protected ArenaItemController CreateNewListRivalsItem(Vector3 pos)
		{
			if (_arenaRivalsItemPrefab == null)
			{
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(_arenaRivalsItemPrefab);
			gameObject.SetActive(true);
			gameObject.transform.parent = _scrollableContent.transform;
			gameObject.transform.localPosition = pos;
			return gameObject.GetComponent<ArenaItemController>();
		}

		private void UpdateRivals()
		{
			foreach (KeyValuePair<string, ArenaItemController> rivalItem in _rivalItems)
			{
				rivalItem.Value.View.UpdateItem();
			}
		}

		private void UpdateRivalsByName(string type)
		{
			foreach (KeyValuePair<string, ArenaItemController> rivalItem in _rivalItems)
			{
				if (rivalItem.Value.ModelValue.ToString() == type)
				{
					rivalItem.Value.View.UpdateItem();
				}
			}
			DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			if (duelSetting.Foe.Type == EnemyType.None.ToString() || duelSetting.Foe == null)
			{
				_bntNextOn.SetActive(false);
				_bntNextOff.SetActive(true);
			}
			else
			{
				_bntNextOn.SetActive(true);
				_bntNextOff.SetActive(false);
			}
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

		private void OnBtnBackpackClick()
		{
			_backpackPrefab.SetActive(true);
			gameObject.SetActive(false);
		}

		private void OnMap()
		{
			_popup.Show(string.Empty, () =>
			{
				SceneLoader.LoadFromPrev<MapSceneController>(true);
			});
		}

		private void OnRestart()
		{
			_popup.Show(string.Empty, () =>
			{
				SceneLoader.ReloadScene();
			});
		}

		private void OnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Achievements, GameServiceLeaderboardTimeScope.AllTime, null);
		}
	}
}
