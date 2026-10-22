using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using UnityEngine;

[TaskCategory("Slugterra/MovePoint")]
public class LastKnownPoint : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override void OnStart()
	{
		NNInfo nearest = AstarData.active.GetNearest(LastKnownPos(), NNConstraint.Default);
		dataHolder.Value.Me.Movement.MovePoint.position = (Vector3)nearest.node.position;
		dataHolder.Value.Me.Movement.MovePoint.parent = null;
		dataHolder.Value.Me.MovementSettings.TargetReachedDistance = GetDefaultTargetReachedDistance();
		dataHolder.Value.Me.Movement.Speed = dataHolder.Value.Me.Controller.Model.RunSpeed;
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	public Vector3 LastKnownPos()
	{
		return dataHolder.Value.Enemy.LastKnownPos;
	}

	private float GetDefaultTargetReachedDistance()
	{
		return 0.26f;
	}
}
