using System;
using System.Collections.Generic;
using ExitGames.Client.Photon.LoadBalancing;
using UnityEngine;

namespace Slugterra.Multiplayer.Lobby
{
	public class LobbyManager : MonoBehaviour
	{
		public Action<Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo>> updateRoomsListAction;

		public byte Version = 1;

		public void Connect()
		{
		}

		private void OnApplicationQuit()
		{
		}

		private void Update()
		{
		}
	}
}
