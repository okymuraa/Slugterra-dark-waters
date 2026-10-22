using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using UnityEngine;

[TaskCategory("Slugterra/MovePoint")]
public class StepBackPoint : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override void OnStart()
	{
		NNInfo nearest = AstarData.active.GetNearest(GetStepBackPoint(), NNConstraint.Default);
		dataHolder.Value.Me.Movement.MovePoint.position = (Vector3)nearest.node.position;
		dataHolder.Value.Me.Movement.MovePoint.parent = null;
		dataHolder.Value.Me.MovementSettings.TargetReachedDistance = GetDefaultTargetReachedDistance();
		dataHolder.Value.Me.Movement.Speed = dataHolder.Value.Me.Controller.Model.RunSpeed;
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	private Vector3 GetStepBackPoint()
	{
		if (!dataHolder.Value.Enemy.Exist)
		{
			return GetPointFromPatrolPoint();
		}
		float num = 2f;
		return dataHolder.Value.Me.Root.position - dataHolder.Value.Me.Root.forward * num;
	}

	private Vector3 GetPointFromPatrolPoint()
	{
		return dataHolder.Value.Me.MovementSettings.PatrolPath.GetNextPathPoint().position;
	}

	private float GetDefaultTargetReachedDistance()
	{
		return 0.26f;
	}
}
