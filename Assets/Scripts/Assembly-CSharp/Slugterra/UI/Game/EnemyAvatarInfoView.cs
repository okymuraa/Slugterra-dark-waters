using AppsMinistry.UI;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class EnemyAvatarInfoView : UIView
	{
		[SerializeField]
		private tk2dSprite _avatarSprite;

		protected override void OnApplyModel(object model)
		{
			if (!(model is Humanoid))
			{
			}
		}

		public void SetAvatar(string Avatar)
		{
		}
	}
}
