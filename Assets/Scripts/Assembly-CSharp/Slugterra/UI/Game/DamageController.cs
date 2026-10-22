using AppsMinistry.UI;
using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class DamageController : UIController<DamageView>
	{
		[SerializeField]
		private float timeIn = 0.3f;

		[SerializeField]
		private float timeOut = 0.3f;

		public bool IsDamaged
		{
			set
			{
				tk2dSlicedSprite[] allSprites = View.GetAllSprites();
				if (value)
				{
					return;
				}
				for (int i = 0; i < allSprites.Length; i++)
				{
					if (allSprites[i] != null)
					{
						allSprites[i].DOFade(0f, timeOut).SetUpdate(true).OnComplete(() =>
						{
						});
					}
				}
			}
		}

		protected override void _OnEnable()
		{
			tk2dSlicedSprite[] allSprites = View.GetAllSprites();
			for (int i = 0; i < allSprites.Length; i++)
			{
				if (allSprites[i] != null)
				{
					Color color = allSprites[i].color;
					color.a = 0f;
					allSprites[i].color = color;
				}
			}
		}

		public void Damage()
		{
			tk2dSlicedSprite[] allSprites = View.GetAllSprites();
			IsDamaged = true;
			for (int i = 0; i < allSprites.Length; i++)
			{
				if (allSprites[i] != null)
				{
					allSprites[i].DOKill();
					allSprites[i].DOFade(0.5f, timeIn).SetUpdate(true).OnComplete(() =>
					{
						IsDamaged = false;
					});
				}
			}
		}
	}
}
