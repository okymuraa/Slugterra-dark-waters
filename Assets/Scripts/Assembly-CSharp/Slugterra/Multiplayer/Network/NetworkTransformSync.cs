using UnityEngine;

namespace Slugterra.Multiplayer.Network
{
	public class NetworkTransformSync : NetworkSyncComponent, IPunObservable
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private bool _positionSync = true;

		[SerializeField]
		private bool _rotationSync = true;

		private bool _isLocal;

		private Vector3 _networkPosition;

		private Quaternion _networkRotation;

		private double _lastNetworkDataReceivedTime;

		private Vector3 _lastPosition = Vector3.zero;

		private float _speed;

		public void Awake()
		{
			_isLocal = photonView.isMine;
		}

		private void OnEnable()
		{
			_lastPosition = _target.position;
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				if (_positionSync)
				{
					stream.SendNext(_target.position);
					stream.SendNext(_speed);
				}
				if (_rotationSync)
				{
					stream.SendNext(_target.rotation);
				}
				return;
			}
			if (_positionSync)
			{
				_networkPosition = (Vector3)stream.ReceiveNext();
				_speed = (float)stream.ReceiveNext();
			}
			if (_rotationSync)
			{
				_networkRotation = (Quaternion)stream.ReceiveNext();
			}
			_lastNetworkDataReceivedTime = PhotonNetwork.time;
		}

		private void CalculateSpeed()
		{
			_speed = (_target.position - _lastPosition).magnitude / Time.unscaledDeltaTime;
			_lastPosition = _target.position;
		}

		private void UpdateNetworkedPosition()
		{
			if (Mathf.Abs(_speed) > 0.001f)
			{
				float num = (float)PhotonNetwork.GetPing() * 0.001f;
				float num2 = (float)(PhotonNetwork.time - _lastNetworkDataReceivedTime);
				float num3 = num + num2;
				Vector3 normalized = (_networkPosition - _target.position).normalized;
				Vector3 target = _networkPosition + normalized * _speed * num3;
				Vector3 position = Vector3.MoveTowards(_target.position, target, _speed * Time.unscaledDeltaTime);
				_target.position = position;
			}
			else
			{
				_target.position = Vector3.Lerp(_target.position, _networkPosition, 15f * Time.unscaledDeltaTime);
			}
		}

		private void UpdateNetworkedRotation()
		{
			_target.rotation = Quaternion.RotateTowards(_target.rotation, _networkRotation, 180f * Time.unscaledDeltaTime);
		}

		private void Update()
		{
			if (_isLocal && _positionSync)
			{
				CalculateSpeed();
				return;
			}
			if (_positionSync)
			{
				UpdateNetworkedPosition();
			}
			if (_rotationSync)
			{
				UpdateNetworkedRotation();
			}
		}
	}
}
