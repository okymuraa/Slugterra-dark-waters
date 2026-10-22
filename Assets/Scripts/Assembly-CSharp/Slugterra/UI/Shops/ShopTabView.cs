using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopTabView : UIView
	{
		[SerializeField]
		private ShopItemsScrollController _shopItemsScroll;

		private Vector2 _lastScrollValue = Vector2.zero;

		private int _lastUnlockedValue;

		[SerializeField]
		private GameObject _badge;

		public ShopItemsScrollController ShopItemsScrollScontroller
		{
			get
			{
				return _shopItemsScroll;
			}
		}

		public Vector2 LastScrollValue
		{
			get
			{
				return _lastScrollValue;
			}
			set
			{
				_lastScrollValue = value;
			}
		}

		public int LastUnlockedValue
		{
			get
			{
				return _lastUnlockedValue;
			}
			set
			{
				_lastUnlockedValue = value;
			}
		}

		public GameObject Badge
		{
			get
			{
				return _badge;
			}
		}

		protected virtual void OnDisable()
		{
			if (_shopItemsScroll != null)
			{
				_lastScrollValue = _shopItemsScroll.Controller.CurrentValue;
			}
		}

		public void GotoLastScrollValue()
		{
			if (_shopItemsScroll != null)
			{
				if (_lastUnlockedValue != 0)
				{
					_shopItemsScroll.Controller.CurrentValue = new Vector2(0f, _lastUnlockedValue);
					_lastUnlockedValue = 0;
				}
				else
				{
					_shopItemsScroll.Controller.FastGoto(_lastScrollValue);
				}
			}
		}
	}
}
