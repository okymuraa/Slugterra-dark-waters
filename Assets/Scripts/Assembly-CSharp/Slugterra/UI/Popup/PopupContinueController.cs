using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Plugins;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Shops;
using Slugterra.Game;
using Slugterra.UI.Game;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupContinueController : UIController<PopupContinueView>, IObserver
	{
		private GameController _gameController;

		private bool _adsStart;

		protected override void _OnEnable()
		{
			base._OnEnable();
			View.PaymentButton.OnBuyEvent += OnButtonBuyClick;
			View.PaymentButtonOne.OnBuyEvent += OnButtonBuyClick;
			PopupContinueView view = View;
			view.OnCountdownComplete = (Action)Delegate.Combine(view.OnCountdownComplete, new Action(EndGame));
			DonDestroyMonoSingleton<PopupManager>.Instance.OnPopupClose += CloseShopPopupChecker;
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			_gameController.Model.Eli.AddObserver(this);
			PopupContinueView view2 = View;
			view2.onClosedEvent = (Action)Delegate.Combine(view2.onClosedEvent, new Action(EndGame));
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			View.PaymentButton.OnBuyEvent -= OnButtonBuyClick;
			View.PaymentButtonOne.OnBuyEvent -= OnButtonBuyClick;
			PopupContinueView view = View;
			view.OnCountdownComplete = (Action)Delegate.Remove(view.OnCountdownComplete, new Action(EndGame));
			if (DonDestroyMonoSingleton<PopupManager>.IsAlive)
			{
				DonDestroyMonoSingleton<PopupManager>.Instance.OnPopupClose -= CloseShopPopupChecker;
			}
			PopupContinueView view2 = View;
			view2.onClosedEvent = (Action)Delegate.Remove(view2.onClosedEvent, new Action(EndGame));
		}

		private void CloseShopPopupChecker(PopupView obj)
		{
			if (obj is ShopIAPView)
			{
				View.ResetCountdown();
			}
		}

		public void ShowPopup()
		{
			if (!gameObject.activeSelf)
			{
				LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
				if (!GameFactory.CanRespawn(_gameController.Model.RespawnCount))
				{
					EndGame();
					return;
				}
				View.gameObject.SetActive(true);
				View.OneButtonStatus = _gameController.Model.RespawnCount >= 3;
				View.PaymentButton.SetSate(PaymentButtonState.Buy);
				View.PaymentButton.View.SetPrice(GameFactory.GetRespawnCurrency(_gameController.Model.RespawnCount), GameFactory.GetRespawnCost(_gameController.Model.RespawnCount).ToString());
				View.SetActiveAdssButton(DonDestroyMonoSingleton<AdsManager>.Instance.CurrentAdsShowCounter < DonDestroyMonoSingleton<AdsManager>.Instance.RewardAdsLimitPerTimeInterval && DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable());
				View.StartCountdown();
			}
		}

		private void OnAdsButtonPressed()
		{
			PopupConfirmView _popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = null;
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = () =>
			{
				View.ResetCountdown();
			};
			View.NeedCountdown = true;
			_adsStart = false;
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
				if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
				{
					DonDestroyMonoSingleton<AnalyticsManager>.Instance.RespawnOnLevelForAds(_gameController.Model.RespawnCount);
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(string.Empty);
					ContinueGame();
				}
				else
				{
					_popupConfirm.Show("alert_ads_not_available", () =>
					{
					}, null, false);
				}
			}, (string errorMsg) =>
			{
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		private void OnButtonBuyClick()
		{
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.RespawnOnLevel(_gameController.Model.RespawnCount);
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			if (shopModel.CanBuySimple(GameFactory.GetRespawnCost(_gameController.Model.RespawnCount), GameFactory.GetRespawnCurrency(_gameController.Model.RespawnCount)) && shopModel.BuySimple(GameFactory.GetRespawnCost(_gameController.Model.RespawnCount), GameFactory.GetRespawnCurrency(_gameController.Model.RespawnCount)))
			{
				ContinueGame();
			}
			else
			{
				View.PauseCountdown(true);
			}
		}

		private void EndGame()
		{
			Debug.Log("end game");
			gameObject.SetActive(false);
			if (View.NeedCountdown && !_adsStart)
			{
				_gameController.Model.GameFailed = true;
			}
			View.NeedCountdown = true;
		}

		private void ContinueGame()
		{
			Debug.Log("continue game");
			_gameController.Model.GameFailed = false;
			_gameController.RespawnPlayer();
			gameObject.SetActive(false);
		}

		public void OnObjectChanged(object observable)
		{
			Humanoid humanoid = observable as Humanoid;
			if (humanoid != null && humanoid.IsDead())
			{
				if (!SurvivalController.IsSurvival(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission) && !_gameController.IsMultiplayer)
				{
					ShowPopup();
				}
				else
				{
					EndGame();
				}
			}
		}
	}
}
