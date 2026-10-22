using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class PopupChallengeController : UIController<PopupChallengeView>
	{
		protected override void _OnEnable()
		{
			View.OnAcceptEvent += OnAccept;
			View.OnCloseEvent += OnClose;
		}

		protected override void _OnDisable()
		{
			View.OnAcceptEvent -= OnAccept;
			View.OnCloseEvent -= OnClose;
		}

		private void OnAccept()
		{
			if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers && PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.LoadLevel((string)PhotonNetwork.room.customProperties["MP"]);
			}
		}

		private void OnReject()
		{
			bool flag = PhotonNetwork.LeaveRoom();
			Debug.Log("OnReject " + flag);
		}

		private void OnClose()
		{
			OnReject();
			gameObject.SetActive(false);
		}
	}
}
