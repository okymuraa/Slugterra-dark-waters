using AppsMinistry.Core.State;

namespace Slugterra.Game.Units.Enemy.Deadweed
{
	public class DeadweedState : MonoState
	{
		public DeadweedController Controller
		{
			get
			{
				return StateMachine.Controller as DeadweedController;
			}
		}
	}
}
