using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.Plugins;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class WinView : UIView
	{
		[Serializable]
		private struct ChangePlatformObjects
		{
			public GameObject _objectTVOS;

			public GameObject _objectDefault;

			public ChangePlatformObjects(GameObject objectTVOS, GameObject objectDefault)
			{
				_objectTVOS = objectTVOS;
				_objectDefault = objectDefault;
			}

			public void SetAcriveObjects()
			{
				bool flag = false;
				flag = false;
				if (_objectTVOS != null)
				{
					_objectTVOS.SetActive(flag);
				}
				if (_objectDefault != null)
				{
					_objectDefault.SetActive(!flag);
				}
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSharePhotoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShareVideoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnHelpEvent__BackingField;

		private GameController _gameController;

		[SerializeField]
		private GameObject _saleInStore;

		private GameSetting _settings;

		[SerializeField]
		private GameObject _btnShareVideo;

		[SerializeField]
		public GameObject _btnFbShare;

		[SerializeField]
		private GameObject _btnFreeBonus;

		[SerializeField]
		private MissionRewards _commonRewards;

		[SerializeField]
		private MissionRewards _multiplierRewards;

		[SerializeField]
		private MissionRewards _videoRewards;

		[SerializeField]
		private StatisticsController _statisticController;

		private PopupConfirmView _popupConfirm;

		private bool doubleCheckForReplayKit;

		[SerializeField]
		private ChangePlatformObjects _btns;

		public event Action OnSharePhotoEvent
		{
			add
			{
				Action action = OnSharePhotoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSharePhotoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSharePhotoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSharePhotoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnShareVideoEvent
		{
			add
			{
				Action action = OnShareVideoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareVideoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnShareVideoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareVideoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNextEvent
		{
			add
			{
				Action action = OnNextEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNextEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnHelpEvent
		{
			add
			{
				Action action = OnHelpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHelpEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnHelpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHelpEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			_btns.SetAcriveObjects();
		}

		private void OnEnable()
		{
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			if (_settings == null)
			{
				_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			}
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (_gameController != null && _gameController.Model.GameWon)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.LevelComplete();
			}
			if (MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier == 2)
			{
				_multiplierRewards._parent.SetActive(true);
				_btnFreeBonus.SetActive(false);
				float objectivesCoinsReward = _statisticController.View.GetObjectivesCoinsReward(MonoSingleton<AppController>.Instance.GetCurrentGameLevel());
				_multiplierRewards._coins.text = "+" + (int)objectivesCoinsReward/*cast due to constrained. prefix*/;
				float objectivesGemsReward = _statisticController.View.GetObjectivesGemsReward(MonoSingleton<AppController>.Instance.GetCurrentGameLevel());
				_multiplierRewards._gems.text = "+" + (int)objectivesGemsReward/*cast due to constrained. prefix*/;
			}
			else
			{
				_multiplierRewards._parent.SetActive(false);
				if (DonDestroyMonoSingleton<CheckInternetService>.Instance.IsInternetAvailable && DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
				{
					_btnFreeBonus.SetActive(true);
				}
				else
				{
					_btnFreeBonus.SetActive(false);
				}
			}
			StartCoroutine(UpdateViews());
		}

		private void OnBtnSharePhotoClick()
		{
			if (OnSharePhotoEvent__BackingField != null)
			{
				OnSharePhotoEvent__BackingField();
			}
		}

		private IEnumerator UpdateViews()
		{
			yield return StartCoroutine(CollectionUtils.WaitForRealSeconds(0.5f));
			UnityEngine.Debug.Log("UpdateViews");
		}

		private void OnBtnShareVideoClick()
		{
			if (OnShareVideoEvent__BackingField != null)
			{
				OnShareVideoEvent__BackingField();
			}
		}

		private void OnBtnNextClick()
		{
			if (OnNextEvent__BackingField != null)
			{
				OnNextEvent__BackingField();
			}
		}

		private void OnBtnHelpClick()
		{
			if (OnHelpEvent__BackingField != null)
			{
				OnHelpEvent__BackingField();
			}
		}

		private void OnBtnFreeBonus()
		{
			_btnFreeBonus.SetActive(false);
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = () =>
				{
					if (_multiplierRewards != null && _multiplierRewards._parent != null)
					{
						_multiplierRewards._parent.SetActive(false);
					}
					if (_videoRewards != null && _videoRewards._parent != null)
					{
						_videoRewards._parent.SetActive(true);
					}
					int num = 200;
					int num2 = 3;
					if (_videoRewards != null && _videoRewards._coins != null)
					{
						_videoRewards._coins.text = "+" + num;
					}
					if (_videoRewards != null && _videoRewards._gems != null)
					{
						_videoRewards._gems.text = "+" + num2;
					}
					MonoSingleton<AppController>.Instance.Model.Wallet.Gold += num;
					MonoSingleton<AppController>.Instance.Model.Wallet.Gems += num2;
				};
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = null;
				if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
				{
					DonDestroyMonoSingleton<AnalyticsManager>.Instance.BonusForWin();
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(string.Empty);
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

		public void SetButtonsState(bool isShareVideoEnable)
		{
			_btnFbShare.SetActive(!isShareVideoEnable);
			_btnShareVideo.SetActive(isShareVideoEnable);
		}
	}
}
