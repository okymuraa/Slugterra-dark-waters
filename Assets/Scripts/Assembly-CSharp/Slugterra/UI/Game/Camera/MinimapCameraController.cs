using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.UI.Game.Camera
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	[DisallowMultipleComponent]
	public class MinimapCameraController : MonoBehaviour
	{
		public const float SwipeForceFactor = 8f;

		[SerializeField]
		private BoxCollider _viewZone;

		[SerializeField]
		private float _smoothSpeed = 15f;

		private UnityEngine.Camera _camera;

		private Transform _cameraTransform;

		private Vector3 _cameraPosition;

		private Vector3 _prevMovePos = Vector3.zero;

		private Vector3 _lastWorldDelta = Vector3.zero;

		private PlayerController _player;

		private void Start()
		{
			_camera = GetComponent<UnityEngine.Camera>();
			_cameraTransform = _camera.transform;
			_cameraPosition = _cameraTransform.position;
			_player = PlayerManager.LocalPlayer;
			MoveToPlayer();
		}

		private void OnEnable()
		{
			MoveToPlayer();
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += OnMove;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnTouchEnd;
		}

		private void OnDisable()
		{
			if (DonDestroyMonoSingleton<InputManager>.Instance != null)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouch -= OnMove;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd -= OnTouchEnd;
			}
		}

		private void MoveToPlayer()
		{
			if (_player != null)
			{
				Vector3 position = _player.transform.position;
				Vector3 cameraPosition = _cameraPosition;
				cameraPosition.y = (position.y = 0f);
				Vector3 offset = position - cameraPosition;
				TranslateCamera(offset);
			}
		}

		private Vector3 ScreenToWorld(Vector2 position)
		{
			float num = 2f * _camera.orthographicSize;
			float num2 = num * _camera.aspect;
			float num3 = num / ((float)Screen.height * _camera.rect.height);
			float num4 = num2 / ((float)Screen.width * _camera.rect.width);
			return new Vector3(position.x * num4, 0f, position.y * num3);
		}

		private Vector3 RotatePoint(Vector3 point, Quaternion rotation)
		{
			return Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one).MultiplyPoint(point);
		}

		private void TranslateCamera(Vector3 offset)
		{
			Vector3 vector = new Vector3(_camera.orthographicSize * _camera.aspect, 0f, _camera.orthographicSize);
			Vector3 vector2 = _viewZone.bounds.min + vector;
			Vector3 vector3 = _viewZone.bounds.max - vector;
			_cameraPosition += offset;
			_cameraPosition.x = Mathf.Clamp(_cameraPosition.x, vector2.x, vector3.x);
			_cameraPosition.z = Mathf.Clamp(_cameraPosition.z, vector2.z, vector3.z);
		}

		private void OnMove(Vector3 position, float actionTime)
		{
			if (_prevMovePos == Vector3.zero)
			{
				_prevMovePos = position;
			}
			Vector3 vector = -(position - _prevMovePos);
			Vector3 eulerAngles = _cameraTransform.rotation.eulerAngles;
			Vector3 vector2 = RotatePoint(ScreenToWorld(vector), Quaternion.Euler(0f, eulerAngles.y, eulerAngles.z));
			TranslateCamera(vector2);
			_lastWorldDelta = vector2;
			_prevMovePos = position;
		}

		private void OnTouchEnd(Vector3 position, float ationTime)
		{
			_prevMovePos = Vector3.zero;
			float magnitude = _lastWorldDelta.magnitude;
			if (magnitude > 1f)
			{
				TranslateCamera(_lastWorldDelta.normalized * (magnitude * 8f));
			}
			_lastWorldDelta = Vector3.zero;
		}

		private void LateUpdate()
		{
			_cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraPosition, _smoothSpeed * Time.unscaledDeltaTime);
		}
	}
}
