using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.UI.Shops.PaymentStates;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class PaymentButtonView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnButtonEvent__BackingField;

		[SerializeField]
		private AbstractPaymentState _buyState;

		[SerializeField]
		private AbstractPaymentState _upgradeState;

		[SerializeField]
		private AbstractPaymentState _recoverState;

		[SerializeField]
		private AbstractPaymentState _currentState;

		[SerializeField]
		private GameObject[] _levels;

		public event Action OnButtonEvent
		{
			add
			{
				Action action = OnButtonEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnButtonEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnButtonEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnButtonClick()
		{
			if (OnButtonEvent__BackingField != null)
			{
				OnButtonEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			if (model is PaymentButtonState)
			{
				SetState((PaymentButtonState)model);
			}
		}

		private void SetState(PaymentButtonState state)
		{
			if (_buyState != null)
			{
				_buyState.Hide();
			}
			if (_upgradeState != null)
			{
				_upgradeState.Hide();
			}
			if (_recoverState != null)
			{
				_recoverState.Hide();
			}
			switch (state)
			{
			case PaymentButtonState.Recover:
				SetRecoverState(false);
				return;
			case PaymentButtonState.RecoverByAd:
				SetRecoverState(true);
				return;
			case PaymentButtonState.Buy:
				SetBuyState();
				return;
			case PaymentButtonState.UpgradeLocked:
				SetLockedUpgradeState();
				return;
			case PaymentButtonState.LevelDefault:
			case PaymentButtonState.Level1:
			case PaymentButtonState.Level2:
			case PaymentButtonState.Level3:
			case PaymentButtonState.Level4:
			case PaymentButtonState.Level5:
			{
				int upgradeState = (int)(5 - (8 - state));
				SetUpgradeState(upgradeState);
				return;
			}
			}
			if (state == PaymentButtonState.Hide)
			{
				if (_buyState != null)
				{
					_buyState.Hide();
				}
				if (_upgradeState != null)
				{
					_upgradeState.Hide();
				}
				if (_recoverState != null)
				{
					_recoverState.Hide();
				}
				if (_currentState != null)
				{
					_currentState.Hide();
				}
			}
		}

		private void SetLevel(int level)
		{
			level = Mathf.Clamp(level, 0, 5);
			for (int i = 0; i < _levels.Length; i++)
			{
				_levels[i].SetActive(false);
				if (i == level)
				{
					_levels[i].SetActive(true);
					break;
				}
			}
		}

		public void SetPrice(PaymentButtonCurrency currency, string price)
		{
			if (_currentState != null)
			{
				_currentState.SetPrice(currency, price);
			}
		}

		public void SetLockedText(string text)
		{
			if (_currentState is UpgradePaymentState)
			{
				(_currentState as UpgradePaymentState).SetLockedText(text);
			}
		}

		private void SetBuyState()
		{
			SetCurrentState(_buyState);
			SetLevel(0);
		}

		private void SetRecoverState(bool byAd)
		{
			if (_recoverState == null)
			{
				return;
			}
			SetCurrentState(_recoverState);
			RecoverPaymentState state = _currentState as RecoverPaymentState;
			if (byAd)
			{
				DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
				{
					state.SetAdView();
				}, (string errorMsg) =>
				{
					state.SetDefaultView();
				});
			}
			SetLevel(0);
		}

		private void SetLockedUpgradeState()
		{
			SetCurrentState(_upgradeState);
			UpgradePaymentState upgradePaymentState = _currentState as UpgradePaymentState;
			upgradePaymentState.SetLocked();
		}

		private void SetUpgradeState(int level)
		{
			SetCurrentState(_upgradeState);
			UpgradePaymentState upgradePaymentState = _currentState as UpgradePaymentState;
			if (level > 5)
			{
				upgradePaymentState.SetUpgrade(true);
				SetLevel(5);
			}
			else if (upgradePaymentState != null)
			{
				upgradePaymentState.SetUpgrade(false);
				SetLevel(level);
			}
		}

		private void SetCurrentState(AbstractPaymentState newState)
		{
			if (!(newState == null))
			{
				if (_currentState != null)
				{
					_currentState.Hide();
				}
				_currentState = newState;
				_currentState.Show();
			}
		}
	}
}
