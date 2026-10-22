using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopInappTabView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnGiftsEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnFreeBonusEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Type> OnTabEvent__BackingField;

		[SerializeField]
		private FreeBonusButton _freeBonusBtn;

		[SerializeField]
		private GameObject _panelBackAndroid;

		[SerializeField]
		private GameObject _panelBackOther;

		public FreeBonusButton FreeBonustBtn
		{
			get
			{
				return _freeBonusBtn;
			}
		}

		public event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnGiftsEvent
		{
			add
			{
				Action action = OnGiftsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGiftsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnGiftsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGiftsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnPlayEvent
		{
			add
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnFreeBonusEvent
		{
			add
			{
				Action action = OnBtnFreeBonusEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnFreeBonusEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnFreeBonusEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnFreeBonusEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Type> OnTabEvent
		{
			add
			{
				Action<Type> action = OnTabEvent__BackingField;
				Action<Type> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTabEvent__BackingField, (Action<Type>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Type> action = OnTabEvent__BackingField;
				Action<Type> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTabEvent__BackingField, (Action<Type>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			_panelBackAndroid.SetActive(true);
			_panelBackOther.SetActive(false);
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnBtnGiftsClick()
		{
			if (OnGiftsEvent__BackingField != null)
			{
				OnGiftsEvent__BackingField();
			}
		}

		private void OnBtnPreviewClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(EliPreviewTabController));
			}
		}

		private void OnBtnWeaponClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(BlastersTabController));
			}
		}

		private void OnBtnSlugsClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(SlugsTabController));
			}
		}

		private void OnBtnArmorClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(ArmorsTabController));
			}
		}

		private void OnBtnAssistantsClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(AssistantsTabController));
			}
		}

		private void OnBtnBoostsClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(BoostsTabController));
			}
		}

		private void OnBtnFoesClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(FoesTabController));
			}
		}

		private void OnBtnRecommendedClick()
		{
		}

		private void OnBtnStartClick()
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnBtnFreeBonus()
		{
			if (OnBtnFreeBonusEvent__BackingField != null)
			{
				OnBtnFreeBonusEvent__BackingField();
			}
		}
	}
}
