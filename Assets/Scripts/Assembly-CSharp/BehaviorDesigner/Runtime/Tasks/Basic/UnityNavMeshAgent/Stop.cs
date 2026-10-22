using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskCategory("Basic/NavMeshAgent")]
	[TaskDescription("Stop movement of this agent along its current path. Returns Success.")]
	public class Stop : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

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
#if UNITY_2017_1_OR_NEWER
			navMeshAgent.isStopped = true;
#else
			navMeshAgent.Stop();
#endif
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
		}
	}
}
