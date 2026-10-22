using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Enemy")]
public class IsTargetLost : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Enemy.IsTargetLost) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
