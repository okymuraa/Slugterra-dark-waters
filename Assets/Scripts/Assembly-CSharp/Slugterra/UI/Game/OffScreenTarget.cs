using UnityEngine;

namespace Slugterra.UI.Game
{
	public class OffScreenTarget : MonoBehaviour
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private UnityEngine.Camera _cameraUI;

		[SerializeField]
		private float _screenOffset = 0.3f;

		private UnityEngine.Camera _camera;

		private Transform _transform;

		private Renderer _renderer;

		private void Start()
		{
			_camera = UnityEngine.Camera.main;
			_transform = transform;
			_renderer = GetComponent<Renderer>();
		}

		public void SwitchTarget(Transform _newTarget)
		{
			_target = _newTarget;
		}

		private static float Vector2dSignedAngle(Vector2 v1, Vector2 v2)
		{
			return 0f - Mathf.Atan2(v1.x * v2.y - v1.y * v2.x, v1.x * v2.x + v1.y * v2.y);
		}

		private static float RectangleRadius(Vector2 size, float t)
		{
			float f = 1f / Mathf.Cos(t);
			float f2 = 1f / Mathf.Sin(t);
			float magnitude = size.magnitude;
			float num = size.x * 0.5f;
			float num2 = size.y * 0.5f;
			return Mathf.Min(magnitude, Mathf.Min(num * Mathf.Abs(f), num2 * Mathf.Abs(f2)));
		}

		private void LateUpdate()
		{
			if (_target == null || _cameraUI == null || !_target.gameObject.activeSelf || Time.timeScale != 1f)
			{
				if (_renderer.enabled)
				{
					_renderer.enabled = false;
				}
				return;
			}
			Vector3 vector = _camera.WorldToScreenPoint(_target.position);
			Vector2 vector2 = new Vector2(Screen.width, Screen.height);
			bool flag = (vector.z > 0f && vector.x > 0f && vector.x < vector2.x && vector.y > 0f && vector.y < vector2.y) || !_camera.enabled;
			_renderer.enabled = !flag;
			if (!flag)
			{
				if (vector.z < 0f)
				{
					vector *= -1f;
				}
				vector.x -= vector2.x * 0.5f;
				vector.y -= vector2.y * 0.5f;
				float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f;
				Vector2 normalized = ((Vector2)vector).normalized;
				float num = vector2.y / vector2.x;
				Vector2 size = new Vector2(_cameraUI.orthographicSize * 2f, _cameraUI.orthographicSize * num * 2f);
				float num2 = Vector2dSignedAngle(normalized, Vector2.right);
				float num3 = RectangleRadius(size, num2) - _screenOffset;
				_transform.localPosition = new Vector3(num3 * Mathf.Cos(num2), num3 * Mathf.Sin(num2), _transform.localPosition.z);
				_transform.localEulerAngles = new Vector3(0f, 0f, z);
			}
		}
	}
}
