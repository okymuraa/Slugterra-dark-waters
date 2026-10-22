using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using Slugterra.Game.Input;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.UI.Game.Camera
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class CameraSmoothFollow : MonoBehaviour
	{
		[SerializeField]
		private float _smoothTime = 1.5f;

		[SerializeField]
		public Vector3 cameraOffset;

		[SerializeField]
		private Transform _player;

		[SerializeField]
		private Transform _target;

		public float _damping = 6f;

		public float _dragSpeed = 0.04f;

		private UnityEngine.Camera _camera;

		private float _maxFollowSpeed;

		private Transform _transform;

		private Vector3 _cameraVelocity;

		private Vector3 _pointToLookAtVelocity;

		private Vector3 _pointToLookAt;

		public float _posMixer = 0.5f;

		private const float SPEED_RATIO = 20f;

		private Vector3 _drag = Vector3.zero;

		[SerializeField]
		private float _maxZoom = 65f;

		[SerializeField]
		private float _minZoom = 45f;

		[SerializeField]
		private float _zoom = 65f;

		private float _zoomVelocity;

		private float _CameraMaxHeight = 12f;

		private float v_CameraY;

		private Vector3 _previosPlayerPos;

		private bool _isDrag;

		private float _speed;

		private bool _isDetour;

		public float RadiusShake = 0.1f;

		public float TimeShake = 0.5f;

		private float timerShake;

		private Vector3 Shake = Vector3.zero;

		public bool SmoothShake;

		public float ReduceSmooth = 1f;

		private Vector3 oldCameraPos;

		private void Awake()
		{
			_transform = transform;
			_camera = GetComponent<UnityEngine.Camera>();
			_cameraVelocity = new Vector3(0.3f, 0.3f, 0.3f);
			_pointToLookAtVelocity = new Vector3(0.3f, 0.3f, 0.3f);
			v_CameraY = cameraOffset.y;
			_CameraMaxHeight = v_CameraY;
			PlayerManager.OnAddLocalPlayer(OnSpawnPlayer);
		}

		private void OnSpawnPlayer(PlayerController player)
		{
			if (player != null)
			{
				_player = player.transform;
				_target = player.Waypoint.transform;
				_pointToLookAt = _target.position + Vector3.up;
			}
		}

		private void OnEnable()
		{
			if ((bool)_player)
			{
				_pointToLookAt = _player.position + Vector3.up;
			}
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnRelease;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTap += OnTap;
			DonDestroyMonoSingleton<InputManager>.Instance.OnZoom += OnZoom;
		}

		private void OnDisable()
		{
		}

		private void OnDrag(Vector3 drag)
		{
			_drag = -drag;
			_isDrag = true;
		}

		private void OnZoom(float zoom)
		{
			_zoom -= zoom;
			_zoom = Mathf.Min(Mathf.Max(_zoom, _minZoom), _maxZoom);
		}

		private void OnTap(Vector3 position)
		{
			Vector3 terrainPosition;
			if (MonoSingleton<GameCollisionMap>.Instance.IsTapOnTerrain(position, out terrainPosition))
			{
				_isDetour = false;
			}
		}

		private void OnRelease(Vector3 position, float actionTime)
		{
			_drag = Vector3.zero;
			_isDrag = false;
		}

		private void Update()
		{
			if ((bool)_camera)
			{
				_camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, _zoom, ref _zoomVelocity, 0.3f);
			}
		}

		private void LateUpdate()
		{
			if (_player == null)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 zero3 = Vector3.zero;
			zero3 = (_target.position - _player.position) * _posMixer + _player.position;
			if ((bool)_camera)
			{
				vector2 = _camera.WorldToViewportPoint(_player.position + Vector3.up);
				zero2 = _camera.WorldToViewportPoint(_target.position);
				bool flag = vector2.x > 0f && vector2.x < 1f && vector2.y > 0f && vector2.y < 1f;
				bool flag2 = zero2.x > 0f && zero2.x < 1f && zero2.y > 0f && zero2.y < 1f;
				float num = Mathf.Max(Mathf.Abs(vector2.x - zero2.x), Mathf.Abs(vector2.y - zero2.y));
				if (flag && flag2 && !_isDetour)
				{
					zero3 = (_target.position - _player.position) * _posMixer + _player.position;
					_maxFollowSpeed = (0.8f - num) * 20f * cameraOffset.y / _CameraMaxHeight;
					_maxFollowSpeed = ((!(_maxFollowSpeed > 0f)) ? 0f : _maxFollowSpeed);
					_speed = 1f;
				}
				else
				{
					zero3 = _player.position;
					_maxFollowSpeed = _speed;
					_speed = ((!(_speed < 20f)) ? 20f : (_speed + 0.1f));
					_isDetour = true;
				}
			}
			if (!_isDrag)
			{
				vector = Vector3.SmoothDamp(_pointToLookAt, zero3 + Vector3.up, ref _pointToLookAtVelocity, _smoothTime, _maxFollowSpeed);
				if (Vector3.SqrMagnitude(_pointToLookAt - vector) > 25f)
				{
					Debug.Log("Point " + Vector3.SqrMagnitude(_pointToLookAt - vector));
				}
				zero.x = vector.x + cameraOffset.x;
				zero.z = vector.z + cameraOffset.z;
				zero.y = Mathf.SmoothDamp(_transform.position.y, zero3.y + cameraOffset.y, ref _cameraVelocity.y, _smoothTime);
				if (timerShake > 0f)
				{
					if (Time.timeScale > 0f)
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
						zero += Shake;
					}
				}
				else if (Shake != Vector3.zero)
				{
					Shake = Vector3.zero;
					zero += Shake;
				}
			}
			else
			{
				if (((double)vector2.x > 0.9 || (double)vector2.x > 0.9) && _drag.z > 0f)
				{
					_drag.z = 0f;
				}
				if (Vector3.SqrMagnitude(_player.position - _transform.position) > 625f && _drag.z < 0f)
				{
					_drag.z = 0f;
				}
				if ((double)vector2.x > 0.9 && _drag.x < 0f)
				{
					_drag.x = 0f;
				}
				if ((double)vector2.x < 0.1 && _drag.x > 0f)
				{
					_drag.x = 0f;
				}
				if ((double)vector2.y < 0.1 && _drag.z > 0f)
				{
					_drag.z = 0f;
				}
				if ((double)vector2.y > 0.9 && _drag.z < 0f)
				{
					_drag.z = 0f;
				}
				Vector3 vector3 = _player.position - _previosPlayerPos;
				zero.x = _transform.position.x + _drag.x * _dragSpeed + vector3.x;
				zero.y = _transform.position.y;
				zero.z = _transform.position.z + _drag.z * _dragSpeed + vector3.z;
				vector.x = _pointToLookAt.x + _drag.x * _dragSpeed + vector3.x;
				vector.y = _pointToLookAt.y;
				vector.z = _pointToLookAt.z + _drag.z * _dragSpeed + vector3.z;
			}
			Quaternion b = Quaternion.LookRotation(_pointToLookAt + -_transform.position);
			_transform.rotation = Quaternion.Slerp(_transform.rotation, b, Time.deltaTime * _damping);
			_transform.position = zero;
			oldCameraPos = zero;
			_pointToLookAt = vector;
			_previosPlayerPos = _player.position;
		}

		public void SetTarget(Transform target)
		{
			_target = target;
		}

		public void SetPlayer(Transform target)
		{
			_player = target;
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
