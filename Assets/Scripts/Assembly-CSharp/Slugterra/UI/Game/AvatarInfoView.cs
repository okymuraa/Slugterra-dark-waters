using AppsMinistry.UI;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class AvatarInfoView : UIView
	{
		[SerializeField]
		private tk2dSprite _avatarSprite;

		protected override void OnApplyModel(object model)
		{
			if (model is Humanoid)
			{
				Humanoid humanoid = model as Humanoid;
				_avatarSprite.SetSprite(humanoid.Avatar);
			}
		}

		public void SetAvatar(string Avatar)
		{
			_avatarSprite.SetSprite(Avatar);
		}
	}
}
