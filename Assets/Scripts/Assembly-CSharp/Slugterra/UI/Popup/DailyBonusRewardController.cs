using AppsMinistry.UI;

namespace Slugterra.UI.Popup
{
	public class DailyBonusRewardController : UIController<DailyBonusRewardView>
	{
		private DailyBonusRewardState _state;

		public DailyBonusRewardState State
		{
			get
			{
				return _state;
			}
			set
			{
				SetState(value);
			}
		}

		private void SetState(DailyBonusRewardState state)
		{
			if (state != _state)
			{
				_state = state;
				View.ApplyModel(_state);
			}
		}
	}
}
