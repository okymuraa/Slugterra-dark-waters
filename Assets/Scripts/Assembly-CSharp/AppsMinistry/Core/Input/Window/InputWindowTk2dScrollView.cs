using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	public class InputWindowTk2dScrollView : InputWindowView
	{
		internal class ScrollBounds
		{
			public float Top { get; set; }

			public float Down { get; set; }

			public ScrollBounds(float top, float down)
			{
				Top = top;
				Down = down;
			}

			public bool InBounds(float value)
			{
				if (value >= Top && value <= Down)
				{
					return true;
				}
				return false;
			}
		}

		[SerializeField]
		private InputWindowTk2dScrollFacade scrollParent;

		private ScrollBounds _bounds;

		[SerializeField]
		private float _viewContentSize;

		[SerializeField]
		private float _scrollPercent;

		private void SetSelfBoundValue()
		{
			scrollParent = GetComponentInParent<InputWindowTk2dScrollFacade>();
			if (scrollParent == null)
			{
				return;
			}
			_bounds = new ScrollBounds(0f, scrollParent.ScrollableArea.VisibleAreaLength);
			int num = 0;
			foreach (Transform item in scrollParent.ScrollableArea.contentContainer.transform)
			{
				if (item == transform)
				{
					_scrollPercent = (float)num * _viewContentSize / scrollParent.ScrollableArea.ContentLength;
					break;
				}
				num++;
			}
		}

		public override void SelectElement(InputWindowElement element)
		{
			base.SelectElement(element);
			if (scrollParent == null)
			{
				SetSelfBoundValue();
			}
			if (scrollParent != null)
			{
				scrollParent.TweenScrollTo(_scrollPercent);
				scrollParent.LastSelectedView = this;
			}
		}

		public override void DisableSelection()
		{
			base.DisableSelection();
			if (_elements == null)
			{
				return;
			}
			foreach (InputWindowElement element in _elements)
			{
				if (element != null)
				{
					element.SetState(false);
				}
			}
		}
	}
}
