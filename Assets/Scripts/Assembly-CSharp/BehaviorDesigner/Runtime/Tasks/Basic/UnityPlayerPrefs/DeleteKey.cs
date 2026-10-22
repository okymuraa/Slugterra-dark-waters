using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskDescription("Deletes the specified key from the PlayerPrefs.")]
	[TaskCategory("Basic/PlayerPrefs")]
	public class DeleteKey : Action
	{
		[Tooltip("The key to delete")]
		public SharedString key;

		public override TaskStatus OnUpdate()
		{
			PlayerPrefs.DeleteKey(key.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			key = string.Empty;
		}
	}
}
