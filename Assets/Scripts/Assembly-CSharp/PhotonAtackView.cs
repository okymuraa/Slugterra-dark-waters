using UnityEngine;

[AddComponentMenu("Photon Networking/Photon Atack View")]
[RequireComponent(typeof(PhotonView))]
public class PhotonAtackView : MonoBehaviour, IPunObservable
{
	[SerializeField]
	private PhotonTransformViewPositionModel m_PositionModel = new PhotonTransformViewPositionModel();

	private PhotonTransformViewPositionControl m_AtackControl;

	private PhotonView m_PhotonView;

	private bool m_ReceivedNetworkUpdate;

	private bool m_firstTake;

	private void Awake()
	{
		m_PhotonView = GetComponent<PhotonView>();
	}

	private void OnEnable()
	{
		m_firstTake = true;
	}

	private void Update()
	{
		if (!(m_PhotonView == null) && !m_PhotonView.isMine && PhotonNetwork.connected)
		{
			UpdatePosition();
		}
	}

	private void UpdatePosition()
	{
		if (m_PositionModel.SynchronizeEnabled && m_ReceivedNetworkUpdate)
		{
		}
	}

	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!m_PhotonView.isMine && m_PositionModel.DrawErrorGizmo)
		{
			DoDrawEstimatedPositionError();
		}
		if (stream.isReading)
		{
			m_ReceivedNetworkUpdate = true;
			if (m_firstTake)
			{
				m_firstTake = false;
			}
		}
	}

	private void DoDrawEstimatedPositionError()
	{
		if (!(transform.parent != null))
		{
		}
	}
}
