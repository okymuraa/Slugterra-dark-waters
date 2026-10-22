using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
	[TaskDescription("Stores the gravity modifier of the Particle System.")]
	[TaskCategory("Basic/ParticleSystem")]
	public class GetGravityModifier : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[RequiredField]
		[Tooltip("The gravity modifier of the ParticleSystem")]
		public SharedFloat storeResult;

		private ParticleSystem particleSystem;

		private GameObject prevGameObject;

		public override void OnStart()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			if (defaultGameObject != prevGameObject)
			{
				particleSystem = defaultGameObject.GetComponent<ParticleSystem>();
				prevGameObject = defaultGameObject;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (particleSystem == null)
			{
				Debug.LogWarning("ParticleSystem is null");
				return TaskStatus.Failure;
			}
			storeResult.Value = particleSystem.gravityModifier;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeResult = 0f;
		}
	}
}
