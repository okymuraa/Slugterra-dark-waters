using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
	[TaskCategory("Basic/Rigidbody")]
	[TaskDescription("Stores the center of mass of the Rigidbody. Returns Success.")]
	public class GetCenterOfMass : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[RequiredField]
		[Tooltip("The center of mass of the Rigidbody")]
		public SharedVector3 storeValue;

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
			storeValue.Value = rigidbody.centerOfMass;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeValue = Vector3.zero;
		}
	}
}
