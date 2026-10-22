namespace Slugterra.Game.Units.Ally.Assistant
{
	public class FollowerState : HumanoidState
	{
		protected new FollowerController Controller
		{
			get
			{
				return StateMachine.Controller as FollowerController;
			}
		}
	}
}
