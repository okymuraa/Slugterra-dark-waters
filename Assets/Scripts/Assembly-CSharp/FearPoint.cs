using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using UnityEngine;

[TaskCategory("Slugterra/MovePoint")]
public class FearPoint : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private Vector3 fearPoint = Vector3.zero;

	public override void OnAwake()
	{
		if (fearPoint == Vector3.zero)
		{
			CalculateFearPoint();
		}
	}

	public override void OnStart()
	{
		NNInfo nearest = AstarData.active.GetNearest(GetFearPoint(), new NNConstraint
		{
			constrainWalkability = true,
			walkable = true,
			constrainDistance = false
		});
		dataHolder.Value.Me.Movement.MovePoint.position = (Vector3)nearest.node.position;
		dataHolder.Value.Me.Movement.MovePoint.parent = null;
		dataHolder.Value.Me.MovementSettings.TargetReachedDistance = GetDefaultTargetReachedDistance();
		dataHolder.Value.Me.Movement.Speed = dataHolder.Value.Me.Controller.Model.RunSpeed;
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	private void CalculateFearPoint()
	{
		if (!(dataHolder.Value.Me.Controller == null) && !(dataHolder.Value.Me.Controller.Transform == null))
		{
			if (dataHolder.Value.Enemy.Root != null)
			{
				FleePath fleePath = FleePath.Construct(dataHolder.Value.Me.Controller.Transform.position, dataHolder.Value.Enemy.Root.position, 10000, FearPointCalculated);
				fleePath.spread = 2000;
				fleePath.aimStrength = 0.5f;
				AstarPath.StartPath(fleePath);
			}
			else
			{
				RandomPath randomPath = RandomPath.Construct(dataHolder.Value.Me.Controller.Transform.position, 10000, FearPointCalculated);
				randomPath.spread = 2000;
				randomPath.aimStrength = 0.5f;
				randomPath.aim = Vector3.zero;
				AstarPath.StartPath(randomPath);
			}
		}
	}

	private void FearPointCalculated(Path p)
	{
		fearPoint = ((RandomPath)p).endPoint;
	}

	private Vector3 GetFearPoint()
	{
		Vector3 vector = fearPoint;
		if (vector == Vector3.zero && dataHolder.Value.Enemy.Exist)
		{
			Quaternion quaternion = Quaternion.LookRotation(dataHolder.Value.Me.Controller.transform.position - dataHolder.Value.Enemy.Root.position, Vector3.up);
			vector = dataHolder.Value.Me.Controller.transform.position + (quaternion * Vector3.forward).normalized * 30f;
		}
		else if (vector == Vector3.zero)
		{
			Quaternion quaternion2 = dataHolder.Value.Me.Controller.transform.rotation * Quaternion.AngleAxis(180f, Vector3.up);
			vector = dataHolder.Value.Me.Controller.transform.position + (quaternion2 * Vector3.forward).normalized * 30f;
		}
		return vector;
	}

	private Transform GetNullParent()
	{
		return null;
	}

	private float GetDefaultTargetReachedDistance()
	{
		return 0.26f;
	}
}
