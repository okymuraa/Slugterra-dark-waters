using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.UI.Game
{
	internal class BlasterIndicatorController : UIController<BlasterIndicatorView>
	{
		private Slug _model;

		private PlayerController _controller;

		protected override void OnStart()
		{
			base.OnStart();
			PlayerManager.OnAddLocalPlayer((PlayerController player) =>
			{
				_model = (player.Model as Humanoid).GetSlug(SlugType.Flopper);
				_controller = player;
			});
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_model != null && !(_controller == null))
			{
				if (_controller.NextFire > Time.time)
				{
					float calldownProgress = 1f - (_controller.NextFire - Time.time) / _model.CallDown;
					View.SetCalldownProgress(calldownProgress);
				}
				else
				{
					View.SetCalldownProgress(0f);
				}
			}
		}
	}
}
