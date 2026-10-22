using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Me")]
public class IsRangeAttacker : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Me.AttackSettings.IsRangeAttacker) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
