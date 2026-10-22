using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskDescription("Sets the value with the specified key from the PlayerPrefs.")]
	[TaskCategory("Basic/PlayerPrefs")]
	public class SetFloat : Action
	{
		[Tooltip("The key to store")]
		public SharedString key;

		[Tooltip("The value to set")]
		public SharedFloat value;

		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.SetFloat(key.Value, value.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			key = string.Empty;
			value = 0f;
		}
	}
}
