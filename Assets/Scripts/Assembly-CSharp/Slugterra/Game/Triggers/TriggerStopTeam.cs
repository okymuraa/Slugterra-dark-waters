using Slugterra.Game.Units.Mechanisms.Spline;
using UnityEngine;

namespace Slugterra.Game.Triggers
{
	internal class TriggerStopTeam : Trigger
	{
		[SerializeField]
		private TeamController _controller;

		protected override void OnTrigger()
		{
			_controller.OnStop();
		}
	}
}
