using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupDailyBonusController : UIController<PopupDailyBonusView>
	{
		public const int MaxDay = 5;

		public static readonly float[] RewardByDay = new float[5] { 100f, 200f, 300f, 400f, 500f };

		protected override void OnStart()
		{
			if (!TryGetDailyBonus())
			{
				View.gameObject.SetActive(false);
			}
		}

		protected override void _OnEnable()
		{
			View.OnCloseEvent += OnClose;
			View.OnBtnDoubleRewardEvent += OnDoubleReward;
		}

		protected override void _OnDisable()
		{
			View.OnCloseEvent -= OnClose;
			View.OnBtnDoubleRewardEvent -= OnDoubleReward;
		}

		private void OnClose()
		{
			int num = PlayerPrefs.GetInt("CurrentBonusDay", -1);
			if (num >= 0)
			{
				DailyBonusRewardController dailyBonusControllerByDay = View.GetDailyBonusControllerByDay(num);
				Slugterra.Domain.App.Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
				wallet.Gold += dailyBonusControllerByDay.View.DayRewardCoins;
				wallet.Gems += dailyBonusControllerByDay.View.DayRewardGems;
			}
			View.gameObject.SetActive(false);
		}

		private void OnDoubleReward()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = () =>
				{
					int num = PlayerPrefs.GetInt("CurrentBonusDay", -1);
					if (num >= 0)
					{
						DailyBonusRewardController dailyBonusControllerByDay = View.GetDailyBonusControllerByDay(num);
						Slugterra.Domain.App.Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
						wallet.Gold += 2 * dailyBonusControllerByDay.View.DayRewardCoins;
						wallet.Gems += dailyBonusControllerByDay.View.DayRewardGems;
					}
					View.gameObject.SetActive(false);
				};
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = null;
				if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(string.Empty);
				}
				else
				{
					Debug.LogWarning("PopupDailyBonusController::OnDoubleReward IncentivizeAds not available");
					OnClose();
				}
			}, (string errorMsg) =>
			{
				Debug.LogWarning("PopupDailyBonusController::OnDoubleReward Internet not available");
				OnClose();
			});
		}

		private void OnApplicationPause(bool pause)
		{
		}

		private bool TryGetDailyBonus()
		{
			int bonusDay = GetBonusDay();
			if (bonusDay >= 0)
			{
				View.gameObject.SetActive(true);
				View.ApplyModel(bonusDay);
				return true;
			}
			return false;
		}

		private int GetBonusDay()
		{
			int num = -1;
			DateTime today = DateTime.Today;
			if (PlayerPrefsSystem.HasKey("NextDailyBonus"))
			{
				DateTime result;
				if (DateTime.TryParse(PlayerPrefs.GetString("NextDailyBonus"), out result) && result.Ticks <= today.Ticks)
				{
					num = (((today - result).Days < 1) ? PlayerPrefs.GetInt("DailyBonusDay", 0) : 0);
					PlayerPrefs.SetInt("CurrentBonusDay", num);
					PlayerPrefs.SetString("NextDailyBonus", today.AddDays(1.0).ToString());
					PlayerPrefs.SetInt("DailyBonusDay", (num + 1) % 5);
				}
			}
			else
			{
				PlayerPrefs.SetString("NextDailyBonus", today.AddDays(1.0).ToString());
				PlayerPrefs.SetInt("DailyBonusDay", 0);
			}
			return num;
		}
	}
}
