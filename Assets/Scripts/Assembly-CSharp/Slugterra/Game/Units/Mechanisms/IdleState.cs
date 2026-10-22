namespace Slugterra.Game.Units.Mechanisms
{
	public class IdleState : WagonState
	{
		protected override void OnStart(object[] args)
		{
			Controller.PathFinder.speed = 0f;
			Controller.PathFinder.turningSpeed = 0f;
			Controller.PathFinder.canMove = false;
			Controller.PathFinder.target.position = Controller.Transform.position;
			Controller.Animator.SetFloat("Speed", Controller.PathFinder.speed);
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetFloat("Speed", Controller.PathFinder.speed);
			}
		}
	}
}
