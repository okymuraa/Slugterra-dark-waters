using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityParticleSystem
{
	[TaskDescription("Sets if the Particle System should loop.")]
	[TaskCategory("Basic/ParticleSystem")]
	public class SetLoop : Action
	{
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		[Tooltip("Should the ParticleSystem loop?")]
		public SharedBool loop;

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
			particleSystem.loop = loop.Value;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			loop = false;
		}
	}
}
