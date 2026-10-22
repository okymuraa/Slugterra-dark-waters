using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Input.Window;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopItemsScrollController : MonoBehaviour
	{
		private class ControllerIndex
		{
			public ShopItemController item;

			public int index;
		}

		public const float DisabledSrollButtonAlpha = 0.333f;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<ShopItemController> OnObjectSelected__BackingField;

		[SerializeField]
		private ScrollController _scrollController;

		[SerializeField]
		private float startYPos;

		[SerializeField]
		private float yDist;

		[SerializeField]
		private float swipeSmoothFactor = 1f;

		[SerializeField]
		private tk2dButton _scrollUpButton;

		[SerializeField]
		private tk2dButton _scrollDownButton;

		[SerializeField]
		private InputWindowScrollView _inputWindowScrollView;

		[SerializeField]
		private tk2dSprite _scrollUpSprite;

		[SerializeField]
		private tk2dSprite _scrollDownSprite;

		[SerializeField]
		private List<ShopItemController> _items = new List<ShopItemController>();

		[SerializeField]
		private int lastIndexObject;

		public ScrollController Controller
		{
			get
			{
				return _scrollController;
			}
		}

		public tk2dButton ScrollUpButton
		{
			get
			{
				return _scrollUpButton;
			}
		}

		public tk2dButton ScrollDownButton
		{
			get
			{
				return _scrollDownButton;
			}
		}

		public event Action<ShopItemController> OnObjectSelected
		{
			add
			{
				Action<ShopItemController> action = OnObjectSelected__BackingField;
				Action<ShopItemController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnObjectSelected__BackingField, (Action<ShopItemController>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<ShopItemController> action = OnObjectSelected__BackingField;
				Action<ShopItemController> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnObjectSelected__BackingField, (Action<ShopItemController>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void Start()
		{
			if (!_scrollController)
			{
				_scrollController = gameObject.AddComponent<ScrollController>();
			}
			_scrollController.MaxValue = new Vector2(0f, _items.Count - 1);
			_scrollController.CurrentValue = Vector2.zero;
			_scrollController.PixelsBetweenValues = new Vector2(Screen.width, Screen.height) / 3f;
		}

		private void OnBtnScrollUpClick()
		{
			_scrollController.Shift(0f, -1f);
		}

		private void OnBtnScrollDownClick()
		{
			_scrollController.Shift(0f, 1f);
		}

		public void ApplyModel(List<ShopItemController> model)
		{
			if (model != null)
			{
				Clear();
				_items.AddRange(model);
				UpdateButtons();
			}
			_scrollController.MaxValue = new Vector2(0f, _items.Count - 1);
			_scrollController.CurrentValue = Vector2.zero;
			if (OnObjectSelected__BackingField != null)
			{
				lastIndexObject = _items.Count - 1;
				OnObjectSelected__BackingField(_items[_items.Count - 1]);
				UpdateButtons();
			}
		}

		public void SelectItemByName(string name)
		{
			if (OnObjectSelected__BackingField != null)
			{
				ControllerIndex controllerIndex = FindByName(name);
				if (controllerIndex != null)
				{
					OnObjectSelected__BackingField(controllerIndex.item);
					lastIndexObject = controllerIndex.index;
					_scrollController.FastGoto(new Vector2(0f, _items.Count - controllerIndex.index - 1));
				}
				else
				{
					OnObjectSelected__BackingField(_items[lastIndexObject]);
				}
				UpdateButtons();
			}
		}

		public void SelectItemByGroup(SlugGroup group)
		{
			if (OnObjectSelected__BackingField != null)
			{
				ControllerIndex controllerIndex = FindByGroup(group);
				if (controllerIndex != null)
				{
					OnObjectSelected__BackingField(controllerIndex.item);
					lastIndexObject = controllerIndex.index;
					_scrollController.FastGoto(new Vector2(0f, _items.Count - controllerIndex.index - 1));
				}
				else
				{
					OnObjectSelected__BackingField(_items[lastIndexObject]);
				}
				UpdateButtons();
			}
		}

		private ControllerIndex FindByName(string name)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				string text = null;
				if (_items[i].Model.GetType() == typeof(SlugItem))
				{
					SlugItem slugItem = _items[i].Model as SlugItem;
					text = slugItem.Type.ToString();
				}
				if (_items[i].Model.GetType() == typeof(AssistantItem))
				{
					AssistantItem assistantItem = _items[i].Model as AssistantItem;
					text = assistantItem.Type.ToString();
				}
				if (_items[i].Model.GetType() == typeof(BlasterItem))
				{
					BlasterItem blasterItem = _items[i].Model as BlasterItem;
					text = blasterItem.Type.ToString();
				}
				if (_items[i].Model.GetType() == typeof(FoeItem))
				{
					FoeItem foeItem = _items[i].Model as FoeItem;
					text = foeItem.Type;
				}
				if (!string.IsNullOrEmpty(text) && text == name)
				{
					ControllerIndex controllerIndex = new ControllerIndex();
					controllerIndex.item = _items[i];
					controllerIndex.index = i;
					return controllerIndex;
				}
			}
			return null;
		}

		private ControllerIndex FindByGroup(SlugGroup group)
		{
			for (int num = _items.Count - 1; num >= 0; num--)
			{
				if (_items[num].Model.GetType() == typeof(SlugItem))
				{
					SlugItem slugItem = _items[num].Model as SlugItem;
					SlugGroup slugGroup = SlugTypeHelper.GetGroup(slugItem.Type);
					if (group == slugGroup)
					{
						ControllerIndex controllerIndex = new ControllerIndex();
						controllerIndex.item = _items[num];
						controllerIndex.index = num;
						return controllerIndex;
					}
				}
			}
			return null;
		}

		private void Clear()
		{
			for (int i = 0; i < _items.Count; i++)
			{
				UnityEngine.Object.Destroy(_items[i].gameObject);
			}
			_items.Clear();
		}

		private void LateUpdate()
		{
			for (int i = 0; i < _items.Count; i++)
			{
				float y = _scrollController.SmoothValue.y;
				Vector3 position = _items[i].transform.position;
				float num = _scrollController.MaxValue.y - y;
				position.y = startYPos + (float)i * (yDist * swipeSmoothFactor) - num * swipeSmoothFactor * yDist;
				_items[i].transform.position = position;
				float num2 = 1.1f - Mathf.Clamp(Mathf.Abs((float)i - num), 0f, 0.2f);
				Vector3 localScale = new Vector3(num2, num2, num2) * 1.5f;
				float opacity = 1.1f - Mathf.Clamp(Mathf.Abs((float)i - num), 0f, 1f);
				int num3 = Mathf.Clamp(_items.Count - Mathf.RoundToInt(y) - 1, 0, _items.Count - 1);
				if (_items[i] != null && _items[i].ShopItemShaderProxy != null)
				{
					_items[i].ShopItemShaderProxy.SetOpacity(opacity);
				}
				if (1f - num2 < 0.01f && num3 != lastIndexObject)
				{
					lastIndexObject = num3;
					UpdateButtons();
					if (OnObjectSelected__BackingField != null)
					{
						OnObjectSelected__BackingField(_items[lastIndexObject]);
					}
				}
				_items[i].transform.localScale = localScale;
			}
		}

		private void UpdateButtons()
		{
			_scrollUpButton.gameObject.SetActive(_items.Count > 1);
			_scrollDownButton.gameObject.SetActive(_items.Count > 1);
			_scrollUpButton.enabled = lastIndexObject != _items.Count - 1;
			_scrollDownButton.enabled = lastIndexObject != 0;
			Color color = _scrollUpSprite.color;
			color.a = ((!_scrollUpButton.enabled) ? 0.333f : 1f);
			_scrollUpSprite.color = color;
			color.a = ((!_scrollDownButton.enabled) ? 0.333f : 1f);
			_scrollDownSprite.color = color;
			_inputWindowScrollView.SetGrowActive();
		}
	}
}
