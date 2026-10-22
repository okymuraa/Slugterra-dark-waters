using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
	[TaskDescription("Destorys the specified GameObject immediately. Returns Success.")]
	[TaskCategory("Basic/GameObject")]
	public class DestroyImmediate : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		public override TaskStatus OnUpdate()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			Object.DestroyImmediate(defaultGameObject);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
		}
	}
}
