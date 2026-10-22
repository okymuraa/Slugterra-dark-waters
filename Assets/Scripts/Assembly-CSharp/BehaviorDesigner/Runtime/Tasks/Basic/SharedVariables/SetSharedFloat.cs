namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskDescription("Sets the SharedFloat variable to the specified object. Returns Success.")]
	[TaskCategory("Basic/SharedVariable")]
	public class SetSharedFloat : Action
	{
		[Tooltip("The value to set the SharedFloat to")]
		public SharedFloat targetValue;

		[RequiredField]
		[Tooltip("The SharedFloat to set")]
		public SharedFloat targetVariable;

		public override TaskStatus OnUpdate()
		{
			targetVariable.Value = targetValue.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetValue = 0f;
			targetVariable = 0f;
		}
	}
}
