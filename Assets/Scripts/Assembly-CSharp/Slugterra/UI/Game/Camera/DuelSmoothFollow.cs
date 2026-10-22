using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.UI.Game.Camera
{
	public class DuelSmoothFollow : MonoBehaviour
	{
		private const float SMOOTH_TIME = 0.15f;

		private CameraSmoothFollow _cameraSmoothFollow;

		[SerializeField]
		private Vector3 _duelRotation;

		[SerializeField]
		private Vector3 _duelOffset;

		private Transform _playerTransform;

		private Transform _target;

		private Transform _transform;

		private Vector3 _moveVelocity;

		private Vector3 _rotationVelocity;

		public float RadiusShake = 0.3f;

		public float TimeShake = 0.5f;

		private float timerShake;

		private Vector3 Shake = Vector3.zero;

		public bool SmoothShake;

		public float ReduceSmooth = 1f;

		private void Awake()
		{
			_cameraSmoothFollow = GetComponent<CameraSmoothFollow>();
			_transform = transform;
			_rotationVelocity = Vector3.zero;
			_moveVelocity = Vector3.zero;
		}

		public void OnDuel(Transform playerTransform, Transform target)
		{
			_playerTransform = playerTransform;
			_target = target;
			if (_cameraSmoothFollow != null)
			{
				_cameraSmoothFollow.enabled = false;
			}
			enabled = true;
			transform.localEulerAngles = _duelRotation;
		}

		public void SetTarget(Transform target)
		{
			_target = target;
		}

		private void Update()
		{
			if (_target == null)
			{
				return;
			}
			Vector3 normalized = (_playerTransform.position - _target.position).normalized;
			Vector3 vector = new Vector3(normalized.x * (0f - _duelOffset.z), _duelOffset.y, normalized.z * (0f - _duelOffset.z));
			if (timerShake > 0f)
			{
				timerShake -= Time.deltaTime;
				if (!SmoothShake)
				{
					Shake = Random.insideUnitSphere * RadiusShake;
				}
				else
				{
					Shake = Vector3.Lerp(Shake, Random.insideUnitSphere * RadiusShake, Time.deltaTime * ReduceSmooth);
				}
				vector.z = normalized.z * (0f - _duelOffset.z);
				vector += Shake;
			}
			else if (Shake != Vector3.zero)
			{
				Shake = Vector3.zero;
				vector += Shake;
			}
			float angle = MathUtil.GetAngle(new Vector2(1f, 0f), new Vector2(_target.position.x - _transform.position.x, _target.position.z - _transform.position.z));
			_transform.position = Vector3.SmoothDamp(_transform.position, _playerTransform.position + vector, ref _moveVelocity, 0.15f);
			angle = Mathf.SmoothDampAngle(_transform.localEulerAngles.y, angle + 90f, ref _rotationVelocity.y, 0.15f);
			_transform.localEulerAngles = new Vector3(_transform.localEulerAngles.x, angle, _transform.localEulerAngles.z);
		}

		public void DoShake(float timer = 0f)
		{
			if (timer == 0f)
			{
				timerShake = TimeShake;
			}
			else
			{
				timerShake = timer;
			}
		}
	}
}
