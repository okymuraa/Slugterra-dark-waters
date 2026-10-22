using AppsMinistry.Core.State;

namespace Slugterra.Game.Units.Player
{
	public class PlayerState : MonoState
	{
		public PlayerController Controller
		{
			get
			{
				return StateMachine.Controller as PlayerController;
			}
		}
	}
}
