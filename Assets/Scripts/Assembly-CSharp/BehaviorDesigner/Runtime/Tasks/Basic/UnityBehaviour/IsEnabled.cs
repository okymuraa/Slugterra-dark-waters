using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBehaviour
{
	[TaskDescription("Returns Success if the object is enabled, otherwise Failure.")]
	[TaskCategory("Basic/Behaviour")]
	public class IsEnabled : Conditional
	{
		[Tooltip("The Object to use")]
		public SharedObject specifiedObject;

		public override TaskStatus OnUpdate()
		{
			if (specifiedObject == null && !(specifiedObject.Value is Behaviour))
			{
				Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
				return TaskStatus.Failure;
			}
			return (!(specifiedObject.Value as Behaviour).enabled) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnReset()
		{
			if (specifiedObject != null)
			{
				specifiedObject.Value = null;
			}
		}
	}
}
