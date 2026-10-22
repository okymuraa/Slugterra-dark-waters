using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityGameObject
{
	[TaskDescription("Finds a GameObject by tag. Returns Success.")]
	[TaskCategory("Basic/GameObject")]
	public class FindGameObjectsWithTag : Action
	{
		[Tooltip("The tag of the GameObject to find")]
		public SharedString tag;

		[RequiredField]
		[Tooltip("The objects found by name")]
		public SharedGameObjectList storeValue;

		public override TaskStatus OnUpdate()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag.Value);
			for (int i = 0; i < array.Length; i++)
			{
				storeValue.Value.Add(array[i]);
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			tag.Value = null;
			storeValue.Value = null;
		}
	}
}
