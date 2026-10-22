using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Me")]
public class IsInStun : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Me.Effects.InStun) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
