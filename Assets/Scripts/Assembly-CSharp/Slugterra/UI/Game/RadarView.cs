using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class RadarView : UIView
	{
		[SerializeField]
		private Transform _detectLine;

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			_detectLine.eulerAngles = Vector3.forward * (float)model;
		}
	}
}
