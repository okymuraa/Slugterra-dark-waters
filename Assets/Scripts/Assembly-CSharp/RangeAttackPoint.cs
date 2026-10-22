using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using Slugterra.Domain.Game.Units;
using Slugterra.UI.Game;
using UnityEngine;

[TaskCategory("Slugterra/MovePoint")]
public class RangeAttackPoint : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private GameController _gameController;

	private byte hackSide;

	public override void OnStart()
	{
		_gameController = Object.FindObjectOfType<GameController>();
		NNInfo nearest = AstarData.active.GetNearest(RangeChasePoint(), NNConstraint.Default);
		if (nearest.node != null)
		{
			dataHolder.Value.Me.Movement.MovePoint.position = (Vector3)nearest.node.position;
		}
		else
		{
			Debug.Log("<color=red>Get nearest node failed, nearest node is null</color>");
			dataHolder.Value.Me.Movement.MovePoint.position = RangeChasePoint();
		}
		if (_gameController == null)
		{
			_gameController = Object.FindObjectOfType<GameController>();
		}
		if (_gameController != null && _gameController.IsMultiplayer)
		{
			if (GetAttackTargetParent() != null)
			{
				Transform attackTargetParent = GetAttackTargetParent();
				Vector3 vector = attackTargetParent.forward * 2f;
				if (hackSide == 0)
				{
					vector = attackTargetParent.forward * 2f;
				}
				else if (hackSide == 1)
				{
					vector = attackTargetParent.forward * -2f;
				}
				else if (hackSide == 2)
				{
					vector = attackTargetParent.right * 2f;
				}
				else if (hackSide == 3)
				{
					vector = attackTargetParent.right * -2f;
				}
				hackSide++;
				if (hackSide == 4)
				{
					hackSide = 0;
				}
				Vector3 position = attackTargetParent.position + vector;
				dataHolder.Value.Me.Movement.MovePoint.position = position;
			}
			else
			{
				dataHolder.Value.Me.Movement.MovePoint.parent = ((!dataHolder.Value.Me.MovementSettings.AdvancedMovement) ? null : GetAttackTargetParent());
			}
		}
		dataHolder.Value.Me.MovementSettings.TargetReachedDistance = GetDefaultTargetReachedDistance();
		dataHolder.Value.Me.Movement.Speed = dataHolder.Value.Me.Controller.Model.RunSpeed;
		dataHolder.Value.Me.Attack.AttackDistanceSQR = GetRangeAttackDistance();
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	private float GetRangeAttackDistance()
	{
		return Mathf.Pow((dataHolder.Value.Me.Controller.Model as Humanoid).Blaster.Range, 2f) + 0.1f;
	}

	private Vector3 GetPointFromPatrolPoint()
	{
		return dataHolder.Value.Me.MovementSettings.PatrolPath.GetNextPathPoint().position;
	}

	private Vector3 RangeChasePoint()
	{
		if (!dataHolder.Value.Enemy.Exist)
		{
			return GetPointFromPatrolPoint();
		}
		float num = Random.Range(0.4f, 0.6f) * (dataHolder.Value.Me.Controller.Model as Humanoid).Blaster.Range;
		Quaternion quaternion = Quaternion.LookRotation(dataHolder.Value.Me.Root.position - dataHolder.Value.Enemy.Root.position) * Quaternion.AngleAxis(Random.Range(-15f, 15f), Vector3.up);
		return dataHolder.Value.Enemy.Root.position + quaternion * Vector3.forward * num;
	}

	private Transform GetAttackTargetParent()
	{
		return (!dataHolder.Value.Enemy.Exist) ? null : dataHolder.Value.Enemy.Root;
	}

	private float GetDefaultTargetReachedDistance()
	{
		return 0.26f;
	}
}
