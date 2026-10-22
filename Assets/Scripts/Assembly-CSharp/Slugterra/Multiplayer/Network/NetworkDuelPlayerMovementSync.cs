using AppsMinistry.Utils;
using Slugterra.Game.Units.NetPlayer;
using Slugterra.Game.Units.Player.Strategy;
using Slugterra.Multiplayer.Player;
using UnityEngine;

namespace Slugterra.Multiplayer.Network
{
	[RequireComponent(typeof(NetworkPlayerRoot))]
	public class NetworkDuelPlayerMovementSync : NetworkSyncComponent, IPunObservable
	{
		private Transform _target;

		private BaseSplineInputStrategy _strategy;

		private NetworkPlayerRoot _root;

		private Animator _animator;

		private float _networkParam;

		private double _lastNetworkDataReceivedTime;

		private Vector3 _lastPosition;

		private float _lastParam;

		private float _speed;

		private bool _isRolling;

		private bool _isRollingAnimation;

		private bool _isLocal;

		private float error;

		private void Start()
		{
			_isLocal = photonView.isMine;
			_root = GetComponent<NetworkPlayerRoot>();
			NetPlayerController player = _root.Player;
			_target = player.transform;
			_animator = player.Animator;
			if (player.InputStrategy is BaseSplineInputStrategy)
			{
				_strategy = player.InputStrategy as BaseSplineInputStrategy;
			}
			if (_strategy != null)
			{
				_lastParam = (_networkParam = _strategy.CurrentParam);
				if (_isLocal)
				{
					_strategy.OnRollEvent += OnRoll;
				}
			}
			_lastPosition = _target.position;
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			float num = 0.5f;
			if (stream.isWriting)
			{
				if (_strategy != null)
				{
					num = _strategy.CurrentParam;
				}
				stream.SendNext(num);
				stream.SendNext(_speed);
				return;
			}
			_networkParam = (float)stream.ReceiveNext();
			_speed = (float)stream.ReceiveNext();
			_lastNetworkDataReceivedTime = PhotonNetwork.time;
			if (_strategy != null)
			{
				error = _networkParam - _strategy.CurrentParam;
			}
		}

		private void CalculateSpeed()
		{
			if (_strategy != null)
			{
				_speed = Mathf.Abs(_strategy.CurrentParam - _lastParam) / Time.smoothDeltaTime;
				_lastParam = _strategy.CurrentParam;
			}
		}

		private void UpdateNetworkedPosition()
		{
			if (_strategy != null)
			{
				if (Mathf.Abs(_speed) > 0.001f)
				{
					float num = (float)PhotonNetwork.GetPing() * 0.001f;
					float num2 = (float)(PhotonNetwork.time - _lastNetworkDataReceivedTime);
					float num3 = num + num2;
					float num4 = Mathf.Sign(_networkParam - _strategy.CurrentParam);
					float target = _networkParam + num4 * _speed * num3;
					_strategy.CurrentParam = Mathf.MoveTowards(_strategy.CurrentParam, target, _speed * Time.unscaledDeltaTime);
				}
				else
				{
					_strategy.CurrentParam = Mathf.Lerp(_strategy.CurrentParam, _networkParam, 15f * Time.unscaledDeltaTime);
				}
			}
		}

		private void UpdatePlayerMoveAnimation()
		{
			if (_isRolling || _isRollingAnimation)
			{
				bool flag = _animator.GetCurrentAnimatorStateInfo(0).IsName("Roll_Pistol");
				if (flag && _isRolling)
				{
					_isRolling = false;
					_animator.SetBool("RollTrigger", _isRolling);
				}
				else if (!_isRolling && !flag && !_animator.IsInTransition(0) && _isRollingAnimation)
				{
					_isRollingAnimation = false;
					_animator.SetFloat("RollDirectionX", 0f);
					_animator.SetFloat("RollDirectionY", 0f);
				}
				_animator.SetFloat("VelocityX", 0f);
				_animator.SetFloat("VelocityY", 0f);
			}
			else
			{
				Vector3 direction = (_target.position - _lastPosition) / Time.unscaledDeltaTime;
				direction = _target.InverseTransformDirection(direction);
				Vector2 vector = new Vector2(direction.x, direction.z);
				if (_root.Player.RotateToTarget)
				{
					vector = MathUtil.Rotate(vector, _root.Player.RotateToCommand.Angle);
				}
				else
				{
					_animator.transform.localEulerAngles = Vector3.zero;
				}
				_animator.SetFloat("VelocityX", vector.x);
				_animator.SetFloat("VelocityY", vector.y);
			}
			_lastPosition = _target.position;
		}

		private void OnRoll(Vector2 direction)
		{
			photonView.RPC("RpcOnRoll", PhotonTargets.Others, direction);
		}

		[PunRPC]
		private void RpcOnRoll(Vector2 direction)
		{
			_isRolling = true;
			_isRollingAnimation = true;
			_animator.SetFloat("RollDirectionX", direction.x);
			_animator.SetFloat("RollDirectionY", direction.y);
			_animator.SetBool("RollTrigger", _isRolling);
		}

		private void OnGUI()
		{
			PhotonPlayer owner = photonView.owner;
			GUI.color = Color.white;
			GUI.Label(new Rect(370f, 400f + (float)owner.ID * 30f, 350f, 30f), string.Format("error = {0}", error));
		}

		private void Update()
		{
			if (_isLocal)
			{
				CalculateSpeed();
				return;
			}
			UpdateNetworkedPosition();
			UpdatePlayerMoveAnimation();
		}
	}
}
