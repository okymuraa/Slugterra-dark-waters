using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class ObjectiveTargetIcon : MonoBehaviour
	{
		private const float BASE_OFFSET = 0.045f;

		private const float MIN_SMOOTH_TIME = 0.15f;

		[SerializeField]
		private tk2dSprite _circle;

		[SerializeField]
		private tk2dSprite _arrow;

		private Transform _arrtowTransform;

		private Transform _transform;

		private Vector3 _positionVelocity;

		private float _angleVelocity;

		private Vector3 _arrowPositionVelocity;

		private float positionSmoothTime;

		private void Awake()
		{
			_arrtowTransform = _arrow.transform;
			_transform = transform;
		}

		private void OnEnable()
		{
			_transform.localPosition = new Vector3(0f, 0f, _transform.localPosition.z);
			positionSmoothTime = 1f;
		}

		public void SetPosition(Vector3 position)
		{
			position.z = 0.5f;
			positionSmoothTime -= Time.deltaTime;
			positionSmoothTime = Mathf.Max(positionSmoothTime, 0.15f);
			_transform.localPosition = Vector3.SmoothDamp(_transform.localPosition, position, ref _positionVelocity, positionSmoothTime);
		}

		public void SetDirection(Vector3 direction)
		{
			if (gameObject.activeSelf)
			{
				float num = Mathf.Min(Vector3.SqrMagnitude(direction), 75f);
				float num2 = (2f - num / 75f) * 4f;
				_transform.localScale = Vector3.one * num2;
				Vector3 normalized = direction.normalized;
				float target = Mathf.Atan2(normalized.z, normalized.x) * 57.295f - 90f;
				target = Mathf.SmoothDampAngle(_arrtowTransform.localEulerAngles.z, target, ref _angleVelocity, 0.15f);
				if (float.IsNaN(target))
				{
					target = 0f;
				}
				_arrtowTransform.localEulerAngles = new Vector3(0f, 0f, target);
				Vector3 target2 = new Vector3(0.045f * normalized.x, 0.045f * normalized.z, _arrtowTransform.localPosition.z);
				_arrtowTransform.localPosition = Vector3.SmoothDamp(_arrtowTransform.localPosition, target2, ref _arrowPositionVelocity, 0.15f);
			}
		}
	}
}
