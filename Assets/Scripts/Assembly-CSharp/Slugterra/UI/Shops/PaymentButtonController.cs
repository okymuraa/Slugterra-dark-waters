using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.UI.Popup;

namespace Slugterra.UI.Shops
{
	public class PaymentButtonController : UIController<PaymentButtonView>
	{
		public const int MaxLevel = 5;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBuyEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUpgradeEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRecoverEvent__BackingField;

		private PopupConfirmView _popupConfirm;

		private PaymentButtonState _state = PaymentButtonState.Buy;

		public PaymentButtonState State
		{
			get
			{
				return _state;
			}
		}

		public event Action OnBuyEvent
		{
			add
			{
				Action action = OnBuyEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBuyEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnUpgradeEvent
		{
			add
			{
				Action action = OnUpgradeEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUpgradeEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnUpgradeEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUpgradeEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRecoverEvent
		{
			add
			{
				Action action = OnRecoverEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRecoverEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRecoverEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRecoverEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			View.ApplyModel(_state);
		}

		protected override void _OnEnable()
		{
			View.OnButtonEvent += OnButton;
		}

		protected override void _OnDisable()
		{
			View.OnButtonEvent -= OnButton;
		}

		private void OnButton()
		{
			if (_state == PaymentButtonState.Buy)
			{
				if (OnBuyEvent__BackingField != null)
				{
					OnBuyEvent__BackingField();
				}
			}
			else if (_state == PaymentButtonState.Recover)
			{
				if (OnRecoverEvent__BackingField != null)
				{
					OnRecoverEvent__BackingField();
				}
			}
			else if (_state >= PaymentButtonState.LevelDefault && _state < PaymentButtonState.Level5)
			{
				if (OnUpgradeEvent__BackingField != null)
				{
					OnUpgradeEvent__BackingField();
				}
			}
			else if (_state == PaymentButtonState.RecoverByAd)
			{
				ShowRecoverAd();
			}
		}

		private void ShowRecoverAd()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = () =>
				{
					if (OnRecoverEvent__BackingField != null)
					{
						OnRecoverEvent__BackingField();
					}
				};
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = null;
				if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
				{
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

		public void SetSate(PaymentButtonState state)
		{
			if (state != _state)
			{
				View.ApplyModel(state);
				_state = state;
			}
		}
	}
}
