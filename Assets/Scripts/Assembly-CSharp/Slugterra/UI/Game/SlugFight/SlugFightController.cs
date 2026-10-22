using AppsMinistry.UI;
using Slugterra.Game.Duel;

namespace Slugterra.UI.Game.SlugFight
{
	public class SlugFightController : UIController<SlugFightView>
	{
		private SlugFightState _model;

		protected override void _OnEnable()
		{
			View.OnPauseEvent += OnPause;
			View.OnAttackEvent += OnAttack;
		}

		protected override void _OnDisable()
		{
			View.OnPauseEvent -= OnPause;
			View.OnAttackEvent -= OnAttack;
		}

		public void ApplyModel(SlugFightState model)
		{
			_model = model;
			View.ApplyModel(_model);
		}

		private void OnPause()
		{
		}

		private void OnAttack()
		{
			if (_model != null)
			{
				_model.Attack();
			}
		}
	}
}
