using AppsMinistry.Core;
using AppsMinistry.Plugins;
using Slugterra.App;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class ResultMultiplayerWinView : ResultMultiplayerView
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
			if (_gameController != null && _gameController.Model.GameWon)
			{
				_statisticController.View.GetTotalRewards(MonoSingleton<AppController>.Instance.GetCurrentGameLevel());
				MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank++;
				SetPlayerRank();
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.MultiplayerLevelWon(DonDestroyMonoSingleton<AnalyticsManager>.Instance.multiplayerWithBot);
			}
			_btnNext.targetObject = gameObject;
		}

		public override void SetPlayerRank()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportScore((long)MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank, "CgkIjZj4j6kZEAIQHQ");
			float num = (PlayerRank = MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank);
			_playerTextRank.text = (num - 1f).ToString("####0") + " <color=#00FF8CFF>+1</color>";
			_playerCurRank.text = num.ToString("####0");
			_playerRivalCurRank.text = (RivalRank - 1f).ToString("####0");
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_btns.SetDisableObjects();
		}
	}
}
