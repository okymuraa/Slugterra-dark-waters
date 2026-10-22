using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class MissionMenuView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSettingsEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Type> OnTabEvent__BackingField;

		[SerializeField]
		private tk2dButton _backButton;

		[SerializeField]
		private tk2dButton _settingsButton;

		[SerializeField]
		private tk2dButton _playButton;

		[SerializeField]
		private tk2dButton _blastersButton;

		[SerializeField]
		private tk2dButton _slugsButton;

		[SerializeField]
		private tk2dButton _armorsButton;

		[SerializeField]
		private tk2dButton _consumablesButton;

		[SerializeField]
		private tk2dButton _assistantsButton;

		[SerializeField]
		private TextMesh _tabHeaderText;

		[SerializeField]
		private tk2dSprite _attackSlugIcon;

		[SerializeField]
		private tk2dSprite _defenceSlugIcon;

		[SerializeField]
		private tk2dSprite _specialSlugIcon;

		[SerializeField]
		private tk2dSprite _assistantIcon;

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

		public event Action OnSettingsEvent
		{
			add
			{
				Action action = OnSettingsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSettingsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSettingsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSettingsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		private void OnEnable()
		{
			_backButton.ButtonPressedEvent += OnBack;
			_settingsButton.ButtonPressedEvent += OnSettings;
			_playButton.ButtonPressedEvent += OnPlay;
			_blastersButton.ButtonPressedEvent += OnBlasters;
			_slugsButton.ButtonPressedEvent += OnSlugs;
			_armorsButton.ButtonPressedEvent += OnArmors;
			_consumablesButton.ButtonPressedEvent += OnConsumables;
			_assistantsButton.ButtonPressedEvent += OnAssistants;
		}

		private void OnDisable()
		{
			_backButton.ButtonPressedEvent -= OnBack;
			_settingsButton.ButtonPressedEvent -= OnSettings;
			_playButton.ButtonPressedEvent -= OnPlay;
			_blastersButton.ButtonPressedEvent -= OnBlasters;
			_slugsButton.ButtonPressedEvent -= OnSlugs;
			_armorsButton.ButtonPressedEvent -= OnArmors;
			_consumablesButton.ButtonPressedEvent -= OnConsumables;
			_assistantsButton.ButtonPressedEvent -= OnAssistants;
		}

		private void OnBack(tk2dButton source)
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnSettings(tk2dButton source)
		{
			if (OnSettingsEvent__BackingField != null)
			{
				OnSettingsEvent__BackingField();
			}
		}

		private void OnPlay(tk2dButton source)
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnBlasters(tk2dButton source)
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(BlastersTabController));
			}
		}

		private void OnSlugs(tk2dButton source)
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(SlugsTabController));
			}
		}

		private void OnArmors(tk2dButton source)
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(ArmorsTabController));
			}
		}

		private void OnConsumables(tk2dButton source)
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(ConsumablesTabController));
			}
		}

		private void OnAssistants(tk2dButton source)
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(AssistantsTabController));
			}
		}

		public void SetTabHeader(string header)
		{
			_tabHeaderText.text = header;
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is MissionSetting)
			{
				MissionSetting missionSetting = model as MissionSetting;
				if (missionSetting.AttackSlug == null || missionSetting.AttackSlug.Type == SlugType.None)
				{
					_attackSlugIcon.color = Color.clear;
				}
				else
				{
					_attackSlugIcon.SetSprite(string.Format("icon_slug-{0}", missionSetting.AttackSlug.Type));
					_attackSlugIcon.color = Color.white;
				}
				if (missionSetting.DefenceSlug == null || missionSetting.DefenceSlug.Type == SlugType.None)
				{
					_defenceSlugIcon.color = Color.clear;
				}
				else
				{
					_defenceSlugIcon.SetSprite(string.Format("icon_slug-{0}", missionSetting.DefenceSlug.Type));
					_defenceSlugIcon.color = Color.white;
				}
				if (missionSetting.SpecialSlug == null || missionSetting.SpecialSlug.Type == SlugType.None)
				{
					_specialSlugIcon.color = Color.clear;
				}
				else
				{
					_specialSlugIcon.SetSprite(string.Format("icon_slug-{0}", missionSetting.SpecialSlug.Type));
					_specialSlugIcon.color = Color.white;
				}
				if (missionSetting.Assistant == null)
				{
					_assistantIcon.SetSprite("btn_screen-blue");
					_assistantIcon.color = new Color32(154, 154, 154, byte.MaxValue);
				}
				else
				{
					_assistantIcon.SetSprite("icon_assistant-pronto");
					_assistantIcon.color = Color.white;
				}
			}
		}
	}
}
