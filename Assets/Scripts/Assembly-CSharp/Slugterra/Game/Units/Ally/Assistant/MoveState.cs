using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	internal class MoveState : HumanoidState
	{
		private Vector3 targetPosition;

		protected override void OnStart(object[] args)
		{
			Setup((float)args[0], (Transform)args[2], (float)args[1]);
		}

		protected override void OnUpdate()
		{
			MoveAnimation();
			CheckDistance();
		}
	}
}
