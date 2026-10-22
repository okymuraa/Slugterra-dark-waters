using System.Collections.Generic;
using AppsMinistry.UI;
using Slugterra.UI.Shops;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	public class InputWindowScrollView : InputWindowView
	{
		[SerializeField]
		private InputEvent _scrollUpEvent;

		[SerializeField]
		private InputEvent _scrollDownEvent;

		[SerializeField]
		private ScrollController _scrollController;

		[SerializeField]
		private ShopItemsScrollController _scrollItemsController;

		[SerializeField]
		private GameObject _glowDownButton;

		[SerializeField]
		private GameObject _glowUpButton;

		private bool _activeView;

		public override void SelectElement(LinkedListNode<InputWindowElement> element)
		{
			_activeView = true;
			base.SelectElement(element);
			SetGrowActive();
		}

		public override LinkedListNode<InputWindowElement> GetLastSelectedElement()
		{
			SetGrowActive();
			return base.GetLastSelectedElement();
		}

		public void SetGrowActive()
		{
			if (_glowDownButton != null && _activeView)
			{
				_glowDownButton.SetActive(_scrollItemsController.ScrollDownButton.enabled);
			}
			if (_glowUpButton != null && _activeView)
			{
				_glowUpButton.SetActive(_scrollItemsController.ScrollUpButton.enabled);
			}
		}

		public override void DisableAllSelections()
		{
			_activeView = false;
			base.DisableAllSelections();
			_glowDownButton.SetActive(false);
			_glowUpButton.SetActive(false);
		}

		public override void UseAction(InputEvent e)
		{
			base.UseAction(e);
			if (_scrollController.allowInput)
			{
				if (e == _scrollUpEvent)
				{
					_scrollController.Shift(0f, -1f);
				}
				else if (e == _scrollDownEvent)
				{
					_scrollController.Shift(0f, 1f);
				}
			}
		}

		public override bool IsActive()
		{
			return _scrollController.transform.childCount > 1 && gameObject.activeSelf && enabled && gameObject.activeInHierarchy;
		}
	}
}
