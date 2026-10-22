namespace Slugterra.Game.Units.Animals.SlugScorpion
{
	public class IdleState : SlugScorpionState
	{
		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			Controller.Animator.SetFloat("Speed", 0f);
			Controller.MoveTarget.position = Controller.Transform.position;
			Controller.PathFinder.enabled = false;
		}
	}
}
