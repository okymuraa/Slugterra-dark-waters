namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskDescription("Sets the SharedGameObject variable to the specified object. Returns Success.")]
	[TaskCategory("Basic/SharedVariable")]
	public class SetSharedGameObject : Action
	{
		[Tooltip("The value to set the SharedGameObject to. If null the variable will be set to the current GameObject")]
		public SharedGameObject targetValue;

		[Tooltip("The SharedGameObject to set")]
		[RequiredField]
		public SharedGameObject targetVariable;

		[Tooltip("Can the target value be null?")]
		public SharedBool valueCanBeNull;

		public override TaskStatus OnUpdate()
		{
			targetVariable.Value = ((!(targetValue.Value != null) && !valueCanBeNull.Value) ? gameObject : targetValue.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			valueCanBeNull = false;
			targetValue = null;
			targetVariable = null;
		}
	}
}
