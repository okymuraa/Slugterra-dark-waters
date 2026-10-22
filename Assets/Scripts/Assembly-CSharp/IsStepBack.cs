using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Me")]
public class IsStepBack : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Me.Movement.StepBackAfterAttack) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
