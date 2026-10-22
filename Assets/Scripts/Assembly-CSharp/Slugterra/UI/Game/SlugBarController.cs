using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SlugBarController : UIController<SlugBarView>
	{
		[SerializeField]
		private SlugButtonController _attackSlugButton;

		[SerializeField]
		private SlugButtonController _defenceSlugButton;

		[SerializeField]
		private SlugButtonController _specialSlugButton;

		private PlayerController _playerController;

		protected override void OnStart()
		{
			base.OnStart();
			PlayerManager.OnAddLocalPlayer((PlayerController p) =>
			{
				_playerController = p;
			});
		}

		protected override void _OnEnable()
		{
			_attackSlugButton.OnUseEvent += OnUseSlug;
			_defenceSlugButton.OnUseEvent += OnUseSlug;
			_specialSlugButton.OnUseEvent += OnUseSlug;
		}

		protected override void _OnDisable()
		{
			_attackSlugButton.OnUseEvent -= OnUseSlug;
			_defenceSlugButton.OnUseEvent -= OnUseSlug;
			_specialSlugButton.OnUseEvent -= OnUseSlug;
		}

		public void OnApplyModel(InGameModel model)
		{
			_attackSlugButton.ApplyModel(model.AttackSlug);
			_defenceSlugButton.ApplyModel(model.DefenceSlug);
			_specialSlugButton.ApplyModel(model.SpecialSlug);
		}

		private void OnUseSlug(SlugType slugType)
		{
			if (_playerController != null)
			{
				_playerController.OnUseSlug(slugType);
			}
		}
	}
}
