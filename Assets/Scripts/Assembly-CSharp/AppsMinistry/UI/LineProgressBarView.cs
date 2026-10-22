using UnityEngine;

namespace AppsMinistry.UI
{
	public class LineProgressBarView : ProgressBarView
	{
		[SerializeField]
		private tk2dSlicedSprite _background;

		[SerializeField]
		private tk2dSlicedSprite _foreground;

		[SerializeField]
		private float _width;

		[SerializeField]
		private bool _isColored;

		[SerializeField]
		private Color _color;

		protected override void OnStart()
		{
			_foreground.dimensions = new Vector2(_width, _foreground.dimensions.y);
			_background.dimensions = new Vector2(_width, _background.dimensions.y);
			if (_isColored)
			{
				_foreground.color = _color;
			}
		}

		private void OnEnable()
		{
			_foreground.enabled = true;
			_background.enabled = true;
		}

		private void OnDisable()
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				if (_foreground != null)
				{
					_foreground.enabled = false;
				}
				if (_background != null)
				{
					_background.enabled = false;
				}
			}
		}

		protected override void OnProgressChanged(float value)
		{
			_foreground.dimensions = new Vector2(_width * value, _foreground.dimensions.y);
			if (_isColored)
			{
				_foreground.color = _color;
			}
			else
			{
				_foreground.color = new Color(1f * (1f - value), 1f * value, 0f, 1f);
			}
		}

		protected override void OnProgressChanged(Color color)
		{
			_color = color;
			_foreground.color = color;
		}
	}
}
