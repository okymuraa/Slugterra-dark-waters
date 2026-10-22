using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCharacterController
{
	[TaskCategory("Basic/CharacterController")]
	[TaskDescription("Returns Success if the character is grounded, otherwise Failure.")]
	public class IsGrounded : Conditional
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		private CharacterController characterController;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				characterController = defaultGameObject.GetComponent<CharacterController>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (characterController == null)
			{
				Debug.LogWarning("CharacterController is null");
				return TaskStatus.Failure;
			}
			return (!characterController.isGrounded) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
		}
	}
}
