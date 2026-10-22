namespace Slugterra.Game.Units.Enemy
{
	public class IdleState : EnemyState
	{
		protected override void OnStart(object[] args)
		{
			if (!(Controller.PathFinder == null))
			{
				if (Controller.PathFinder.target != Controller.MoveTarget)
				{
					Controller.PathFinder.target = Controller.MoveTarget;
				}
				Controller.PathFinder.target.transform.position = Controller.Transform.position;
				Controller.Animator.SetFloat("VelocityX", 0f);
				Controller.Animator.SetFloat("VelocityY", 0f);
			}
		}
	}
}
