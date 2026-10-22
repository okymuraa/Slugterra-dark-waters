namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
	[TaskDescription("Stores the GameObject tag. Returns Success.")]
	[TaskCategory("Basic/GameObject")]
	public class GetTag : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[RequiredField]
		[Tooltip("Active state of the GameObject")]
		public SharedString storeValue;

		public override TaskStatus OnUpdate()
		{
			storeValue.Value = GetDefaultGameObject(targetGameObject.Value).tag;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeValue = string.Empty;
		}
	}
}
