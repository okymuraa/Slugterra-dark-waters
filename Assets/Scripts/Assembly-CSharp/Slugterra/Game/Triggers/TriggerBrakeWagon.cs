using Slugterra.Game.Units.Mechanisms;
using UnityEngine;

namespace Slugterra.Game.Triggers
{
	internal class TriggerBrakeWagon : Trigger
	{
		[SerializeField]
		private WagonController _controller;

		private Transform _transform;

		protected override void OnStart()
		{
			base.OnStart();
			_transform = transform;
		}

		protected override void OnTrigger()
		{
			_controller.PushBrake();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			_transform.position = _controller.Transform.position;
			_transform.rotation = _controller.Transform.rotation;
		}
	}
}
