using Slugterra.Configuration;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class KordController : AssistantController
	{
		private KordStrategy _strategy;

		public KordStrategy Strategy
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
			Type = AllyType.Kord;
			base.OnCreate();
		}

		protected override void OnStart()
		{
			base.OnStart();
			_strategy = new KordStrategy(this);
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
