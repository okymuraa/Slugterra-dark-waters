using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupAssistantWarningView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRecoverClick__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseClick__BackingField;

		[SerializeField]
		private GameObject _prontoSprite;

		[SerializeField]
		private GameObject _trixieSprite;

		[SerializeField]
		private GameObject _kordSprite;

		[SerializeField]
		private TextMesh _priceLabel;

		public event Action OnRecoverClick
		{
			add
			{
				Action action = OnRecoverClick__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRecoverClick__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRecoverClick__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRecoverClick__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnCloseClick
		{
			add
			{
				Action action = OnCloseClick__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseClick__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseClick__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseClick__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			gameObject.SetActive(false);
		}

		public void SetAssistantSprite(AllyType assistantType)
		{
			switch (assistantType)
			{
			case AllyType.Trixie:
				return;
			case AllyType.Kord:
				return;
			}
			_prontoSprite.SetActive(true);
			_trixieSprite.SetActive(false);
			_kordSprite.SetActive(false);
		}

		public void SetPrice(int price)
		{
			_priceLabel.text = price.ToString();
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		private void OnBtnRecoverClick()
		{
			if (OnRecoverClick__BackingField != null)
			{
				OnRecoverClick__BackingField();
			}
		}

		private void OnBtnCloseClick()
		{
			if (OnCloseClick__BackingField != null)
			{
				OnCloseClick__BackingField();
			}
		}

		public override void Close()
		{
			gameObject.SetActive(false);
		}
	}
}
