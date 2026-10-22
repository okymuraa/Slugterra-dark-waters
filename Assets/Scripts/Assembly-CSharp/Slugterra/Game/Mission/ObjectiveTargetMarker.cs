using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class ObjectiveTargetMarker : MonoBehaviour
	{
		private enum State
		{
			None = 0,
			Increase = 1,
			Decrease = 2
		}

		private State _state;

		private Transform _transform;

		private Vector3 _baseScale;

		private Vector3 _targetScale;

		private Vector3 _velocity;

		private void Start()
		{
			_state = State.Decrease;
			_transform = transform;
			_baseScale = _transform.localScale;
			_targetScale = _baseScale * 0.7f;
			_velocity = Vector3.zero;
		}

		private void Update()
		{
			if (Vector3.SqrMagnitude(_transform.localScale - _targetScale) < 0.01f)
			{
				if (_state == State.Decrease)
				{
					_state = State.Increase;
					_targetScale = _baseScale;
				}
				else if (_state == State.Increase)
				{
					_state = State.Decrease;
					_targetScale = _baseScale * 0.7f;
				}
			}
			_transform.localScale = Vector3.SmoothDamp(_transform.localScale, _targetScale, ref _velocity, 0.5f);
		}
	}
}
