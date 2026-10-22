using AppsMinistry.UI;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BoostIconView : UIView
	{
		public const float IconDisabledAlpha = 0.334f;

		[SerializeField]
		private TextMesh _count;

		[SerializeField]
		private tk2dSprite _iconSprite;

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is ConsumableItem)
			{
				ConsumableItem consumableItem = model as ConsumableItem;
				SetCount(consumableItem.Count);
			}
		}

		private void SetCount(int value)
		{
			_count.text = value.ToString();
			SetTransparency((value <= 0) ? 0.334f : 1f);
		}

		private void SetTransparency(float value)
		{
			Color color = _iconSprite.color;
			color.a = value;
			_iconSprite.color = color;
			color = _count.color;
			color.a = value;
			_count.color = color;
		}
	}
}
