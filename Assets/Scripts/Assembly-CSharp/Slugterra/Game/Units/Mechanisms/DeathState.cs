namespace Slugterra.Game.Units.Mechanisms
{
	public class DeathState : WagonState
	{
		protected override void OnStart(object[] args)
		{
			Controller.Animator.SetTrigger("Death");
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetTrigger("Death");
			}
			Controller.PathFinder.enabled = false;
			Controller.enabled = false;
		}
	}
}
