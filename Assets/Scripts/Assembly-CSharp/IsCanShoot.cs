using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Me")]
public class IsCanShoot : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		return (!dataHolder.Value.Me.AttackSettings.CanShoot) ? TaskStatus.Failure : TaskStatus.Success;
	}
}
