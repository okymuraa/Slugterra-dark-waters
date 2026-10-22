using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;

namespace Slugterra.UI.Popup
{
	public class PopupAnnouncementView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		protected AnnouncementContents _content;

		protected Action<AnnounceStatus> _onClose;

		protected bool _isBadgeOffer;

		public event Action OnCloseEvent
		{
			add
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public virtual void Open(AnnouncementContents content, Action<AnnounceStatus> onClose, bool isBadge = false)
		{
			gameObject.SetActive(true);
			_content = content;
			_onClose = onClose;
			_isBadgeOffer = isBadge;
		}

		public virtual void Open(AnnouncementContents content, bool isBadge = true)
		{
			gameObject.SetActive(true);
			_content = content;
			_isBadgeOffer = isBadge;
		}

		public override void Close()
		{
			OnBtnCloseClick();
		}

		protected virtual void OnBtnCloseClick()
		{
			_onClose(AnnounceStatus.Ignored);
			OnClose();
		}

		protected void OnClose()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
		}
	}
}
