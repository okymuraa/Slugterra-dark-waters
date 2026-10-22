using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
	[TaskCategory("Basic/ParticleSystem")]
	[TaskDescription("Stores if the Particle System is emitting particles.")]
	public class GetEnableEmission : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[RequiredField]
		[Tooltip("Is the Particle System emitting particles?")]
		public SharedBool storeResult;

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
			storeResult.Value = particleSystem.emission.enabled;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			storeResult = false;
		}
	}
}
