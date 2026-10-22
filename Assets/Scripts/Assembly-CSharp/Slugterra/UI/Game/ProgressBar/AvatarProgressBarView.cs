using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game.ProgressBar
{
	public class AvatarProgressBarView : ProgressBarView
	{
		public enum viewBar
		{
			Horizontal = 0,
			Vertical = 1
		}

		[SerializeField]
		private viewBar ViewBar;

		[SerializeField]
		private tk2dTiledSprite _bar;

		[SerializeField]
		private float _width;

		[SerializeField]
		private float _step;

		protected override void OnStart()
		{
			if (ViewBar == viewBar.Horizontal)
			{
				_bar.dimensions = new Vector2(_width, _bar.dimensions.y);
			}
			if (ViewBar == viewBar.Vertical)
			{
				_bar.dimensions = new Vector2(_bar.dimensions.x, _width);
			}
		}

		private void OnEnable()
		{
			_bar.enabled = true;
		}

		private void OnDisable()
		{
			_bar.enabled = false;
		}

		protected override void OnProgressChanged(float value)
		{
			if (ViewBar == viewBar.Horizontal)
			{
				_bar.dimensions = new Vector2((float)Mathf.RoundToInt(_width * value / _step) * _step, _bar.dimensions.y);
			}
			if (ViewBar == viewBar.Vertical)
			{
				_bar.dimensions = new Vector2(_bar.dimensions.x, (float)Mathf.RoundToInt(_width * value / _step) * _step);
			}
		}
	}
}
