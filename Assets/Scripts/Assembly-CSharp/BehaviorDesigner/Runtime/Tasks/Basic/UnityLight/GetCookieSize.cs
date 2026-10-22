using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
	[TaskDescription("Stores the light's cookie size.")]
	[TaskCategory("Basic/Light")]
	public class GetCookieSize : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The size to store")]
		[RequiredField]
		public SharedFloat storeValue;

		private Light light;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				light = defaultGameObject.GetComponent<Light>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (light == null)
			{
				Debug.LogWarning("Light is null");
				return TaskStatus.Failure;
			}
			storeValue = light.cookieSize;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeValue = 0f;
		}
	}
}
