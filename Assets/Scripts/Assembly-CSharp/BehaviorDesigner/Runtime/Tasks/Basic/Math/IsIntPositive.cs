namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskDescription("Is the int a positive value?")]
	[TaskCategory("Basic/Math")]
	public class IsIntPositive : Conditional
	{
		[Tooltip("The int to check if positive")]
		public SharedInt intVariable;

		public override TaskStatus OnUpdate()
		{
			return (intVariable.Value <= 0) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			intVariable = 0;
		}
	}
}
