using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class ArmorItemView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUseEvent__BackingField;

		[SerializeField]
		private tk2dButton _useButton;

		[SerializeField]
		private tk2dSprite _armorSprite;

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
		private ProgressBarController _armorProgresBar;

		[SerializeField]
		private ProgressBarController _speedProgresBar;

		[SerializeField]
		private TextMesh _armorText;

		[SerializeField]
		private TextMesh _armorCountText;

		[SerializeField]
		private TextMesh _speedText;

		[SerializeField]
		private TextMesh _speedCountText;

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
			if (!(model is ArmorItem))
			{
				return;
			}
			ArmorItem armorItem = model as ArmorItem;
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			ArmorLevels armorParams = instance.ArmorConfigManager.GetArmorParams(armorItem.Type, armorItem.Level);
			_armorProgresBar.SetProgress(armorParams.DamageAbsorption / 1f, false);
			_armorCountText.text = armorParams.DamageAbsorption.ToString();
			_speedProgresBar.SetProgress(armorParams.SpeedRatio / 1.5f, false);
			_speedCountText.text = armorParams.SpeedRatio.ToString();
			_armorSprite.color = Color.white;
			_nameText.text = armorItem.Type.ToString();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting.Armor.Type == armorItem.Type)
			{
				_activeLightObject.SetActive(true);
				_useButton.gameObject.SetActive(false);
			}
			else
			{
				_activeLightObject.SetActive(false);
				_useButton.gameObject.SetActive(true);
			}
			_levelText.text = armorItem.Level.ToString();
			_upgradeBar.gameObject.SetActive(true);
			for (int i = 1; i < 6; i++)
			{
				if (i <= armorItem.Level)
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
