using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Animals;
using Slugterra.Game.Units.Mechanisms.Spline;
using Slugterra.UI.Game;
using UnityEngine;

public class MoveToPoint : Action
{
	public enum MoveType
	{
		None = 0,
		Run = 1,
		Walk = 2,
		Backward = 3
	}

	public MoveType moveType;

	public UnitControllerVar unitController;

	public SharedTransform enemyRoot;

	public SharedTransform attackTarget;

	public SharedObject enemyController;

	public SharedBool canMove;

	public SharedBool stun;

	public SharedBool fear;

	public SharedBool movingInProgress;

	public MoveTargetTypeSharedVariable moveTargetType;

	public SharedBool isRangeAttacker;

	public SharedBool isTargetFinded;

	public SharedBool isAlive;

	public SharedBool isMoveOrderRecieved;

	public SharedBool useCustomDirection;

	public SharedBool isEnemyReloadSlug;

	public SharedBool stopMoveIfEmenyReloadSlug;

	public SharedBool useCustomWalkSpeed;

	public SharedFloat customWalkSpeed;

	public SharedBool useCustomPatrolSpeed;

	public SharedFloat customPatrolSpeed;

	public SharedFloat targetReachedDistance;

	private bool _isTargetFinded;

	private bool _fear;

	public SharedTransform pointToMove;

	public SharedBool stepBackAfterAttack;

	public SharedBool cutSceneUnlock;

	private GameController gameController;

	private const string ANIMATOR_VELOCITY_X = "VelocityX";

	private const string ANIMATOR_VELOCITY_Y = "VelocityY";

	private const string ANIMATOR_SPEED = "Speed";

	public override void OnStart()
	{
		if (gameController == null)
		{
			gameController = Object.FindObjectOfType<GameController>();
		}
		if (unitController.Value.PathFinder == null)
		{
			return;
		}
		unitController.Value.PathFinder.pickNextWaypointDist = 2f;
		unitController.Value.PathFinder.target = pointToMove.Value;
		if (moveType == MoveType.Walk)
		{
			unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
			if (moveTargetType.Value == SelectPointToMove.SelectMoveTargetType.PatrolPoint)
			{
				if (useCustomPatrolSpeed.Value)
				{
					unitController.Value.PathFinder.speed = customPatrolSpeed.Value;
				}
				else
				{
					unitController.Value.PathFinder.speed = unitController.Value.Model.WalkSpeed;
				}
			}
			else if (useCustomWalkSpeed.Value)
			{
				unitController.Value.PathFinder.speed = customWalkSpeed.Value;
			}
			else
			{
				unitController.Value.PathFinder.speed = unitController.Value.Model.WalkSpeed;
			}
			unitController.Value.Model.Walk();
		}
		else if (moveType == MoveType.Run && isEnemyReloadSlug.Value)
		{
			unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
			unitController.Value.PathFinder.speed = unitController.Value.Model.RunSpeed + 4f;
			unitController.Value.Model.Run();
		}
		else if (moveType == MoveType.Run)
		{
			unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
			unitController.Value.PathFinder.speed = unitController.Value.Model.RunSpeed;
			unitController.Value.Model.Run();
		}
		else if (moveType == MoveType.Backward)
		{
			unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Backward;
			unitController.Value.PathFinder.speed = unitController.Value.Model.BackwardSpeed;
			unitController.Value.Model.Walk();
		}
		if (useCustomDirection.Value)
		{
			if (enemyRoot.Value == null || enemyRoot.Value.transform == null)
			{
				unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
				if (useCustomWalkSpeed.Value)
				{
					unitController.Value.PathFinder.speed = customWalkSpeed.Value;
				}
				else
				{
					unitController.Value.PathFinder.speed = unitController.Value.Model.WalkSpeed;
				}
				unitController.Value.Model.Walk();
			}
			else
			{
				unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.CustomLookPoint;
				unitController.Value.PathFinder.CustomLookPoint = enemyRoot.Value.transform;
			}
		}
		else if (!stepBackAfterAttack.Value)
		{
			unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
		}
		unitController.Value.PathFinder.canMove = true;
		unitController.Value.PathFinder.canSearch = true;
		unitController.Value.PathFinder.SearchPath();
		if (unitController.Value.PathFinder.speed == 0f)
		{
			unitController.Value.PathFinder.speed = 2f;
		}
	}

