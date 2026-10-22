using Slugterra.Configuration;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class ProntoController : AssistantController
	{
		private ProntoStrategy _strategy;

		public ProntoStrategy Strategy
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
			Type = AllyType.Pronto;
			base.OnCreate();
		}

		protected override void OnStart()
		{
			base.OnStart();
			_strategy = new ProntoStrategy(this);
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
