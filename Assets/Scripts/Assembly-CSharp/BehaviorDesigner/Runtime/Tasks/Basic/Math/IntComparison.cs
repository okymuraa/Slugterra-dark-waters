namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskCategory("Basic/Math")]
	[TaskDescription("Performs comparison between two integers: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
	public class IntComparison : Conditional
	{
		public enum Operation
		{
			LessThan = 0,
			LessThanOrEqualTo = 1,
			EqualTo = 2,
			NotEqualTo = 3,
			GreaterThanOrEqualTo = 4,
			GreaterThan = 5
		}

		[Tooltip("The operation to perform")]
		public Operation operation;

		[Tooltip("The first integer")]
		public SharedInt integer1;

		[Tooltip("The second integer")]
		public SharedInt integer2;

		public override TaskStatus OnUpdate()
		{
			switch (operation)
			{
			case Operation.LessThan:
				return (integer1.Value >= integer2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.LessThanOrEqualTo:
				return (integer1.Value > integer2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.EqualTo:
				return (integer1.Value != integer2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.NotEqualTo:
				return (integer1.Value == integer2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.GreaterThanOrEqualTo:
				return (integer1.Value < integer2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.GreaterThan:
				return (integer1.Value <= integer2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			default:
				return TaskStatus.Failure;
			}
		}

		public override void OnReset()
		{
			operation = Operation.LessThan;
			integer1.Value = 0;
			integer2.Value = 0;
		}
	}
}
