using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
	[TaskDescription("Returns Success if the animation is currently playing.")]
	[TaskCategory("Basic/Animation")]
	public class IsPlaying : Conditional
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the animation")]
		public SharedString animationName;

		private Animation animation;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				animation = defaultGameObject.GetComponent<Animation>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			if (string.IsNullOrEmpty(animationName.Value))
			{
				return (!animation.isPlaying) ? TaskStatus.Failure : TaskStatus.Success;
			}
			return (!animation.IsPlaying(animationName.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			animationName.Value = string.Empty;
		}
	}
}
