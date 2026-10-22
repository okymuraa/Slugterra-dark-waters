using AppsMinistry.Core.State;

namespace Slugterra.Game.Duel
{
	public abstract class DuelState : MonoState
	{
		public DuelManager Controller
		{
			get
			{
				return StateMachine.Controller as DuelManager;
			}
		}
	}
}
