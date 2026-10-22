using Slugterra.Game.Units.Player;
using Slugterra.Multiplayer;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.UI.Multiplayer.Game
{
	public class InGameMpView : InGameView
	{
		[SerializeField]
		private TextMesh _playerName;

		[SerializeField]
		private TextMesh _enemyName;

		[SerializeField]
		private TextMesh _timer;

		private GamemodeDuel _gamemodeDuel;

		protected override void OnStart()
		{
			base.OnStart();
			_gamemodeDuel = GamemodeManager.CurrentGamemode as GamemodeDuel;
			PlayerManager.OnDuel((PlayerController player, PlayerController enemy) =>
			{
				_playerName.text = PhotonNetwork.player.name;
				_enemyName.text = enemy.NetworkRoot.photonView.owner.name;
			});
		}

		private void Update()
		{
			if (_gamemodeDuel != null)
			{
				int num = (int)_gamemodeDuel.TimeLeft;
				int num2 = num % 60;
				int num3 = num / 60;
				_timer.text = string.Format("{0:00}:{1:00}", num3, num2);
			}
		}
	}
}
