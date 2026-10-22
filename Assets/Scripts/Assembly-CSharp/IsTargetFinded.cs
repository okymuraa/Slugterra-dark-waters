using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Enemy")]
public class IsTargetFinded : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Enemy.Exist) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
