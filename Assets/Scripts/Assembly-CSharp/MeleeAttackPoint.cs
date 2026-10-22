using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using Slugterra.Game.Units.Animals;
using UnityEngine;

[TaskCategory("Slugterra/MovePoint")]
public class MeleeAttackPoint : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override void OnStart()
	{
		NNInfo nearest = AstarData.active.GetNearest(MeleeChasePoint(), NNConstraint.Default);
		dataHolder.Value.Me.Movement.MovePoint.position = (Vector3)nearest.node.position;
		dataHolder.Value.Me.Movement.MovePoint.parent = GetAttackTargetParent();
		dataHolder.Value.Me.MovementSettings.TargetReachedDistance = GetDefaultTargetReachedDistance();
		dataHolder.Value.Me.Movement.Speed = dataHolder.Value.Me.Controller.Model.RunSpeed;
		dataHolder.Value.Me.Attack.AttackDistanceSQR = GetMeleeAttackDistance();
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	private Vector3 MeleeChasePoint()
	{
		Quaternion quaternion = Quaternion.LookRotation(dataHolder.Value.Enemy.Root.position - dataHolder.Value.Me.Root.position);
		float num = (dataHolder.Value.Me.Controller as AnimalController).AttackDistance * 0.4f;
		return dataHolder.Value.Enemy.Root.position - quaternion * Vector3.forward * num;
	}

	private Transform GetAttackTargetParent()
	{
		return (!dataHolder.Value.Enemy.Exist) ? null : dataHolder.Value.Enemy.Root;
	}

	private float GetMeleeAttackDistance()
	{
		return Mathf.Pow((dataHolder.Value.Me.Controller as AnimalController).AttackDistance, 2f);
	}

	private float GetDefaultTargetReachedDistance()
	{
		return 0.26f;
	}
}
