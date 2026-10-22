using UnityEngine;

namespace Slugterra.Game.Units
{
	[RequireComponent(typeof(Seeker))]
	public class UnitPathFinder : AIPath
	{
		[SerializeField]
		private float sleepVelocity = 0.4f;

		private Vector3 _velocity;

		public Vector3 MoveVelocity
		{
			get
			{
				return _velocity;
			}
		}

		public override Vector3 GetFeetPosition()
		{
			return tr.position;
		}

		protected new void Update()
		{
			if (MovementType == UnitController.UnitMovementType.CustomLookPoint && CustomLookPoint == null)
			{
				MovementType = UnitController.UnitMovementType.Forward;
			}
			Vector3 direction;
			if (canMove)
			{
				Vector3 vector = CalculateVelocity(GetFeetPosition());
				if (MovementType == UnitController.UnitMovementType.Forward)
				{
					RotateTowards(targetDirection);
				}
				else if (MovementType == UnitController.UnitMovementType.Backward)
				{
					RotateTowards(Quaternion.AngleAxis(180f, tr.up) * targetDirection);
				}
				else if (MovementType == UnitController.UnitMovementType.CustomLookPoint)
				{
					RotateTowards(CustomLookPoint.position - transform.position);
				}
				vector.y = 0f;
				if (vector.sqrMagnitude < sleepVelocity * sleepVelocity)
				{
					vector = Vector3.zero;
				}
				if (controller != null)
				{
					controller.SimpleMove(vector);
				}
				direction = controller.velocity;
			}
			else
			{
				direction = Vector3.zero;
			}
			_velocity = tr.InverseTransformDirection(direction);
		}
	}
}
