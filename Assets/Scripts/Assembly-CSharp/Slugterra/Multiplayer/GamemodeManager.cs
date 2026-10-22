using Slugterra.Multiplayer.Utils;
using UnityEngine;

namespace Slugterra.Multiplayer
{
	public class GamemodeManager : PhotonSingleton<GamemodeManager>
	{
		private GamemodeBase _currentGamemode;

		public static GamemodeBase CurrentGamemode
		{
			get
			{
				if (PhotonSingleton<GamemodeManager>.Instance != null)
				{
					return PhotonSingleton<GamemodeManager>.Instance._currentGamemode;
				}
				return null;
			}
		}

		protected override void OnCreate()
		{
			_currentGamemode = Object.FindObjectOfType<GamemodeBase>();
			_currentGamemode.OnSetup();
		}

		private void Start()
		{
			_currentGamemode.SpawnPlayer();
		}

		public override void OnJoinedRoom()
		{
		}
	}
}
