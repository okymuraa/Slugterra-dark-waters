using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Me")]
public class IsCanMove : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Me.MovementSettings.CanMove) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
