using AppsMinistry.Core.State;

namespace Slugterra.Game.Units.Enemy
{
	public class EnemyState : MonoState
	{
		public EnemyController Controller
		{
			get
			{
				return StateMachine.Controller as EnemyController;
			}
		}
	}
}
