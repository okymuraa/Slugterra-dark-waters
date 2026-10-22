using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class RadarItemView : UIView
	{
		[SerializeField]
		private tk2dSprite _pointSprite;

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			float num = (float)model;
			num = 0.5f + num * 0.5f;
			_pointSprite.color = new Color(1f, 1f, 1f, num);
			_pointSprite.scale = Vector3.one * 1.5f * num;
		}
	}
}
