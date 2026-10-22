using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class DailyBonusRewardView : UIView
	{
		[SerializeField]
		private int _dayRewardCoins;

		[SerializeField]
		private int _dayRewardGems;

		[SerializeField]
		private GameObject _rewardTitle;

		[SerializeField]
		private GameObject[] _rewardState;

		public int DayRewardCoins
		{
			get
			{
				return _dayRewardCoins;
			}
		}

		public int DayRewardGems
		{
			get
			{
				return _dayRewardCoins;
			}
		}

		public GameObject RewardTitle
		{
			get
			{
				return _rewardTitle;
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is DailyBonusRewardState)
			{
				DailyBonusRewardState? dailyBonusRewardState = model as DailyBonusRewardState?;
				if (dailyBonusRewardState.HasValue)
				{
					SetState(dailyBonusRewardState.Value);
				}
			}
		}

		private void SetState(DailyBonusRewardState state)
		{
			_rewardTitle.SetActive(false);
			switch (state)
			{
			case DailyBonusRewardState.Active:
				_rewardState[0].SetActive(false);
				_rewardState[1].SetActive(true);
				_rewardState[2].SetActive(false);
				break;
			case DailyBonusRewardState.Checked:
				_rewardTitle.SetActive(true);
				_rewardState[0].SetActive(false);
				_rewardState[1].SetActive(false);
				_rewardState[2].SetActive(true);
				break;
			default:
				_rewardState[0].SetActive(true);
				_rewardState[1].SetActive(false);
				_rewardState[2].SetActive(false);
				break;
			}
		}
	}
}
