using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class ConsumableItemView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBuyEvent__BackingField;

		[SerializeField]
		private tk2dButton _buyButton;

		[SerializeField]
		private TextMesh _buttonText;

		[SerializeField]
		private TextMesh _moneyCountText;

		public event Action OnBuyEvent
		{
			add
			{
				Action action = OnBuyEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBuyEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnEnable()
		{
			_buyButton.ButtonPressedEvent += OnBuy;
		}

		private void OnDisable()
		{
			_buyButton.ButtonPressedEvent -= OnBuy;
		}

		private void OnBuy(tk2dButton source)
		{
			if (OnBuyEvent__BackingField != null)
			{
				OnBuyEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
		}
	}
}
