using UnityEngine;

namespace AppsMinistry.UI
{
	public class ProgressBarController : UIController<ProgressBarView>
	{
		private bool _showText;

		private float _currentProgress;

		private float _progress;

		private Color _color;

		private float _velocity;

		[SerializeField]
		private float _smothTime = 0.3f;

		public bool ShowText
		{
			get
			{
				return _showText;
			}
			set
			{
				_showText = value;
				View.SetTextEnable(_showText);
			}
		}

		public string SetText
		{
			set
			{
				View.SetText(value);
			}
		}

		private float CurrentProgress
		{
			set
			{
				_currentProgress = value;
				if (_currentProgress < 0f)
				{
					_currentProgress = 0f;
				}
				View.ApplyModel(_currentProgress);
			}
		}

		public float Progress
		{
			get
			{
				return _progress;
			}
			set
			{
				_progress = Mathf.Min(value, 1f);
			}
		}

		public Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
				View.ApplyModel(_color);
			}
		}

		protected override void OnStart()
		{
			_velocity = 0f;
		}

		protected override void _OnEnable()
		{
			View.enabled = true;
		}

		protected override void _OnDisable()
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				View.enabled = false;
			}
		}

		protected override void OnUpdate()
		{
			if (Mathf.Abs(_currentProgress - _progress) > 1E-05f)
			{
				CurrentProgress = Mathf.SmoothDamp(_currentProgress, _progress, ref _velocity, _smothTime);
			}
		}

		public void SetProgress(float value, bool animation = true)
		{
			_progress = Mathf.Min(value, 1f);
			if (!animation)
			{
				CurrentProgress = _progress;
			}
		}
	}
}
