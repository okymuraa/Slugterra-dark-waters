using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Slugterra/Me")]
public class IsAnnounceRecieved : Conditional
{
	public BehaviorDataHolderSharedVariable dataHolder;

	public override TaskStatus OnUpdate()
	{
		if (dataHolder.Value.Me.Movement.IsMoveOrderRecieved)
		{
			dataHolder.Value.Me.Movement.Speed = ((!dataHolder.Value.Me.MovementSettings.UseCustomWalkSpeed) ? dataHolder.Value.Me.Controller.Model.WalkSpeed : dataHolder.Value.Me.MovementSettings.CustomWalkSpeed);
			return TaskStatus.Success;
		}
		return TaskStatus.Failure;
	}
}
