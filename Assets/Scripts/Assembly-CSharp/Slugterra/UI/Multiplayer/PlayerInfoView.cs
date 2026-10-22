using AppsMinistry.UI;
using Slugterra.Domain.App.Settings;
using Slugterra.Multiplayer;
using Slugterra.Multiplayer.Lobby.Match;
using Slugterra.UI.Multiplayer.Game;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class PlayerInfoView : UIView
	{
		[SerializeField]
		protected TextMesh _playerName;

		[SerializeField]
		protected tk2dSprite _playerIcon;

		[SerializeField]
		protected TextMesh _playerRank;

		[SerializeField]
		protected PlayerSlugsViewController _slugs;

		public void OnApplyModel(PlayerData model)
		{
			_playerName.text = model.Name;
			_playerRank.text = model.Rank.ToString();
			_slugs.ApplyModel(model);
		}

		protected void OnApplyModel(MissionSetting model)
		{
			_playerName.text = NetworkManager.GetPlayerName();
			_playerRank.text = "?";
			_slugs.ApplyModel(model);
		}
	}
}
