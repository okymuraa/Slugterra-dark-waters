using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using UnityEngine;

[TaskCategory("Slugterra/MovePoint")]
public class AntiJamPoint : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override void OnStart()
	{
		NNInfo nearest = AstarData.active.GetNearest(GetObstacleAvoidPoint(), NNConstraint.Default);
		dataHolder.Value.Me.Movement.MovePoint.position = (Vector3)nearest.node.position;
		dataHolder.Value.Me.Movement.MovePoint.parent = null;
		dataHolder.Value.Me.MovementSettings.TargetReachedDistance = GetDefaultTargetReachedDistance();
		dataHolder.Value.Me.Movement.Speed = dataHolder.Value.Me.Controller.Model.RunSpeed;
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	private Vector3 GetObstacleAvoidPoint()
	{
		RaycastHit hitInfo;
		Physics.SphereCast(new Ray(dataHolder.Value.Me.Root.position, dataHolder.Value.Me.Root.forward), 0.4f, out hitInfo, 1f, 4096);
		RaycastHit hitInfo2;
		Physics.SphereCast(new Ray(dataHolder.Value.Me.Root.position, dataHolder.Value.Me.Root.right), 0.4f, out hitInfo2, 1f, 4096);
		RaycastHit hitInfo3;
		Physics.SphereCast(new Ray(dataHolder.Value.Me.Root.position, -dataHolder.Value.Me.Root.right), 0.4f, out hitInfo3, 1f, 4096);
		RaycastHit hitInfo4;
		Physics.SphereCast(new Ray(dataHolder.Value.Me.Root.position, -dataHolder.Value.Me.Root.forward), 0.4f, out hitInfo4, 1f, 4096);
		if (hitInfo.collider == null)
		{
			return dataHolder.Value.Me.Root.position + dataHolder.Value.Me.Root.forward;
		}
		if (hitInfo4.collider == null)
		{
			return dataHolder.Value.Me.Root.position - dataHolder.Value.Me.Root.forward;
		}
		if (hitInfo2.collider == null)
		{
			return dataHolder.Value.Me.Root.position + dataHolder.Value.Me.Root.right;
		}
		if (hitInfo3.collider == null)
		{
			return dataHolder.Value.Me.Root.position - dataHolder.Value.Me.Root.forward;
		}
		Debug.Log("avoid obstacle failed");
		return dataHolder.Value.Me.Root.position;
	}

	private float GetDefaultTargetReachedDistance()
	{
		return 0.26f;
	}
}
