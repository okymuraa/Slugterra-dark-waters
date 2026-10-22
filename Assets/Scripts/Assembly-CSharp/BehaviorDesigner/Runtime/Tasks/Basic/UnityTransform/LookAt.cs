using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityTransform
{
	[TaskDescription("Rotates the transform so the forward vector points at worldPosition. Returns Success.")]
	[TaskCategory("Basic/Transform")]
	public class LookAt : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Point to look at")]
		public SharedVector3 worldPosition;

		[Tooltip("Vector specifying the upward direction")]
		public Vector3 worldUp;

		private Transform targetTransform;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				targetTransform = defaultGameObject.GetComponent<Transform>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (targetTransform == null)
			{
				Debug.LogWarning("Transform is null");
				return TaskStatus.Failure;
			}
			targetTransform.LookAt(worldPosition.Value, worldUp);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			worldPosition = Vector3.up;
			worldUp = Vector3.up;
		}
	}
}
