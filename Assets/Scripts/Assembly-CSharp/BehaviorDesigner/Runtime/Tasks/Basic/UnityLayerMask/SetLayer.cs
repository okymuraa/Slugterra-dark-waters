using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLayerMask
{
	[TaskDescription("Sets the layer of a GameObject.")]
	[TaskCategory("Basic/LayerMask")]
	public class SetLayer : Action
	{
		[Tooltip("The GameObject to set the layer of")]
		public SharedGameObject targetGameObject;

		[Tooltip("The name of the layer to set")]
		public SharedString layerName = "Default";

		public override TaskStatus OnUpdate()
		{
			GameObject defaultGameObject = GetDefaultGameObject(targetGameObject.Value);
			defaultGameObject.layer = LayerMask.NameToLayer(layerName.Value);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetGameObject = null;
			layerName = "Default";
		}
	}
}
