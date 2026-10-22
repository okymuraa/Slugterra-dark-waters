using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Plugins;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Photon;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.NetPlayer;
using Slugterra.Multiplayer.Utils;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Popup;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
	[Serializable]
	public class SpecialObject
	{
		public GameObject[] Object;

		public string Mesege;
	}

	public bool AutoConnect = true;

	public byte MaxPlayers = 2;

	public byte Version = 1;

	public int disconnectTime = 60;

	public int disconnectPauseTimeoutTime = 15;

	public int sessionTime = 360;

	public int sessionCurTime;

	public float disconnectCurTime = 60f;

	public float disconnectPauseTimeoutCurTime = 5f;

	public bool sessionTimeNeed = true;

	private bool dontDisconnect = true;

	private bool sessionStarted;

	public bool isMaster;

	public TextMesh textCountdown;

	public GameObject btnPause;

	public GameObject prefabWaiting;

	public tk2dSlicedSprite ProgressBar;

	private Vector2 scale;

	public GameObject prefabController;

	public GameObject[] hidePrefabs;

	private bool _admission;

	private static bool _battleBegun;

	private bool ConnectInUpdate = true;

	private bool isPaused;

	private NetPlayerController netPlayerEnemy;

	private NetPlayerController netPlayerCurrent;

	private BillyController mobPlayerEnemy;

	private EliController playerCurrent;

	[SerializeField]
	private SpawnPoints _spawnPoints;

	public GameObject _playerGO;

	public GameObject _mobGO;

	[SerializeField]
	private AutoCam _CameraThirdView;

	public SpawnPlayers _spawnPlayers;

	public SpecialObject[] SpecialObjects;

	private float m_lastInputTime;

	private Vector3 oldMousePosition = Vector3.zero;

	private bool needCheckTime;

	public static bool BattleBegun
	{
		get
		{
			return _battleBegun;
		}
	}

	public object FotonView { get; private set; }

	public virtual void Start()
	{
		PlayerPrefs.DeleteKey("PUNCloudBestRegion");
		PhotonNetwork.autoJoinLobby = false;
		_battleBegun = false;
		disconnectCurTime = disconnectTime;
		sessionCurTime = sessionTime;
		_CameraThirdView = UnityEngine.Object.FindObjectOfType<AutoCam>();
		if (_CameraThirdView != null)
		{
			_CameraThirdView.GetComponent<ProtectCameraFromWallClip>().enabled = false;
		}
		if (prefabWaiting != null)
		{
			prefabWaiting.SetActive(true);
		}
		if (prefabController != null)
		{
			prefabController.SetActive(false);
		}
		textCountdown.transform.parent.gameObject.SetActive(false);
		if (hidePrefabs != null)
		{
			GameObject[] array = hidePrefabs;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(false);
			}
		}
		if ((bool)ProgressBar)
		{
			scale = ProgressBar.dimensions;
		}
		StartCoroutine(ChangeCurTime(0.1f));
	}

	private void OnEnable()
	{
		CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
		instance.onInternetConnectionLost = (Action)Delegate.Combine(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
		NetworkingPeer.OnOperationFailed += OnNetworkingPeerFailed;
	}

	private void OnDisable()
	{
		CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
		instance.onInternetConnectionLost = (Action)Delegate.Remove(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
		NetworkingPeer.OnOperationFailed -= OnNetworkingPeerFailed;
	}

	public virtual void Update()
	{
		if (PhotonNetwork.player == null || Input.anyKey || oldMousePosition != Input.mousePosition || !_battleBegun)
		{
			m_lastInputTime = Time.time;
		}
		oldMousePosition = Input.mousePosition;
		if (m_lastInputTime + (float)Screen.sleepTimeout - 1f < Time.time)
		{
			QuitToMenuLoose();
		}
		if (needCheckTime)
		{
			if (Time.realtimeSinceStartup >= disconnectPauseTimeoutCurTime)
			{
				Debug.LogFormat("<b><color=green>Exit after timeOut. isPaused {0} needCheckTime {1}  realtimeSinceStartup {2}  disconnectPauseTimeoutCurTime {3} </color></b>", isPaused, needCheckTime, Time.realtimeSinceStartup, disconnectPauseTimeoutCurTime);
				QuitToMenuLoose();
			}
			needCheckTime = false;
		}
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex + "." + Application.platform.ToString() + "." + Application.version.ToString());
		}
		if ((bool)ProgressBar)
		{
			scale.x = 25f + ((float)disconnectTime - disconnectCurTime) * 350f / (float)disconnectTime;
			ProgressBar.dimensions = scale;
		}
		if (!dontDisconnect)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((int)disconnectCurTime);
			string text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
			textCountdown.text = text;
		}
		else if (sessionStarted)
		{
			TimeSpan timeSpan2 = TimeSpan.FromSeconds(sessionCurTime);
			string text2 = string.Format("{0:D2}:{1:D2}", timeSpan2.Minutes, timeSpan2.Seconds);
			textCountdown.text = text2;
		}
	}

	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = " + MaxPlayers + "}, null);");
		isMaster = true;
		bool flag = PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			MaxPlayers = MaxPlayers
		}, null);
		Debug.LogFormat("<b><color=cyan>OnPhotonRandomJoinFailed isRoomCreated {0} </color></b>", flag);
	}

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
		if (!_battleBegun)
		{
			dontDisconnect = false;
			StartCoroutine(CoroutineSpawnBot(2f));
		}
	}

	public virtual void OnNetworkingPeerFailed()
	{
		Debug.LogError("OnNetworkingPeerFailed");
		if (!_battleBegun)
		{
			dontDisconnect = false;
			StartCoroutine(CoroutineSpawnBot(0.1f));
		}
	}

	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		if (isMaster)
		{
			dontDisconnect = false;
			disconnectCurTime = disconnectTime;
			StartCoroutine(CoroutineQuitToMenu(disconnectTime));
			int num = UnityEngine.Random.Range(disconnectTime / 2, disconnectTime);
			StartCoroutine(CoroutineSpawnBot(num));
		}
		else
		{
			dontDisconnect = true;
			if (sessionTimeNeed)
			{
				StartCoroutine(CorotineEndTimeGame(sessionTime));
				StartCoroutine(ChangeCurSessionTime(1f));
				sessionCurTime = sessionTime;
				sessionStarted = true;
			}
			else
			{
				textCountdown.transform.parent.gameObject.SetActive(false);
				btnPause.SetActive(true);
				prefabWaiting.SetActive(false);
				prefabController.SetActive(true);
				if (hidePrefabs != null)
				{
					GameObject[] array = hidePrefabs;
					foreach (GameObject gameObject in array)
					{
						gameObject.SetActive(true);
					}
				}
			}
			_spawnPlayers.CreatePlayerObject();
			Admission();
			if (_CameraThirdView == null)
			{
				_CameraThirdView = UnityEngine.Object.FindObjectOfType<AutoCam>();
			}
			if (_CameraThirdView != null)
			{
				_CameraThirdView.GetComponent<SplineAnimator>().enabled = false;
				_CameraThirdView.GetComponent<ProtectCameraFromWallClip>().enabled = true;
			}
		}
		sessionCurTime = sessionTime;
		StartCoroutine(CorotineSetEnemyName(2f));
	}

	private void SetEnemyName()
	{
		NetPlayerController[] array = UnityEngine.Object.FindObjectsOfType<NetPlayerController>();
		NetPlayerController[] array2 = array;
		foreach (NetPlayerController netPlayerController in array2)
		{
			PhotonView component = netPlayerController.GetComponent<PhotonView>();
			if (component != null && !component.isMine)
			{
				netPlayerEnemy = netPlayerController;
				GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
				if (gameController != null)
				{
					gameController.View.MultiplayerWinView.RivalNickName = netPlayerController.gameObject.transform.root.name;
					gameController.View.MultiplayerLoseView.RivalNickName = netPlayerController.gameObject.transform.root.name;
					gameController.View.MultiplayerWinView.RivalRank = netPlayerController.Rank;
					gameController.View.MultiplayerLoseView.RivalRank = netPlayerController.Rank;
				}
			}
			else if (component != null && component.isMine)
			{
				netPlayerCurrent = netPlayerController;
			}
		}
	}

	public void OnPhotonPlayerDisconnected()
	{
		SetEnemyName();
		StartCoroutine(CorotinePlayerDisconnect(2f));
	}

	public void OnPhotonPlayerConnected()
	{
		dontDisconnect = true;
		StartCoroutine(CorotineSetEnemyName(2f));
		if (sessionTimeNeed)
		{
			StartCoroutine(CorotineEndTimeGame(sessionTime));
			StartCoroutine(ChangeCurSessionTime(1f));
			sessionStarted = true;
			sessionCurTime = sessionTime;
		}
		else
		{
			textCountdown.transform.parent.gameObject.SetActive(false);
			btnPause.SetActive(true);
			prefabWaiting.SetActive(false);
			prefabController.SetActive(true);
			if (hidePrefabs != null)
			{
				GameObject[] array = hidePrefabs;
				foreach (GameObject gameObject in array)
				{
					gameObject.SetActive(true);
				}
			}
		}
		_spawnPlayers.CreatePlayerObject();
		Admission();
		if (_CameraThirdView == null)
		{
			_CameraThirdView = UnityEngine.Object.FindObjectOfType<AutoCam>();
		}
		if (_CameraThirdView != null)
		{
			_CameraThirdView.GetComponent<SplineAnimator>().enabled = false;
			_CameraThirdView.GetComponent<ProtectCameraFromWallClip>().enabled = true;
		}
		DonDestroyMonoSingleton<AnalyticsManager>.Instance.multiplayerWithBot = false;
		DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackMultiplayerParameters();
	}

	private void InternetConnectionLost()
	{
		PopupConfirmView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
		if (_battleBegun)
		{
			registeredPopup.Show("alert_internet_error", null, QuitToMenuLoose, false);
		}
		else
		{
			registeredPopup.Show("alert_internet_error", null, QuitToMenu, false);
		}
		CheckInternetService instance = DonDestroyMonoSingleton<CheckInternetService>.Instance;
		instance.onInternetConnectionLost = (Action)Delegate.Remove(instance.onInternetConnectionLost, new Action(InternetConnectionLost));
	}

	private void QuitToMenuLoose()
	{
		PhotonNetwork.Disconnect();
		Debug.Log("<b><color=red>QuitToMenuLoose</color></b>");
		GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
		if (_battleBegun)
		{
			if (!gameController.Model.GameWon)
			{
				gameController.OnResultmultiplayerFail();
			}
		}
		else
		{
			SceneLoader.LoadScene<MultiplayerSceneController>(true);
		}
	}

	private void QuitToMenu()
	{
		PhotonNetwork.Disconnect();
		Debug.Log("<b><color=red>QuitToMenu</color></b>");
		SceneLoader.LoadScene<MultiplayerSceneController>(true);
	}

	private void QuitToMenuWin()
	{
		GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
		gameController.OnResultmultiplayerWin();
		PhotonNetwork.Disconnect();
	}

	private void QuitToMenuWinOrLoose()
	{
		if (netPlayerEnemy != null && netPlayerCurrent != null)
		{
			if (netPlayerCurrent.Model.Health > netPlayerEnemy.Model.Health)
			{
				QuitToMenuWin();
			}
			else
			{
				QuitToMenuLoose();
			}
		}
		else if (mobPlayerEnemy != null && playerCurrent != null)
		{
			if (playerCurrent.Model.Health > mobPlayerEnemy.Model.Health)
			{
				QuitToMenuWin();
			}
			else
			{
				QuitToMenuLoose();
			}
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		isPaused = pauseStatus;
		if (isPaused)
		{
			disconnectPauseTimeoutCurTime = Time.realtimeSinceStartup + (float)disconnectPauseTimeoutTime;
		}
		else
		{
			needCheckTime = true;
		}
		Debug.LogFormat("<b><color=magenta>OnApplicationPause isPaused {0} needCheckTime {1}  </color></b>", isPaused, needCheckTime);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		isPaused = !hasFocus;
	}

	private void Admission()
	{
		_battleBegun = true;
		MonoSingleton<AppController>.Instance.Model.Wallet.Ticket--;
		if (MonoSingleton<AppController>.Instance.Model.Wallet.Ticket == 0f)
		{
			PlayerPrefs.SetString("NextTicketsDate", DateTime.Now.AddMinutes(90.0).ToString());
		}
	}

	private IEnumerator CoroutineQuitToMenu(float time)
	{
		yield return new WaitForSeconds(time);
		if (dontDisconnect)
		{
			yield break;
		}
		PhotonNetwork.Disconnect();
		SpawnMob();
		dontDisconnect = true;
		if (sessionTimeNeed)
		{
			sessionStarted = true;
			StartCoroutine(ChangeCurSessionTime(1f));
			StartCoroutine(CorotineEndTimeGame(sessionTime));
			sessionCurTime = sessionTime;
			yield break;
		}
		textCountdown.transform.parent.gameObject.SetActive(false);
		btnPause.SetActive(true);
		prefabWaiting.SetActive(false);
		prefabController.SetActive(true);
		if (hidePrefabs != null)
		{
			GameObject[] array = hidePrefabs;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(true);
			}
		}
	}

	private IEnumerator CoroutineSpawnBot(float time)
	{
		yield return new WaitForSeconds(time);
		if (dontDisconnect)
		{
			yield break;
		}
		PhotonNetwork.Disconnect();
		SpawnMob();
		dontDisconnect = true;
		if (sessionTimeNeed)
		{
			sessionStarted = true;
			StartCoroutine(ChangeCurSessionTime(1f));
			StartCoroutine(CorotineEndTimeGame(sessionTime));
			sessionCurTime = sessionTime;
			yield break;
		}
		textCountdown.transform.parent.gameObject.SetActive(false);
		btnPause.SetActive(true);
		prefabWaiting.SetActive(false);
		prefabController.SetActive(true);
		if (hidePrefabs != null)
		{
			GameObject[] array = hidePrefabs;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(true);
			}
		}
	}

	private void SpawnMob()
	{
		if (_spawnPoints != null)
		{
			Transform nextSpawnPoint = _spawnPoints.GetNextSpawnPoint();
			Transform nextSpawnPoint2 = _spawnPoints.GetNextSpawnPoint();
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			GameObject gameObject = UnityEngine.Object.Instantiate(_playerGO, nextSpawnPoint.position, nextSpawnPoint.rotation);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(_mobGO, nextSpawnPoint2.position, nextSpawnPoint2.rotation);
			EliController componentInChildren = gameObject.GetComponentInChildren<EliController>();
			Humanoid humanoid = componentInChildren.Model as Humanoid;
			humanoid.AddSlug(new Slug(SlugType.Infurnus, 1));
			humanoid.AddSlug(new Slug(SlugType.Arachnet, 1));
			humanoid.AddSlug(new Slug(SlugType.Geoshard, 1));
			humanoid.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(BlasterType.LeaveBlaster, 6), BlasterType.LeaveBlaster);
			componentInChildren.BlasterType = BlasterType.LeaveBlaster;
			humanoid.SetArmor(MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(ArmorType.Default, 0));
			InGameController inGameController = UnityEngine.Object.FindObjectOfType<InGameController>();
			InGameModel inGameModel = new InGameModel(LevelType.Action);
			inGameModel.AttackSlug = new Slug(SlugType.Infurnus);
			inGameModel.DefenceSlug = new Slug(SlugType.Arachnet);
			inGameModel.SpecialSlug = new Slug(SlugType.Geoshard);
			playerCurrent = componentInChildren;
			componentInChildren.enabled = true;
			if (_CameraThirdView == null)
			{
				_CameraThirdView = UnityEngine.Object.FindObjectOfType<AutoCam>();
			}
			if (_CameraThirdView != null)
			{
				_CameraThirdView.SetTarget(componentInChildren.gameObject.transform);
				_CameraThirdView.GetComponent<SplineAnimator>().enabled = false;
				_CameraThirdView.GetComponent<ProtectCameraFromWallClip>().enabled = true;
			}
			BillyController billyController = (mobPlayerEnemy = gameObject2.GetComponentInChildren<BillyController>());
			if (billyController != null)
			{
				billyController.Model.MaxHealth = 2500f;
				billyController.Model.Health = 2500f;
				gameController.InGameController.NetPlayerAvatarInfo.OnApplyModel(billyController.Model);
				string randomMobName = MonoSingleton<GameConfiguration>.Instance.MobNamesConfiguration.GetRandomMobName();
				gameController.View.MultiplayerWinView.RivalNickName = randomMobName;
				gameController.View.MultiplayerLoseView.RivalNickName = randomMobName;
				float playerRank = gameController.View.MultiplayerWinView.PlayerRank;
				int num = UnityEngine.Random.Range(1, 6);
				gameController.View.MultiplayerWinView.RivalRank = playerRank - (float)num;
				gameController.View.MultiplayerLoseView.RivalRank = playerRank - (float)num;
			}
			if ((bool)DonDestroyMonoSingleton<AnalyticsManager>.Instance && (bool)GoogleAnalyticsV4.instance)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.multiplayerWithBot = true;
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackMultiplayerParameters(true);
			}
		}
		FixSceneForBot();
		Admission();
	}

	private void FixSceneForBot()
	{
		for (int i = 0; i < SpecialObjects.Length; i++)
		{
			for (int j = 0; j < SpecialObjects[i].Object.Length; j++)
			{
				SpecialObjects[i].Object[j].SendMessage(SpecialObjects[i].Mesege);
			}
		}
	}

	private IEnumerator ChangeCurTime(float time)
	{
		yield return new WaitForSeconds(time);
		disconnectCurTime -= 0.1f;
		if (disconnectCurTime < 0f)
		{
			disconnectCurTime = disconnectTime;
		}
		else
		{
			StartCoroutine(ChangeCurTime(0.1f));
		}
	}

	private IEnumerator ChangeCurSessionTime(float time)
	{
		yield return new WaitForSeconds(time);
		sessionCurTime--;
		if (sessionCurTime < 0)
		{
			sessionCurTime = sessionTime;
		}
		else
		{
			StartCoroutine(ChangeCurSessionTime(1f));
		}
	}

	private IEnumerator CorotineSetEnemyName(float time)
	{
		yield return new WaitForSeconds(time);
		SetEnemyName();
	}

	private IEnumerator CorotineEndTimeGame(float time)
	{
		yield return new WaitForSeconds(time);
		QuitToMenuWinOrLoose();
	}

	private IEnumerator CorotinePlayerDisconnect(float time)
	{
		yield return new WaitForSeconds(time);
		QuitToMenuWin();
	}
}
