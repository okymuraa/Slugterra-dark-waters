using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskDescription("Sets the destination of the agent in world-space units. Returns Success if the destination is valid.")]
	[TaskCategory("Basic/NavMeshAgent")]
	public class SetDestination : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The NavMeshAgent destination")]
		[SharedRequired]
		public SharedVector3 destination;

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
			return (!navMeshAgent.SetDestination(destination.Value)) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			destination = Vector3.zero;
		}
	}
}
