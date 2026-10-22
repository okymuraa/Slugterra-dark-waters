using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class InventoryItemView : UIView
	{
		public const float ItemDisabledButtonAlpha = 0.334f;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUseEvent__BackingField;

		[SerializeField]
		private tk2dButton _useButton;

		[SerializeField]
		private tk2dSlicedSprite _buttonSprite;

		[SerializeField]
		private tk2dSprite _iconSprite;

		[SerializeField]
		private TextMesh _count;

		[SerializeField]
		private tk2dUIProgressBar _duration;

		[SerializeField]
		private GameObject _frame;

		public event Action OnUseEvent
		{
			add
			{
				Action action = OnUseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnUseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public void OnBtnUseClick()
		{
			if (OnUseEvent__BackingField != null)
			{
				OnUseEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is ConsumableItem)
			{
				ConsumableItem consumableItem = model as ConsumableItem;
				_count.text = consumableItem.Count.ToString();
			}
		}

		public void SetState(InventoryItemState state)
		{
			switch (state)
			{
			case InventoryItemState.None:
			case InventoryItemState.Disabled:
				_buttonSprite.gameObject.GetComponent<MeshRenderer>().enabled = false;
				SetTransparency(0.334f);
				_useButton.enabled = false;
				_duration.gameObject.SetActive(false);
				break;
			case InventoryItemState.Enabled:
				_buttonSprite.gameObject.GetComponent<MeshRenderer>().enabled = true;
				SetTransparency(1f);
				_useButton.enabled = true;
				_duration.gameObject.SetActive(false);
				break;
			case InventoryItemState.Duration:
				_buttonSprite.gameObject.GetComponent<MeshRenderer>().enabled = true;
				SetTransparency(0.334f);
				_useButton.enabled = false;
				_duration.gameObject.SetActive(true);
				break;
			}
		}

		public void SetDurationProgress(float progress)
		{
			_duration.Value = progress;
		}

		private void SetTransparency(float value)
		{
			Color color = _buttonSprite.color;
			color.a = value;
			_buttonSprite.color = color;
			color = _iconSprite.color;
			color.a = value;
			_iconSprite.color = color;
			color = _count.color;
			color.a = value;
			_count.color = color;
		}
	}
}
