using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupConfirmOfferIgnoringView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Action> OnDeclineEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Action> OnAcceptEvent__BackingField;

		[SerializeField]
		private TextMesh _titleText;

		[SerializeField]
		private TextMesh _descriptionText;

		protected Action _accept;

		protected Action _decline;

		public event Action<Action> OnDeclineEvent
		{
			add
			{
				Action<Action> action = OnDeclineEvent__BackingField;
				Action<Action> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDeclineEvent__BackingField, (Action<Action>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Action> action = OnDeclineEvent__BackingField;
				Action<Action> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDeclineEvent__BackingField, (Action<Action>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Action> OnAcceptEvent
		{
			add
			{
				Action<Action> action = OnAcceptEvent__BackingField;
				Action<Action> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAcceptEvent__BackingField, (Action<Action>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Action> action = OnAcceptEvent__BackingField;
				Action<Action> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAcceptEvent__BackingField, (Action<Action>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		protected override void OnDisable()
		{
			_accept = (_decline = null);
			base.OnDisable();
		}

		private void OnBtnDeclineClick()
		{
			if (OnDeclineEvent__BackingField != null)
			{
				OnDeclineEvent__BackingField(_decline);
			}
		}

		private void OnBtnAcceptClick()
		{
			if (OnAcceptEvent__BackingField != null)
			{
				OnAcceptEvent__BackingField(_accept);
			}
		}

		public void Show(string titleLocalizationKey, string descriptionLocalizationKey, Action onAccept, Action onDecline)
		{
			gameObject.SetActive(true);
			if (!string.IsNullOrEmpty(titleLocalizationKey))
			{
				_titleText.text = MonoSingleton<Localization>.Instance.GetValueByKey(titleLocalizationKey);
			}
			if (!string.IsNullOrEmpty(descriptionLocalizationKey))
			{
				_descriptionText.text = MonoSingleton<Localization>.Instance.GetValueByKey(descriptionLocalizationKey);
			}
			_accept = onAccept;
			_decline = onDecline;
		}

		public override void Close()
		{
			OnBtnAcceptClick();
		}
	}
}
