using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityPhysics
{
	[TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
	[TaskCategory("Basic/Physics")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=117")]
	public class Linecast : Action
	{
		[Tooltip("The starting position of the linecast")]
		private SharedVector3 startPosition;

		[Tooltip("The ending position of the linecast")]
		private SharedVector3 endPosition;

		[Tooltip("Selectively ignore colliders.")]
		public LayerMask layerMask = -1;

		public override TaskStatus OnUpdate()
		{
			return (!Physics.Linecast(startPosition.Value, endPosition.Value, layerMask)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			startPosition = Vector3.zero;
			endPosition = Vector3.zero;
			layerMask = -1;
		}
	}
}
