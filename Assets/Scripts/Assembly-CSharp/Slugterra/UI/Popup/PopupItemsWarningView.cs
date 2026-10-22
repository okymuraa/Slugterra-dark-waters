using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupItemsWarningView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnOkClickEvent__BackingField;

		[SerializeField]
		private TextMesh _localizedMesh;

		public event Action OnOkClickEvent
		{
			add
			{
				Action action = OnOkClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnOkClickEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnOkClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnOkClickEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			gameObject.SetActive(false);
		}

		public void OnBtnOkClick()
		{
			if (OnOkClickEvent__BackingField != null)
			{
				OnOkClickEvent__BackingField();
			}
		}

		public void Show(string localizationKey)
		{
			gameObject.SetActive(true);
			_localizedMesh.text = ((!string.IsNullOrEmpty(localizationKey)) ? MonoSingleton<Localization>.Instance.GetValueByKey(localizationKey) : _localizedMesh.text);
		}

		public override void Close()
		{
		}
	}
}
