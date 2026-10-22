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
	public class LoseView : UIView
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
		private Action OnMapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMainMenuEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRestartEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRespawnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnHelpEvent__BackingField;

		[SerializeField]
		private GameObject _btnFreeBonus;

		[SerializeField]
		private GameObject _saleInStore;

		[SerializeField]
		private MissionRewards _videoRewards;

		private GameSetting _settings;

		private PopupConfirmView _popupConfirm;

		private bool doubleCheckForReplayKit;

		[SerializeField]
		private ChangePlatformObjects _btns;

		public event Action OnMapEvent
		{
			add
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMainMenuEvent
		{
			add
			{
				Action action = OnMainMenuEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMainMenuEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMainMenuEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMainMenuEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRestartEvent
		{
			add
			{
				Action action = OnRestartEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestartEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRestartEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestartEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRespawnEvent
		{
			add
			{
				Action action = OnRespawnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRespawnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRespawnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRespawnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (_settings == null)
			{
				_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			}
			if (DonDestroyMonoSingleton<CheckInternetService>.Instance.IsInternetAvailable && DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
			{
				_btnFreeBonus.SetActive(true);
			}
			else
			{
				_btnFreeBonus.SetActive(false);
			}
			StartCoroutine(UpdateViews());
		}

		private IEnumerator UpdateViews()
		{
			yield return StartCoroutine(CollectionUtils.WaitForRealSeconds(0.5f));
			UnityEngine.Debug.Log("UpdateViews");
		}

		private void OnBtnMapClick()
		{
			if (OnMapEvent__BackingField != null)
			{
				OnMapEvent__BackingField();
			}
		}

		private void OnBtnMainMenuClick()
		{
			if (OnMainMenuEvent__BackingField != null)
			{
				OnMainMenuEvent__BackingField();
			}
		}

		private void OnBtnRestartClick()
		{
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.RestartLevel();
			if (OnRestartEvent__BackingField != null)
			{
				OnRestartEvent__BackingField();
			}
		}

		private void OnBtnRespawnClick()
		{
			if (OnRespawnEvent__BackingField != null)
			{
				OnRespawnEvent__BackingField();
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
					if (_videoRewards != null && _videoRewards._parent != null)
					{
						_videoRewards._parent.SetActive(true);
					}
					int num = 100;
					int num2 = 1;
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
					DonDestroyMonoSingleton<AnalyticsManager>.Instance.BonusForLoose();
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
	}
}
