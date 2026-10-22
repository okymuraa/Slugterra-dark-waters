using BehaviorDesigner.Runtime.Tasks;

public class AnimatorMovementCheck : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private const string ANIMATOR_VELOCITY_X = "VelocityX";

	private const string ANIMATOR_VELOCITY_Y = "VelocityY";

	private const string ANIMATOR_SPEED = "Speed";

	public override TaskStatus OnUpdate()
	{
		if (dataHolder.Value.Me.Effects.InStun)
		{
			dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityX", 0f);
			dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityY", 0f);
			dataHolder.Value.Me.Controller.Animator.SetFloat("Speed", 0f);
			return TaskStatus.Running;
		}
		if (dataHolder.Value.Me.Controller != null)
		{
			dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityX", dataHolder.Value.Me.Controller.PathFinder.MoveVelocity.x);
			dataHolder.Value.Me.Controller.Animator.SetFloat("VelocityY", dataHolder.Value.Me.Controller.PathFinder.MoveVelocity.z);
			dataHolder.Value.Me.Controller.Animator.SetFloat("Speed", dataHolder.Value.Me.Controller.PathFinder.MoveVelocity.magnitude);
			return TaskStatus.Running;
		}
		return TaskStatus.Failure;
	}
}
