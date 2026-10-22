using UnityEngine;

namespace AppsMinistry.Core.Motion
{
	public class MoveMotion : BaseMotion
	{
		private enum Position
		{
			World = 0,
			Local = 1
		}

		[SerializeField]
		private Position _position;

		[SerializeField]
		private Vector3 _targetPosition;

		private Vector3 _basePosition;

		private Vector3 _velocity;

		protected override void OnStart()
		{
			base.OnStart();
			if (_position == Position.World)
			{
				_basePosition = Transform.position;
			}
			else if (_position == Position.Local)
			{
				_basePosition = Transform.localPosition;
			}
		}

		protected override void OnMotion()
		{
			if (_position == Position.World)
			{
				if (Iteration % 2 == 0)
				{
					Transform.position = Vector3.Lerp(_basePosition, _targetPosition, CurrentTime / Time);
				}
				else if (Loop)
				{
					Transform.position = Vector3.Lerp(_targetPosition, _basePosition, CurrentTime / Time);
				}
			}
			else if (_position == Position.Local)
			{
				if (Iteration % 2 == 0)
				{
					Transform.localPosition = Vector3.Lerp(_basePosition, _targetPosition, CurrentTime / Time);
				}
				else if (Loop)
				{
					Transform.localPosition = Vector3.Lerp(_targetPosition, _basePosition, CurrentTime / Time);
				}
			}
		}
	}
}
