using AppsMinistry.UI;
using Slugterra.Domain.App.Settings;
using Slugterra.Multiplayer.Lobby.Match;
using UnityEngine;

namespace Slugterra.UI.Multiplayer.Game
{
	public class PlayerSlugsViewController : UIView
	{
		[SerializeField]
		private SlugIconViewController _attackSlug;

		[SerializeField]
		private SlugIconViewController _defenceSlug;

		[SerializeField]
		private SlugIconViewController _specialSlug;

		protected override void OnApplyModel(object model)
		{
			if (model is PlayerData)
			{
				OnApplyModel(model as PlayerData);
			}
			if (model is MissionSetting)
			{
				OnApplyModel(model as MissionSetting);
			}
		}

		protected void OnApplyModel(PlayerData model)
		{
			_attackSlug.SetSlug(model.AttackSlug);
			_defenceSlug.SetSlug(model.ControlSlug);
			_specialSlug.SetSlug(model.SpecialSlug);
		}

		protected void OnApplyModel(MissionSetting model)
		{
			_attackSlug.SetSlug(model.AttackSlug);
			_defenceSlug.SetSlug(model.DefenceSlug);
			_specialSlug.SetSlug(model.SpecialSlug);
		}
	}
}
