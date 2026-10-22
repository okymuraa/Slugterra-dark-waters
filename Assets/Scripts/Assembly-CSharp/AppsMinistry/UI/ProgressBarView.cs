using UnityEngine;

namespace AppsMinistry.UI
{
	public class ProgressBarView : UIView
	{
		[SerializeField]
		private TextMesh _text;

		public TextMesh Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		protected override void OnApplyModel(object model)
		{
			if (model is Color)
			{
				OnProgressChanged((Color)model);
			}
			else
			{
				OnProgressChanged((float)model);
			}
		}

		protected virtual void OnProgressChanged(float value)
		{
		}

		protected virtual void OnProgressChanged(Color color)
		{
		}

		public void SetTextEnable(bool enabled)
		{
			_text.gameObject.SetActive(enabled);
		}

		public void SetText(string text)
		{
			_text.text = text;
		}
	}
}
