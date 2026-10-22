namespace Slugterra.Behavior.Data
{
	public class MovementSettings
	{
		public PatrolPathHolder PatrolPath;

		public float TargetReachedDistance = 0.26f;

		public bool AdvancedMovement;

		public bool CanPatrol = true;

		public bool CanMove = true;

		public bool CanRotate = true;

		public bool UseCustomWalkSpeed;

		public float CustomWalkSpeed;

		public bool UseCustomPatrolSpeed;

		public float CustomPatrolSpeed;

		public float WaitAfterLoseTargetChance;

		public float ReloadRunChance;

		public bool StepBackAfterAttack;

		public float StepBackChance;

		public bool IsMoveOrderRecieved;
	}
}
