using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskCategory("Basic/Animator")]
	[TaskDescription("Returns success if the specified name matches the name of the active state.")]
	public class IsName : Conditional
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The layer's index")]
		public SharedInt index;

		[Tooltip("The state name to compare")]
		public SharedString name;

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
			return (!animator.GetCurrentAnimatorStateInfo(index.Value).IsName(name.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			index = 0;
			name = string.Empty;
		}
	}
}
