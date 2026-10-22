namespace BehaviorDesigner.Runtime.Tasks.Basic.SharedVariables
{
	[TaskDescription("Gets the GameObject from the Transform component. Returns Success.")]
	[TaskCategory("Basic/SharedVariable")]
	public class SharedTransformToGameObject : Action
	{
		[Tooltip("The Transform component")]
		public SharedTransform sharedTransform;

		[Tooltip("The GameObject to set")]
		[RequiredField]
		public SharedGameObject sharedGameObject;

		public override TaskStatus OnUpdate()
		{
			if (sharedTransform.Value == null)
			{
				return TaskStatus.Failure;
			}
			sharedGameObject.Value = sharedTransform.Value.gameObject;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			sharedTransform = null;
			sharedGameObject = null;
		}
	}
}
