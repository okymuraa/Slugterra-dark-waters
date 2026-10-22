using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupDailyBonusView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnDoubleRewardEvent__BackingField;

		[SerializeField]
		private DailyBonusRewardController[] _rewards;

		[SerializeField]
		private GameObject doubleRewardsForAds;

		[SerializeField]
		private GameObject getMyBonusBtn;

		public event Action OnCloseEvent
		{
			add
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnDoubleRewardEvent
		{
			add
			{
				Action action = OnBtnDoubleRewardEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnDoubleRewardEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnDoubleRewardEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnDoubleRewardEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		private void OnBtnCloseClick()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
		}

		private void OnBtnDoubleReward()
		{
			if (OnBtnDoubleRewardEvent__BackingField != null)
			{
				OnBtnDoubleRewardEvent__BackingField();
			}
		}

		public DailyBonusRewardController GetDailyBonusControllerByDay(int day)
		{
			if (day >= _rewards.Length)
			{
				UnityEngine.Debug.LogError("CARAMBAAA PopupDailyBonusView");
				return _rewards[_rewards.Length - 1];
			}
			return _rewards[day];
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			doubleRewardsForAds.SetActive(false);
			getMyBonusBtn.SetActive(true);
			int? num = model as int?;
			if (!num.HasValue)
			{
				return;
			}
			if (num.Value < 4 && DonDestroyMonoSingleton<CheckInternetService>.Instance.IsInternetAvailable && DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
			{
				doubleRewardsForAds.SetActive(true);
				getMyBonusBtn.SetActive(false);
			}
			for (int i = 0; i < 5; i++)
			{
				DailyBonusRewardController dailyBonusRewardController = _rewards[i];
				if (i < num.Value)
				{
					dailyBonusRewardController.State = DailyBonusRewardState.Checked;
					dailyBonusRewardController.View.RewardTitle.SetActive(false);
				}
				else if (i == num.Value)
				{
					dailyBonusRewardController.State = DailyBonusRewardState.Active;
					dailyBonusRewardController.View.RewardTitle.SetActive(true);
				}
				else
				{
					dailyBonusRewardController.State = DailyBonusRewardState.Default;
				}
			}
		}

		public override void Close()
		{
			OnBtnCloseClick();
		}
	}
}
