using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
	[TaskDescription("Stores the rotation of the Rigidbody. Returns Success.")]
	[TaskCategory("Basic/Rigidbody")]
	public class GetRotation : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[RequiredField]
		[Tooltip("The rotation of the Rigidbody")]
		public SharedQuaternion storeValue;

		private Rigidbody rigidbody;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				rigidbody = defaultGameObject.GetComponent<Rigidbody>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (rigidbody == null)
			{
				Debug.LogWarning("Rigidbody is null");
				return TaskStatus.Failure;
			}
			storeValue.Value = rigidbody.rotation;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeValue = Quaternion.identity;
		}
	}
}
