using ExitGames.Client.Photon;
using UnityEngine;

namespace Slugterra.Multiplayer
{
	public abstract class GamemodeBase : MonoBehaviour
	{
		private float _startRoundTime;

		public float StartRoundTime
		{
			get
			{
				return _startRoundTime;
			}
		}

		public abstract void OnSetup();

		public abstract bool IsRoundFinished();

		public abstract void SpawnPlayer();

		protected void SetRoundStartTime(float startTime)
		{
			_startRoundTime = startTime;
			if (PhotonNetwork.isMasterClient)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("ST", _startRoundTime);
				PhotonNetwork.room.SetCustomProperties(hashtable);
			}
		}
	}
}
