using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
	[TaskDescription("Fades the animation over a period of time and fades other animations out. Returns Success.")]
	[TaskCategory("Basic/Animation")]
	public class CrossFade : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the animation")]
		public SharedString animationName;

		[Tooltip("The amount of time it takes to blend")]
		public float fadeLength = 0.3f;

		[Tooltip("The play mode of the animation")]
		public PlayMode playMode;

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
			animation.CrossFade(animationName.Value, fadeLength, playMode);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			animationName.Value = string.Empty;
			fadeLength = 0.3f;
			playMode = PlayMode.StopSameLayer;
		}
	}
}
