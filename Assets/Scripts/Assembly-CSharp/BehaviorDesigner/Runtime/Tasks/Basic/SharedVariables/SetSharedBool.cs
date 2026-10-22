namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskDescription("Sets the SharedBool variable to the specified object. Returns Success.")]
	[TaskCategory("Basic/SharedVariable")]
	public class SetSharedBool : Action
	{
		[Tooltip("The value to set the SharedBool to")]
		public SharedBool targetValue;

		[Tooltip("The SharedBool to set")]
		[RequiredField]
		public SharedBool targetVariable;

		public override TaskStatus OnUpdate()
		{
			targetVariable.Value = targetValue.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetValue = false;
			targetVariable = false;
		}
	}
}
