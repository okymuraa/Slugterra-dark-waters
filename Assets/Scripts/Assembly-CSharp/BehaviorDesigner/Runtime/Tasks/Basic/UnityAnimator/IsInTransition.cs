using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskDescription("Returns success if the specified AnimatorController layer in a transition.")]
	[TaskCategory("Basic/Animator")]
	public class IsInTransition : Conditional
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The layer's index")]
		public SharedInt index;

		private Animator animator;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				animator = defaultGameObject.GetComponent<Animator>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			return (!animator.IsInTransition(index.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			index = 0;
		}
	}
}
