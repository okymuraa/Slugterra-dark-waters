using AppsMinistry.Core;
using AppsMinistry.Plugins;
using Slugterra.App;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class ResultMultiplayerLoseView : ResultMultiplayerView
	{
		private GameController _gameController;

		protected override void OnEnable()
		{
			base.OnEnable();
			_btns.SetActiveObjects();
			if (_gameController == null)
			{
				_gameController = Object.FindObjectOfType<GameController>();
			}
			if (_gameController != null && _gameController.Model.GameFailed)
			{
				_statisticController.View.GetTotalRewards(MonoSingleton<AppController>.Instance.GetCurrentGameLevel(), false);
				MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank--;
				SetPlayerRank();
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.MultiplayerLevelLoose(DonDestroyMonoSingleton<AnalyticsManager>.Instance.multiplayerWithBot);
			}
			_btnNext.targetObject = gameObject;
		}

		public override void SetPlayerRank()
		{
			float num = (PlayerRank = MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank);
			_playerTextRank.text = (num + 1f).ToString("####0") + " <color=#B1B1B1FF>-1</color>";
			_playerCurRank.text = num.ToString("####0");
			_playerRivalCurRank.text = (RivalRank + 1f).ToString("####0");
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_btns.SetDisableObjects();
		}
	}
}
