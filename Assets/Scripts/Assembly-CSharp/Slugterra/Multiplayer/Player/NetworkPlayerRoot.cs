using System.Diagnostics;
using Newtonsoft.Json;
using Photon;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.NetPlayer;
using Slugterra.Multiplayer.Events;
using Slugterra.Multiplayer.Utils;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Multiplayer.Player
{
	public sealed class NetworkPlayerRoot : PunBehaviour, IPunObservable
	{
		public const string PrefabName = "Player";

		[SerializeField]
		private NetPlayerController _player;

		private Humanoid _model;

		public NetPlayerController Player
		{
			get
			{
				return _player;
			}
		}

		public bool IsLocalPlayer
		{
			get
			{
				return photonView.isMine;
			}
		}

		public Humanoid Model
		{
			get
			{
				return _model;
			}
		}

		public override void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			if (!photonView.isMine)
			{
				string value = (string)photonView.instantiationData[0];
				_model = JsonConvert.DeserializeObject<Humanoid>(value);
			}
			else
			{
				_model = GetModel();
			}
			_player.NetworkRoot = this;
			_player.gameObject.SetActive(true);
			PlayerReady();
		}

		private void PlayerReady()
		{
			if (photonView.isMine)
			{
				PhotonSingleton<NetworkEventsDispatcher>.Instance.RaisePlayerReady(this);
			}
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				float obj = _model.Health;
				stream.Serialize(ref obj);
				return;
			}
			float obj2 = 0f;
			stream.Serialize(ref obj2);
			if (_model != null && obj2 != _model.Health)
			{
				_model.Health = obj2;
				if (_model.IsDead())
				{
					Player.OnDead();
				}
			}
		}

		[Conditional("UNITY_STANDALONE")]
		[Conditional("UNITY_EDITOR")]
		private void OnGUI()
		{
			if (_model != null)
			{
				PhotonPlayer owner = photonView.owner;
				GUI.color = Color.white;
				GUI.Label(new Rect(20f, 400f + (float)owner.ID * 30f, 350f, 30f), string.Format("{0} [{1}] [{2}]", owner.name, _model.Health, Player.transform.position));
				GUI.Label(new Rect(20f, 350f, 350f, 30f), string.Format("{0} Ping = {1}", (!PhotonNetwork.isMasterClient) ? "Client" : "Server", PhotonNetwork.GetPing()));
			}
		}

		private static Humanoid GetModel()
		{
			GameController gameController = Object.FindObjectOfType<GameController>();
			return gameController.Model.Eli;
		}

		private static object[] GetNetworkInstantiationData()
		{
			return new object[1] { JsonConvert.SerializeObject(GetModel()) };
		}

		public static GameObject NetworkInstantiate(Vector3 position, Quaternion rotation)
		{
			return PhotonNetwork.Instantiate("Player", position, rotation, 0, GetNetworkInstantiationData());
		}
	}
}
