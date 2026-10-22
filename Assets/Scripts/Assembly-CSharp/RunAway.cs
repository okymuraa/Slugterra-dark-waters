using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units;
using Slugterra.UI.Game;
using UnityEngine;

[TaskCategory("Slugterra/Movement")]
public class RunAway : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

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
		if (!(dataHolder.Value.Me.Controller.PathFinder == null))
		{
			dataHolder.Value.Me.Controller.PathFinder.pickNextWaypointDist = 2f;
			dataHolder.Value.Me.Controller.PathFinder.target = dataHolder.Value.Me.Movement.MovePoint;
			dataHolder.Value.Me.Controller.PathFinder.MovementType = UnitController.UnitMovementType.Forward;
			dataHolder.Value.Me.Controller.PathFinder.speed = dataHolder.Value.Me.Controller.Model.RunSpeed + 1.5f;
			dataHolder.Value.Me.Controller.Model.Run();
			dataHolder.Value.Me.Controller.PathFinder.canMove = true;
			dataHolder.Value.Me.Controller.PathFinder.canSearch = true;
			dataHolder.Value.Me.Controller.PathFinder.SearchPath();
			if (dataHolder.Value.Me.Controller.PathFinder.speed == 0f)
			{
				dataHolder.Value.Me.Controller.PathFinder.speed = 2f;
			}
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (dataHolder.Value.Me.Controller.PathFinder == null)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (!dataHolder.Value.Me.MovementSettings.CanMove || dataHolder.Value.Me.Effects.InStun)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (gameController.IsPlayCutScene && !dataHolder.Value.Me.UnitSettings.CutSceneUnlock)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (!dataHolder.Value.Me.UnitSettings.IsALive)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		if (!dataHolder.Value.Me.Effects.InFear && !dataHolder.Value.Enemy.IsReloadSlug)
		{
			DisablePathfinder();
			return TaskStatus.Failure;
		}
		dataHolder.Value.Me.Controller.Animator.SetBool("EnemyInSight", false);
		dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityX", dataHolder.Value.Me.Controller.PathFinder.MoveVelocity.x);
		dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityY", dataHolder.Value.Me.Controller.PathFinder.MoveVelocity.z);
		dataHolder.Value.Me.Controller.Animator.SetFloat("Speed", dataHolder.Value.Me.Controller.PathFinder.MoveVelocity.magnitude);
		float sqrMagnitude = (dataHolder.Value.Me.Movement.MovePoint.position - dataHolder.Value.Me.Root.position).sqrMagnitude;
		if (sqrMagnitude <= dataHolder.Value.Me.MovementSettings.TargetReachedDistance)
		{
			if (dataHolder.Value.Enemy.Exist)
			{
				if (dataHolder.Value.Enemy.Root != null && dataHolder.Value.Enemy.Root.GetComponent<UnitPathFinder>() != null)
				{
					if (dataHolder.Value.Enemy.Root.GetComponent<UnitPathFinder>().MoveVelocity.magnitude < 0.01f)
					{
						dataHolder.Value.Me.Movement.StepBackAfterAttack = false;
						DisablePathfinder();
						return TaskStatus.Success;
					}
					return TaskStatus.Running;
				}
				dataHolder.Value.Me.Movement.StepBackAfterAttack = false;
				DisablePathfinder();
				return TaskStatus.Success;
			}
			dataHolder.Value.Me.Movement.StepBackAfterAttack = false;
			DisablePathfinder();
			return TaskStatus.Success;
		}
		return TaskStatus.Running;
	}

	private void DisablePathfinder()
	{
		dataHolder.Value.Me.MovementSettings.IsMoveOrderRecieved = false;
		if (!(dataHolder.Value.Me.Controller == null) && !(dataHolder.Value.Me.Controller.Animator == null))
		{
			dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityX", 0f);
			dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityY", 0f);
			dataHolder.Value.Me.Controller.Animator.SetFloat("Speed", 0f);
		}
	}
}
