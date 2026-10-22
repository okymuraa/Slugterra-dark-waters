using System;
using AppsMinistry.Core;
using Photon;
using Slugterra.App;
using Slugterra.Game.Units.NetPlayer;
using Slugterra.Multiplayer.Utils;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class SpawnPlayers : PunBehaviour
{
	private PhotonAnimatorView m_AnimatorView;

	private Animator m_RemoteAnimator;

	private float m_SlideIn;

	private float m_FoundPlayerSlideIn;

	private bool m_IsOpen;

	[SerializeField]
	private SpawnPoints _spawnPoints;

	[SerializeField]
	private AutoCam _CameraThirdView;

	private NetPlayerController controller;

	public void Awake()
	{
	}

	public void Update()
	{
		FindRemoteAnimator();
		m_SlideIn = Mathf.Lerp(m_SlideIn, (!m_IsOpen) ? 0f : 1f, Time.deltaTime * 9f);
		m_FoundPlayerSlideIn = Mathf.Lerp(m_FoundPlayerSlideIn, (!(m_AnimatorView == null)) ? 1f : 0f, Time.deltaTime * 5f);
	}

	public void FindRemoteAnimator()
	{
		if (m_RemoteAnimator != null)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			PhotonView component = array[i].GetComponent<PhotonView>();
			if (component != null && !component.isMine)
			{
				m_RemoteAnimator = array[i].GetComponent<Animator>();
			}
		}
	}

	public override void OnJoinedRoom()
	{
	}

	public void CreatePlayerObject()
	{
		Vector3 position = new Vector3(0f, 0f, 0f);
		Quaternion rotation = Quaternion.identity;
		Transform transform = null;
		if (_spawnPoints != null)
		{
			transform = _spawnPoints.GetNextSpawnPoint();
			if (!PhotonNetwork.isMasterClient)
			{
				transform = _spawnPoints.GetNextSpawnPoint();
			}
			position = transform.position;
			rotation = transform.rotation;
		}
		GameObject gameObject = PhotonNetwork.Instantiate("Prefabs/Multiplayer/Player/Player", position, rotation, 0, new object[2]
		{
			"Player",
			MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank
		}, true);
		if (_CameraThirdView == null)
		{
			_CameraThirdView = UnityEngine.Object.FindObjectOfType<AutoCam>();
		}
		if (_CameraThirdView != null && gameObject != null)
		{
			controller = gameObject.GetComponentInChildren<NetPlayerController>();
			controller.enabled = true;
			_CameraThirdView.SetTarget(controller.gameObject.transform);
			string playerName = Environment.MachineName;
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.IsPlayerAuthenticated())
			{
				playerName = DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerAlias;
			}
			gameObject.name = playerName;
			PhotonNetwork.playerName = playerName;
			controller.UpdateProgressBar();
		}
		m_AnimatorView = gameObject.GetComponent<PhotonAnimatorView>();
	}

	public void Elifire()
	{
		if (controller != null)
		{
			controller.OnBeginAttack();
		}
	}
}
