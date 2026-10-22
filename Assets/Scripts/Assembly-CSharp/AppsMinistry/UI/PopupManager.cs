using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;

namespace AppsMinistry.UI
{
	public class PopupManager : DonDestroyMonoSingleton<PopupManager>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<PopupView> OnPopupClose__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<PopupView> OnPopupShow__BackingField;

		private Dictionary<Type, PopupView> _availablePopups = new Dictionary<Type, PopupView>();

		private LinkedList<PopupView> _popups = new LinkedList<PopupView>();

		public bool IsShowing
		{
			get
			{
				return _popups.Count > 0;
			}
		}

		public event Action<PopupView> OnPopupShow
		{
			add
			{
				Action<PopupView> action = OnPopupShow__BackingField;
				Action<PopupView> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPopupShow__BackingField, (Action<PopupView>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<PopupView> action = OnPopupShow__BackingField;
				Action<PopupView> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPopupShow__BackingField, (Action<PopupView>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<PopupView> OnPopupClose
		{
			add
			{
				Action<PopupView> action = OnPopupClose__BackingField;
				Action<PopupView> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPopupClose__BackingField, (Action<PopupView>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<PopupView> action = OnPopupClose__BackingField;
				Action<PopupView> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPopupClose__BackingField, (Action<PopupView>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public void RegisterPopup(PopupView popup)
		{
			if (popup != null)
			{
				_availablePopups[popup.GetType()] = popup;
			}
		}

		public void UnRegisterPopup(PopupView popup)
		{
			if (popup != null && _availablePopups.ContainsKey(popup.GetType()))
			{
				_availablePopups.Remove(popup.GetType());
			}
		}

		public PopupView GetRegisteredPopup(Type popupType)
		{
			PopupView value;
			if (_availablePopups.TryGetValue(popupType, out value))
			{
				return value;
			}
			return null;
		}

		public T GetRegisteredPopup<T>() where T : PopupView
		{
			PopupView registeredPopup = GetRegisteredPopup(typeof(T));
			if (registeredPopup != null)
			{
				return registeredPopup as T;
			}
			return (T)null;
		}

		public void OnShowPopup(PopupView popup)
		{
			if (popup != null)
			{
				_popups.AddLast(popup);
			}
			RaiseOnPopupShow(popup);
		}

		public void OnClosePopup(PopupView popup)
		{
			if (_popups.Count != 0 && popup != null)
			{
				_popups.Remove(popup);
			}
			RaiseOnPopupHide(popup);
		}

		public void CloseCurrentPopup()
		{
			if (_popups.Count != 0)
			{
				LinkedListNode<PopupView> last = _popups.Last;
				if (last != null)
				{
					last.Value.Close();
				}
			}
		}

		public PopupView GetCurrentPopup()
		{
			if (_popups.Count < 1)
			{
				return null;
			}
			return _popups.Last.Value;
		}

		public void Clear()
		{
			_popups.Clear();
		}

		private void RaiseOnPopupShow(PopupView view)
		{
			if (!(view == null) && OnPopupShow__BackingField != null)
			{
				OnPopupShow__BackingField(view);
			}
		}

		private void RaiseOnPopupHide(PopupView view)
		{
			if (!(view == null) && OnPopupClose__BackingField != null)
			{
				OnPopupClose__BackingField(view);
			}
		}
	}
}
