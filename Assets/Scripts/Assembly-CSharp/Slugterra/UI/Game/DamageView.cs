using System.Collections.Generic;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class DamageView : UIView
	{
		[SerializeField]
		private tk2dSlicedSprite _upSprite;

		[SerializeField]
		private tk2dSlicedSprite _lowerSprite;

		[SerializeField]
		private tk2dSlicedSprite _leftSprite;

		[SerializeField]
		private tk2dSlicedSprite _rightSprite;

		private List<tk2dSlicedSprite> damageSprites;

		public tk2dSlicedSprite UpSprite
		{
			get
			{
				return _upSprite;
			}
		}

		public tk2dSlicedSprite LowerSprite
		{
			get
			{
				return _lowerSprite;
			}
		}

		public tk2dSlicedSprite LeftSprite
		{
			get
			{
				return _leftSprite;
			}
		}

		public tk2dSlicedSprite RightSprite
		{
			get
			{
				return _rightSprite;
			}
		}

		public tk2dSlicedSprite[] GetAllSprites()
		{
			if (damageSprites == null)
			{
				damageSprites = new List<tk2dSlicedSprite> { _upSprite, _lowerSprite, _rightSprite, _leftSprite };
			}
			return damageSprites.ToArray();
		}
	}
}
