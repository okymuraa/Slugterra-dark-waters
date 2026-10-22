using BehaviorDesigner.Runtime.Tasks;

public class IsInRangeOfAttack : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		if (!dataHolder.Value.Enemy.Exist || dataHolder.Value.Me.Controller == null)
		{
			return TaskStatus.Failure;
		}
		float sqrMagnitude = (dataHolder.Value.Enemy.Root.position - dataHolder.Value.Me.Root.position).sqrMagnitude;
		if (sqrMagnitude < dataHolder.Value.Me.Attack.AttackDistanceSQR)
		{
			return TaskStatus.Success;
		}
		return TaskStatus.Failure;
	}
}
