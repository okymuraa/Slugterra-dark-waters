using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.Multiplayer.Lobby.Match;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Multiplayer.Game
{
	public class SlugIconViewController : UIView
	{
		[SerializeField]
		private tk2dSprite _slugIcon;

		[SerializeField]
		private TextMesh _badge;

		public void SetSlug(SlugType slugType, int level)
		{
			_slugIcon.gameObject.SetActive(true);
			_slugIcon.SetSprite(SpriteNamesHelper.GetSlugSpriteName(slugType));
			_badge.text = TextUtils.GetSlugLevel(level);
		}

		public void SetSlug(SlugItem slugItem)
		{
			if (slugItem != null)
			{
				SetSlug(slugItem.Type, slugItem.Level);
			}
			else
			{
				Clear();
			}
		}

		public void SetSlug(PlayerDataSlug slug)
		{
			if (!slug.IsEmpty())
			{
				SetSlug(slug.Slug, slug.Level);
			}
			else
			{
				Clear();
			}
		}

		public void Clear()
		{
			_slugIcon.gameObject.SetActive(false);
			_badge.text = string.Empty;
		}
	}
}
