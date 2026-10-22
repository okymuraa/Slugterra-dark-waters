using AppsMinistry.Core.State;

namespace Slugterra.Game.Units.Mechanisms.Spline
{
	public class TeamState : MonoState
	{
		public TeamController Controller
		{
			get
			{
				return StateMachine.Controller as TeamController;
			}
		}
	}
}
