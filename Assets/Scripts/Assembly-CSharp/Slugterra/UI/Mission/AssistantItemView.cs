using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class AssistantItemView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUseEvent__BackingField;

		[SerializeField]
		private tk2dButton _useButton;

		[SerializeField]
		private tk2dSprite _assistantIcon;

		[SerializeField]
		private TextMesh _nameText;

		[SerializeField]
		private TextMesh _levelText;

		[SerializeField]
		private TextMesh _useButtonText;

		[SerializeField]
		private GameObject _activeLightObject;

		[SerializeField]
		private GameObject _specsObject;

		[SerializeField]
		private ProgressBarController _damageProgresBar;

		[SerializeField]
		private ProgressBarController _speedProgresBar;

		[SerializeField]
		private ProgressBarController _fireRateProgresBar;

		[SerializeField]
		private ProgressBarController _slugSpeedProgresBar;

		[SerializeField]
		private TextMesh _damageText;

		[SerializeField]
		private TextMesh _damageCountText;

		[SerializeField]
		private TextMesh _speedText;

		[SerializeField]
		private TextMesh _speedCountText;

		[SerializeField]
		private TextMesh _fireRateText;

		[SerializeField]
		private TextMesh _fireRateCountText;

		[SerializeField]
		private TextMesh _slugSpeedText;

		[SerializeField]
		private TextMesh _slugSpeedCountText;

		[SerializeField]
		private Transform _upgradeBar;

		[SerializeField]
		private Transform[] _upgradeLevels;

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

		private void OnEnable()
		{
			_useButton.ButtonPressedEvent += OnUse;
		}

		private void OnDisable()
		{
			_useButton.ButtonPressedEvent -= OnUse;
		}

		private void OnUse(tk2dButton source)
		{
			if (OnUseEvent__BackingField != null)
			{
				OnUseEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!(model is AssistantItem))
			{
				return;
			}
			AssistantItem assistantItem = model as AssistantItem;
			_specsObject.SetActive(false);
			_assistantIcon.color = Color.white;
			_nameText.text = assistantItem.Type.ToString();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (assistantItem.Type == missionSetting.Assistant.Type)
			{
				_activeLightObject.SetActive(true);
				_useButton.gameObject.SetActive(false);
			}
			else
			{
				_activeLightObject.SetActive(false);
				_useButton.gameObject.SetActive(true);
			}
			_levelText.text = assistantItem.Level.ToString();
			_upgradeBar.gameObject.SetActive(true);
			for (int i = 1; i < 5; i++)
			{
				if (i <= assistantItem.Level)
				{
					_upgradeLevels[i - 1].gameObject.SetActive(true);
				}
				else
				{
					_upgradeLevels[i - 1].gameObject.SetActive(false);
				}
			}
		}
	}
}
