using UnityEngine;
using UnityEngine.UI;

namespace Slugterra.UI.Game
{
	public class OnScreenTargetCanvas : MonoBehaviour
	{
		private RectTransform rectTransform;

		private Image _renderer;

		public Renderer OnScreenTarget;

		public Transform target;

		[SerializeField]
		private Vector3 _targetOffset = Vector3.zero;

		public void SwitchTarget(Transform _newTarget)
		{
			target = _newTarget;
			if (target == null)
			{
				_renderer.enabled = false;
			}
		}

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			_renderer = GetComponent<Image>();
		}

		private void Update()
		{
			if (target == null)
			{
				_renderer.enabled = false;
				return;
			}
			Vector3 worldPoint = target.position + _targetOffset;
			Vector3 position = RectTransformUtility.WorldToScreenPoint(UnityEngine.Camera.main, worldPoint);
			Vector2 vector = new Vector2(Screen.width, Screen.height);
			bool flag = OnScreenTarget.enabled;
			_renderer.enabled = flag;
			if (flag)
			{
				rectTransform.position = position;
			}
		}
	}
}
