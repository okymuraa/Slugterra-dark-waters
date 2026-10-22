using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units.Enemy;
using UnityEngine;

public class IsTargetClearlyVisible : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		if (dataHolder.Value.Enemy.Root == null || dataHolder.Value.Me.Controller == null)
		{
			return TaskStatus.Failure;
		}
		if (!dataHolder.Value.Me.AttackSettings.IsRangeAttacker)
		{
			return TaskStatus.Success;
		}
		Transform shotPoint = (dataHolder.Value.Me.Controller as EnemyController).ShotPoint;
		RaycastHit hitInfo;
		Physics.SphereCast(new Ray(shotPoint.position - shotPoint.forward * 0.2f, dataHolder.Value.Enemy.AttackTargetPoint.position - shotPoint.position), 0.2f, out hitInfo, (dataHolder.Value.Enemy.AttackTargetPoint.position - shotPoint.position).magnitude + 0.2f, 36864);
		if (hitInfo.collider == null || hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("PropLayer"))
		{
			return TaskStatus.Success;
		}
		return TaskStatus.Failure;
	}
}
