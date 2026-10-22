using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI
{
	public class BadgeIconView : UIView
	{
		[SerializeField]
		private TextMesh _label;

		[SerializeField]
		private tk2dBaseSprite _background;

		public string Label
		{
			get
			{
				return _label.text;
			}
			set
			{
				_label.text = value;
			}
		}
	}
}
