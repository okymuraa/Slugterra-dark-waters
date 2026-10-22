using Photon;
using UnityEngine;

public class SmoothSyncMovement : Photon.MonoBehaviour, IPunObservable
{
	public float SmoothingDelay = 5f;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	public void Awake()
	{
		if (photonView == null || photonView.observed != this)
		{
			Debug.LogWarning(string.Concat(this, " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used."));
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}

	public void Update()
	{
		if (!photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * SmoothingDelay);
			transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * SmoothingDelay);
		}
	}
}
