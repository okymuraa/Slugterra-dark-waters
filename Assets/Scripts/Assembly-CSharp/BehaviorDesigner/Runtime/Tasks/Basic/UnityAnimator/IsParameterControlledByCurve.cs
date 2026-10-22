using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
	[TaskDescription("Returns success if the specified parameter is controlled by an additional curve on an animation.")]
	[TaskCategory("Basic/Animator")]
	public class IsParameterControlledByCurve : Conditional
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

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
			return (!animator.IsParameterControlledByCurve(paramaterName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			paramaterName = string.Empty;
		}
	}
}
