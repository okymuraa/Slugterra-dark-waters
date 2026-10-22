namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskDescription("Performs comparison between two floats: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
	[TaskCategory("Basic/Math")]
	public class FloatComparison : Conditional
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

		[Tooltip("The first float")]
		public SharedFloat float1;

		[Tooltip("The second float")]
		public SharedFloat float2;

		public override TaskStatus OnUpdate()
		{
			switch (operation)
			{
			case Operation.LessThan:
				return (!(float1.Value < float2.Value)) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.LessThanOrEqualTo:
				return (!(float1.Value <= float2.Value)) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.EqualTo:
				return (float1.Value != float2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.NotEqualTo:
				return (float1.Value == float2.Value) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.GreaterThanOrEqualTo:
				return (!(float1.Value >= float2.Value)) ? TaskStatus.Failure : TaskStatus.Success;
			case Operation.GreaterThan:
				return (!(float1.Value > float2.Value)) ? TaskStatus.Failure : TaskStatus.Success;
			default:
				return TaskStatus.Failure;
			}
		}

		public override void OnReset()
		{
			operation = Operation.LessThan;
			float1.Value = 0f;
			float2.Value = 0f;
		}
	}
}
