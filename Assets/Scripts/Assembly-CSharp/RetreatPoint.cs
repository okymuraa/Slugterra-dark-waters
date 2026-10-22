using AppsMinistry.Core;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using Slugterra.Game;
using UnityEngine;

[TaskCategory("Slugterra/MovePoint")]
public class RetreatPoint : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override void OnStart()
	{
		NNInfo nearest = AstarData.active.GetNearest(GetRetreatPoint(), NNConstraint.Default);
		dataHolder.Value.Me.Movement.MovePoint.position = (Vector3)nearest.node.position;
		dataHolder.Value.Me.Movement.MovePoint.parent = null;
		dataHolder.Value.Me.MovementSettings.TargetReachedDistance = GetDefaultTargetReachedDistance();
		dataHolder.Value.Me.Movement.Speed = dataHolder.Value.Me.Controller.Model.RunSpeed;
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	private Vector3 GetRetreatPoint()
	{
		Transform transform = dataHolder.Value.Me.Controller.Transform;
		Transform root = dataHolder.Value.Enemy.Root;
		CoverNode optimalNode = MonoSingleton<CoverSystem>.Instance.GetOptimalNode(transform, root, 20f);
		if (optimalNode != null)
		{
			return (!(root != null)) ? GetPointFromPatrolPoint() : optimalNode.GetNextPathPoint(transform.position, 20f).position;
		}
		if (dataHolder.Value.Me.MovementSettings.CanPatrol && dataHolder.Value.Me.MovementSettings.PatrolPath != null)
		{
			return GetPointFromPatrolPoint();
		}
		Debug.LogWarning("Retreat failed");
		return dataHolder.Value.Me.Root.position;
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
