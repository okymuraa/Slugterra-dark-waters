using AppsMinistry.Core.State;
using Slugterra.Domain.Game.Units.Mechanisms;
using Slugterra.Game.Wave;

namespace Slugterra.Game.Units.Mechanisms
{
	public class FernandoController : MechanismController
	{
		private Wagon _model;

		public override Mechanism Model
		{
			get
			{
				return _model;
			}
		}

		protected override void OnCreate()
		{
			StateMachine.NextState<IdleState>(new object[0]);
		}

		public override void Next(MonoState state)
		{
			if (!(state is IdleState))
			{
				StateMachine.NextState<IdleState>(new object[0]);
			}
		}

		public void OnMove(MovePath path)
		{
			StateMachine.ChangeState<MoveState>(new object[1] { path.Points });
		}

		protected override void OnModelChanged(Mechanism model)
		{
			base.OnModelChanged(model);
			if (_model.Health <= 0f)
			{
				StateMachine.ChangeState<DeathState>(new object[0]);
			}
		}

		public void SetModel(Wagon model)
		{
			_model = model;
		}
	}
}
