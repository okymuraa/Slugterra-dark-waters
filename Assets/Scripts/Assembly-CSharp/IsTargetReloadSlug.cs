using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Enemy")]
public class IsTargetReloadSlug : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Enemy.IsReloadSlug) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
