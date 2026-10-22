namespace Slugterra.Game.Units.Ally.Assistant
{
	internal class IdleState : FollowerState
	{
		protected override void OnStart(object[] args)
		{
			Setup(0f, Controller.Transform, 0f, 0f, false);
			Controller.Animator.SetFloat("VelocityX", 0f);
			Controller.Animator.SetFloat("VelocityY", 0f);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (Controller.AllyArea.enabled && !Controller.AllyArea.Items.Contains(Controller.PlayerTransform))
			{
				Finish();
			}
		}
	}
}
