using Slugterra.Configuration;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class TrixieController : AssistantController
	{
		private TrixieStrategy _strategy;

		public TrixieStrategy Strategy
		{
			get
			{
				return _strategy;
			}
			set
			{
				_strategy = value;
			}
		}

		protected override void OnCreate()
		{
			Type = AllyType.Trixie;
			base.OnCreate();
		}

		protected override void OnStart()
		{
			base.OnStart();
			_strategy = new TrixieStrategy(this);
			_strategy.OnStart();
		}

		protected override void OnUpdate()
		{
			if (!Model.IsDead())
			{
				base.OnUpdate();
				_strategy.OnUpdate();
			}
		}
	}
}
