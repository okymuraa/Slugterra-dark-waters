using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.UI.MapMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slugterra.UI.Popup
{
	public class PopupConfirmView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Action> OnDeclineEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Action> OnAcceptEvent__BackingField;

		private LevelItemView[] views;

		[SerializeField]
		protected TextMesh _text;

		[SerializeField]
		protected GameObject _yesBtn;

		[SerializeField]
		protected GameObject _noBtn;

		[SerializeField]
		protected Transform _centralPositionT;

		protected Action _accept;

		protected Action _decline;

		protected string defaultMessage = string.Empty;

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
			defaultMessage = MonoSingleton<Localization>.Instance.GetValueByKey("popup_warning_desc");
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_text);
		}

		protected override void OnDisable()
		{
			_accept = (_decline = null);
			if (SceneManager.GetActiveScene().name == "MapScene" || SceneManager.GetActiveScene().name == "MapScene_HD")
			{
				views = UnityEngine.Object.FindObjectsOfType<LevelItemView>();
				if (views != null && views.Length > 0)
				{
					for (int i = 0; i < views.Length; i++)
					{
						views[i].enabled = true;
						BoxCollider component = views[i].LevelButton.GetComponent<BoxCollider>();
						if (component != null)
						{
							component.enabled = true;
						}
					}
				}
			}
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

		public virtual void Show(string localizationKey, Action onAccept, Action onDecline = null, bool showYesBtn = true, bool showNoBtn = true)
		{
			UnityEngine.Debug.LogFormat("<b><color=green>{0}</color></b>", "Show PopupConfirm");
			if (SceneManager.GetActiveScene().name == "MapScene" || SceneManager.GetActiveScene().name == "MapScene_HD")
			{
				views = UnityEngine.Object.FindObjectsOfType<LevelItemView>();
				if (views != null && views.Length > 0)
				{
					for (int i = 0; i < views.Length; i++)
					{
						views[i].enabled = false;
						BoxCollider component = views[i].LevelButton.GetComponent<BoxCollider>();
						if (component != null)
						{
							component.enabled = false;
						}
					}
				}
			}
			_yesBtn.SetActive(showYesBtn);
			_noBtn.SetActive(showNoBtn);
			gameObject.SetActive(true);
			_text.text = ((!string.IsNullOrEmpty(localizationKey)) ? MonoSingleton<Localization>.Instance.GetValueByKey(localizationKey) : defaultMessage);
			_accept = onAccept;
			_decline = onDecline;
		}

		public override void Close()
		{
			OnBtnDeclineClick();
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
		}
	}
}
