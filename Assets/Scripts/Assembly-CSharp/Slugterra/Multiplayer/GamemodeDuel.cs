using Slugterra.Multiplayer.Events;
using Slugterra.Multiplayer.Player;
using Slugterra.Multiplayer.Utils;
using UnityEngine;

namespace Slugterra.Multiplayer
{
	public class GamemodeDuel : GamemodeBase
	{
		public const float StartRoundDelay = 3f;

		public const float TotalRoundTime = 900f;

		[SerializeField]
		private Transform[] _spawnPoints;

		private float _timeLeft;

		public float TimeLeft
		{
			get
			{
				return _timeLeft;
			}
		}

		private void Awake()
		{
			NetworkEventsDispatcher.OnPlayerReadyEvent += OnPlayerReady;
			NetworkEventsDispatcher.OnDuelStartEvent += DuelStart;
		}

		public override void OnSetup()
		{
		}

		public override bool IsRoundFinished()
		{
			return _timeLeft <= 0f;
		}

		private void OnPlayerReady(NetworkPlayerRoot player)
		{
			if (PhotonNetwork.playerList.Length == 2 && PhotonNetwork.isMasterClient)
			{
				PhotonSingleton<NetworkEventsDispatcher>.Instance.RaiseDuelStart(PhotonNetwork.time + 3.0);
			}
		}

		private void DuelStart(double startTime)
		{
			SetRoundStartTime((float)startTime);
		}

		private int FindAvailableSpawnPoint()
		{
			int num = -1;
			PhotonPlayer[] otherPlayers = PhotonNetwork.otherPlayers;
			if (otherPlayers != null && otherPlayers.Length > 0)
			{
				for (int i = 0; i < _spawnPoints.Length; i++)
				{
					bool flag = true;
					for (int j = 0; j < otherPlayers.Length; j++)
					{
						object value;
						if (otherPlayers[j].customProperties.TryGetValue("SID", out value) && (int)value == i)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num = i;
						break;
					}
				}
			}
			else
			{
				num = 0;
			}
			num = 0;
			if (PhotonNetwork.isMasterClient)
			{
				num = 1;
			}
			return num;
		}

		public override void SpawnPlayer()
		{
			int num = FindAvailableSpawnPoint();
			if (num == -1)
			{
				Debug.LogError("No available spawn point");
				num = 0;
			}
			NetworkPlayerRoot.NetworkInstantiate(_spawnPoints[num].transform.position, _spawnPoints[num].transform.rotation);
		}

		private void LateUpdate()
		{
			float num = (float)PhotonNetwork.time - StartRoundTime;
			_timeLeft = Mathf.Max(0f, 900f - num);
		}
	}
}
