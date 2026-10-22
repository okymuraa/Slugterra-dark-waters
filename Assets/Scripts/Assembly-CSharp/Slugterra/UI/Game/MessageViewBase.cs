using AppsMinistry.Core;
using AppsMinistry.UI;
using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public abstract class MessageViewBase : UIView
	{
		public const float MoveAnimationTime = 0.35f;

		[SerializeField]
		protected TextMesh _description;

		[SerializeField]
		private Vector3 _hidePosition = new Vector3(-4f, 0f, -1f);

		[SerializeField]
		private Vector3 _showPosition = new Vector3(0f, 0f, -1f);

		protected bool _isShowing;

		private Tweener _tweener;

		public string Description
		{
			get
			{
				return _description.text;
			}
			set
			{
				_description.text = value;
			}
		}

		public bool IsShowing
		{
			get
			{
				return _isShowing;
			}
		}

		public void SetColorDescription(Color newColor)
		{
			_description.color = newColor;
		}

		private void Awake()
		{
			transform.localPosition = _hidePosition;
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_description);
			gameObject.SetActive(false);
		}

		public virtual void Show(bool value)
		{
			if (_isShowing == value)
			{
				return;
			}
			if (_tweener != null && !_tweener.IsComplete())
			{
				_tweener.Kill();
			}
			if (value)
			{
				_tweener = transform.DOLocalMove(_showPosition, 0.35f).OnStart(() =>
				{
					if (!gameObject.activeSelf)
					{
						gameObject.SetActive(true);
					}
				});
			}
			else
			{
				_tweener = transform.DOLocalMove(_hidePosition, 0.35f).OnComplete(() =>
				{
					if (gameObject.activeSelf)
					{
						gameObject.SetActive(false);
					}
				});
			}
			_isShowing = value;
		}
	}
}
