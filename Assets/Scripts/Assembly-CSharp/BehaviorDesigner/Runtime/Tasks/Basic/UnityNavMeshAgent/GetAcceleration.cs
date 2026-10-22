using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityNavMeshAgent
{
	[TaskCategory("Basic/NavMeshAgent")]
	[TaskDescription("Gets the maximum acceleration of an agent as it follows a path, given in units / sec^2.. Returns Success.")]
	public class GetAcceleration : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("The NavMeshAgent acceleration")]
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
			storeValue.Value = navMeshAgent.acceleration;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeValue = 0f;
		}
	}
}
