using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPlayerPrefs
{
	[TaskCategory("Basic/PlayerPrefs")]
	[TaskDescription("Deletes all entries from the PlayerPrefs.")]
	public class DeleteAll : Action
	{
		public override TaskStatus OnUpdate()
		{
			Debug.Log("BehaviorDesigner::DeleteAll == PlayerPrefs.DeleteAll()");
			return TaskStatus.Success;
		}
	}
}
