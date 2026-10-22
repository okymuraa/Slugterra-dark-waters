using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody2D
{
	[TaskDescription("Stores the position of the Rigidbody2D. Returns Success.")]
	[TaskCategory("Basic/Rigidbody2D")]
	public class GetPosition : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Can the target GameObject be empty?")]
		public SharedBool allowEmptyTarget;

		[RequiredField]
		[Tooltip("The velocity of the Rigidbody2D")]
		public SharedVector2 storeValue;

		private Rigidbody2D rigidbody2D;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			if (!allowEmptyTarget.Value)
			{
				GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
				if (defaultGameObject != prevGameObject)
				{
					rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
					prevGameObject = defaultGameObject;
				}
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (rigidbody2D == null)
			{
				Debug.LogWarning("Rigidbody2D is null");
				return TaskStatus.Failure;
			}
			storeValue.Value = rigidbody2D.position;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			allowEmptyTarget = false;
			storeValue = Vector2.zero;
		}
	}
}