	public override TaskStatus OnUpdate()
	{
		movingInProgress.Value = true;
		if (unitController.Value.PathFinder == null)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (pointToMove.Value == null)
		{
			pointToMove.Value = new GameObject("~pointToMove").transform;
		}
		if (!canMove.Value || stun.Value)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (gameController.IsPlayCutScene && !cutSceneUnlock.Value)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (isEnemyReloadSlug.Value && stopMoveIfEmenyReloadSlug.Value)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (isTargetFinded.Value && isEnemyReloadSlug.Value)
		{
			unitController.Value.Animator.SetBool("EnemyInSight", false);
		}
		else if (isTargetFinded.Value)
		{
			unitController.Value.Animator.SetBool("EnemyInSight", true);
		}
		if (useCustomDirection.Value || unitController.Value.PathFinder.MovementType == UnitController.UnitMovementType.CustomLookPoint)
		{
			if (attackTarget.Value != null && unitController.Value.PathFinder.CustomLookPoint != attackTarget.Value)
			{
				unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.CustomLookPoint;
				unitController.Value.PathFinder.CustomLookPoint = attackTarget.Value;
			}
			if (attackTarget.Value == null)
			{
				unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
				unitController.Value.PathFinder.CustomLookPoint = null;
			}
		}
		unitController.Value.Animator.SetFloat("VelocityX", unitController.Value.PathFinder.MoveVelocity.x);
		unitController.Value.Animator.SetFloat("VelocityY", unitController.Value.PathFinder.MoveVelocity.z);
		unitController.Value.Animator.SetFloat("Speed", unitController.Value.PathFinder.MoveVelocity.magnitude);
		if (!isAlive.Value)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (moveType == MoveType.Walk)
		{
			unitController.Value.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
			if (moveTargetType.Value == SelectPointToMove.SelectMoveTargetType.PatrolPoint)
			{
				if (useCustomPatrolSpeed.Value)
				{
					unitController.Value.PathFinder.speed = customPatrolSpeed.Value;
				}
				else
				{
					unitController.Value.PathFinder.speed = unitController.Value.Model.WalkSpeed;
				}
			}
			else if (useCustomWalkSpeed.Value)
			{
				unitController.Value.PathFinder.speed = customWalkSpeed.Value;
			}
			else
			{
				unitController.Value.PathFinder.speed = unitController.Value.Model.WalkSpeed;
			}
		}
		if (!fear.Value && isTargetFinded.Value != _isTargetFinded && !stepBackAfterAttack.Value)
		{
			_isTargetFinded = isTargetFinded.Value;
			unitController.Value.PathFinder.target.position = unitController.Value.Transform.position + unitController.Value.Transform.forward * 0.01f;
			float repathRate = unitController.Value.PathFinder.repathRate;
			unitController.Value.PathFinder.repathRate = 0f;
			unitController.Value.PathFinder.canMove = false;
			unitController.Value.PathFinder.repathRate = repathRate;
			DisablePathfinder();
			return TaskStatus.Success;
		}
		if (fear.Value != _fear)
		{
			_fear = fear.Value;
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		float num = (pointToMove.Value.position - unitController.Value.transform.position).sqrMagnitude;
		if (isTargetFinded.Value && !isRangeAttacker.Value && enemyRoot.Value.GetComponent<TeamController>() != null)
		{
			AnimalController animalController = unitController.Value as AnimalController;
			float num2 = Mathf.Pow(animalController.AttackDistance, 2f) + 0.1f;
			List<RaycastHit> list = Physics.RaycastAll(new Ray(unitController.Value.transform.position + Vector3.up, unitController.Value.transform.forward * Mathf.Sqrt(num2) * 100f), Mathf.Sqrt(num2) * 100f, unitController.Value.ViewArea.GetLayerMask()).ToList();
			int num3 = list.FindIndex((RaycastHit obj) => obj.transform == enemyRoot.Value);
			if (num3 >= 0)
			{
				num = Mathf.Pow(list[num3].distance, 2f);
				if (num < num2 && ((enemyRoot.Value.GetComponent<UnitPathFinder>() != null && enemyRoot.Value.GetComponent<UnitPathFinder>().MoveVelocity.magnitude < 0.01f) || enemyRoot.Value.GetComponent<UnitPathFinder>() == null || enemyRoot.Value.GetComponent<MoveState>() == null))
				{
					return TaskStatus.Success;
				}
			}
		}
		if (num <= targetReachedDistance.Value)
		{
			if (isTargetFinded.Value)
			{
				if (enemyRoot.Value != null && enemyRoot.Value.GetComponent<UnitPathFinder>() != null)
				{
					if (enemyRoot.Value.GetComponent<UnitPathFinder>().MoveVelocity.magnitude < 0.01f)
					{
						stepBackAfterAttack.Value = false;
						DisablePathfinder();
						return TaskStatus.Success;
					}
					return TaskStatus.Running;
				}
				stepBackAfterAttack.Value = false;
				DisablePathfinder();
				return TaskStatus.Success;
			}
			stepBackAfterAttack.Value = false;
			DisablePathfinder();
			return TaskStatus.Success;
		}
		return TaskStatus.Running;
	}

	private void DisablePathfinder()
	{
		movingInProgress.Value = false;
		isMoveOrderRecieved.Value = false;
		if (!(unitController.Value == null) && !(unitController.Value.Animator == null))
		{
			unitController.Value.Animator.SetFloat("VelocityX", 0f);
			unitController.Value.Animator.SetFloat("VelocityY", 0f);
			unitController.Value.Animator.SetFloat("Speed", 0f);
		}
	}
}
