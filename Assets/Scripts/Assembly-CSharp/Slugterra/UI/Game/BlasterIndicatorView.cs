using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	internal class BlasterIndicatorView : UIView
	{
		[SerializeField]
		private tk2dUIProgressBar _calldownProgress;

		[SerializeField]
		private ProgressBarController _progressBar;

		[SerializeField]
		private tk2dBaseSprite[] _buttonElements;

		public const float SlugDisabledButtonAlpha = 0.334f;

		public void SetCalldownProgress(float value)
		{
			if (0f < value && value < 1f)
			{
				SetButtonTransparency(0.334f);
				if (_calldownProgress != null)
				{
					_calldownProgress.gameObject.SetActive(true);
					_calldownProgress.Value = value;
				}
				if (_progressBar != null)
				{
					_progressBar.SetProgress(value);
				}
			}
			else if (_calldownProgress != null && _calldownProgress.gameObject.activeSelf)
			{
				SetButtonTransparency(1f);
				_calldownProgress.gameObject.SetActive(false);
			}
		}

		private void SetButtonTransparency(float value)
		{
			tk2dBaseSprite[] buttonElements = _buttonElements;
			foreach (tk2dBaseSprite tk2dBaseSprite2 in buttonElements)
			{
				Color color = tk2dBaseSprite2.color;
				color.a = value;
				tk2dBaseSprite2.color = color;
			}
		}
	}
}
