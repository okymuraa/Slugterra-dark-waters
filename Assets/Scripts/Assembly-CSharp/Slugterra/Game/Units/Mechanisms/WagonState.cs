using AppsMinistry.Core.State;

namespace Slugterra.Game.Units.Mechanisms
{
	public class WagonState : MonoState
	{
		public WagonController Controller
		{
			get
			{
				return StateMachine.Controller as WagonController;
			}
		}
	}
}
