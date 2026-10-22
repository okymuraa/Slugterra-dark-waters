using Slugterra.Game.Units.Mechanisms.Spline;
using UnityEngine;

namespace Slugterra.Game.Triggers
{
	internal class TriggerBrakeTeam : Trigger
	{
		[SerializeField]
		private TeamController _controller;

		protected override void OnStart()
		{
			base.OnStart();
			Mode = TriggerMode.OnStay;
		}

		protected override void OnTrigger()
		{
			_controller.PushBrake();
		}
	}
}
