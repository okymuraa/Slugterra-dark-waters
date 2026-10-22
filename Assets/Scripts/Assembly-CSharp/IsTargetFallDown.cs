using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Enemy")]
public class IsTargetFallDown : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Enemy.IsFallDown) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
