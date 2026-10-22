using UnityEngine;

namespace Slugterra.UI.Game
{
	public class OnScreenTarget : MonoBehaviour
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private Vector3 _targetOffset = Vector3.zero;

		[SerializeField]
		private float _screenOffset = 0.5f;

		[SerializeField]
		private UnityEngine.Camera _cameraUI;

		private UnityEngine.Camera _camera;

		private Transform _transform;

		private Renderer _renderer;

		public bool UseRotation;

		private void Awake()
		{
			_camera = UnityEngine.Camera.main;
			_transform = transform;
			_renderer = GetComponent<Renderer>();
		}

		public void SwitchTarget(Transform _newTarget)
		{
			_target = _newTarget;
			if (_target == null)
			{
				_renderer.enabled = false;
			}
		}

		private static float Vector2dSignedAngle(Vector2 v1, Vector2 v2)
		{
			return 0f - Mathf.Atan2(v1.x * v2.y - v1.y * v2.x, v1.x * v2.x + v1.y * v2.y);
		}

		private void LateUpdate()
		{
			if (_target == null)
			{
				_renderer.enabled = false;
				return;
			}
			Vector3 position = _target.position + _targetOffset;
			Vector3 vector = _camera.WorldToScreenPoint(position);
			Vector2 vector2 = new Vector2(Screen.width, Screen.height);
			bool flag = vector.z > 0f && vector.x > 0f && vector.x < vector2.x && vector.y > 0f && vector.y < vector2.y;
			_renderer.enabled = flag;
			if (flag)
			{
				float num = 1f;
				float num2 = 0f;
				Vector2 zero = Vector2.zero;
				num = vector2.y / vector2.x;
				Vector2 vector3 = new Vector2(_cameraUI.orthographicSize * 2f, _cameraUI.orthographicSize * num * 2f);
				vector.x = vector.x / vector2.x * vector3.x;
				vector.y = vector.y / vector2.y * vector3.y;
				vector.x -= vector3.x * 0.5f;
				vector.y -= vector3.y * 0.5f;
				num2 = Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f;
				zero = ((Vector2)vector).normalized;
				float num3 = Vector2.Distance(vector, Vector2.zero) - _screenOffset;
				float f = Vector2dSignedAngle(zero, Vector2.right);
				_transform.localPosition = new Vector3(num3 * Mathf.Cos(f), num3 * Mathf.Sin(f), _transform.localPosition.z);
				if (UseRotation)
				{
					_transform.localEulerAngles = new Vector3(0f, 0f, num2);
				}
				else
				{
					_transform.localEulerAngles = Vector3.zero;
				}
			}
		}
	}
}
