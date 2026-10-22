namespace Slugterra.Game.Units.Player
{
	public class IdleState : PlayerState
	{
		protected override void OnStart(object[] args)
		{
			if ((bool)Controller.PathFinder)
			{
				Controller.PathFinder.target.position = Controller.Transform.position;
				Controller.PathFinder.target.gameObject.SetActive(false);
				Controller.PathFinder.speed = 0f;
				Controller.PathFinder.canMove = false;
			}
			Controller.Animator.SetFloat("VelocityX", 0f);
			Controller.Animator.SetFloat("VelocityY", 0f);
		}
	}
}
