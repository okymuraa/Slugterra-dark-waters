using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskCategory("Basic/NavMeshAgent")]
	[TaskDescription("Gets the maximum turning speed in (deg/s) while following a path.. Returns Success.")]
	public class GetAngularSpeed : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The NavMeshAgent angular speed")]
		[SharedRequired]
		public SharedFloat storeValue;

		private NavMeshAgent navMeshAgent;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				navMeshAgent = defaultGameObject.GetComponent<NavMeshAgent>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (navMeshAgent == null)
			{
				Debug.LogWarning("NavMeshAgent is null");
				return TaskStatus.Failure;
			}
			storeValue.Value = navMeshAgent.angularSpeed;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeValue = 0f;
		}
	}
}
