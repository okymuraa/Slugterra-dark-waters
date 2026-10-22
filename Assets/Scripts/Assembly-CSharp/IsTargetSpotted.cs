using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsTargetSpotted : Conditional
{
	public SharedBool isTargetFinded;

	public override TaskStatus OnUpdate()
	{
		if (isTargetFinded.Value)
		{
			return TaskStatus.Success;
		}
		return TaskStatus.Failure;
	}
}
