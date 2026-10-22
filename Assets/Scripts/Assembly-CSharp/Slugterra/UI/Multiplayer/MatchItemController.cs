using AppsMinistry.Core;
using AppsMinistry.UI;
using ExitGames.Client.Photon.LoadBalancing;
using Slugterra.Multiplayer;
using Slugterra.Multiplayer.Lobby.Match;
using Slugterra.Multiplayer.Utils;

namespace Slugterra.UI.Multiplayer
{
	public class MatchItemController : UIController<MatchItemView>
	{
		private ExitGames.Client.Photon.LoadBalancing.RoomInfo _model;

		private PopupChallengeView _popupConfirm;

		protected override void _OnEnable()
		{
			View.OnPlayEvent += OnPlay;
			View.OnSendInviteEvent += OnSendInvite;
		}

		protected override void _OnDisable()
		{
			View.OnPlayEvent -= OnPlay;
			View.OnSendInviteEvent -= OnSendInvite;
		}

		protected override void OnStart()
		{
			base.OnStart();
			_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupChallengeView>();
		}

		public void ApplyModel(object model)
		{
			if (model is ExitGames.Client.Photon.LoadBalancing.RoomInfo)
			{
				_model = model as ExitGames.Client.Photon.LoadBalancing.RoomInfo;
			}
			View.ApplyModel(model);
		}

		private void OnPlay()
		{
			ExitGames.Client.Photon.LoadBalancing.RoomInfo model = _model;
			PlayerData playerData = PlayerData.FromJson((string)model.CustomProperties["OD"]);
			PhotonSingleton<Matchmaking>.Instance.StartMatchmaking(MatchmakingType.OneVsOne, model);
		}

		private void OnSendInvite()
		{
		}
	}
}
