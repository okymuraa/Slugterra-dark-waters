namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskDescription("Sets the SharedTransform variable to the specified object. Returns Success.")]
	[TaskCategory("Basic/SharedVariable")]
	public class SetSharedTransform : Action
	{
		[Tooltip("The value to set the SharedTransform to. If null the variable will be set to the current Transform")]
		public SharedTransform targetValue;

		[Tooltip("The SharedTransform to set")]
		[RequiredField]
		public SharedTransform targetVariable;

		public override TaskStatus OnUpdate()
		{
			targetVariable.Value = ((!(targetValue.Value != null)) ? transform : targetValue.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetValue = null;
			targetVariable = null;
		}
	}
}
