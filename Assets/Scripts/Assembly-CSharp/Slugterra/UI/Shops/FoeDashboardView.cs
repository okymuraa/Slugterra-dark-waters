using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class FoeDashboardView : ItemDashboardView
	{
		public const int MaxFoeLevel = 0;

		[SerializeField]
		private tk2dUIProgressBar _vitalityProgress;

		[SerializeField]
		private tk2dUIProgressBar _damageProgress;

		[SerializeField]
		private tk2dUIProgressBar _speedProgress;

		[SerializeField]
		private TextMesh _vitalityCount;

		[SerializeField]
		private TextMesh _damageCount;

		[SerializeField]
		private TextMesh _speedCount;

		private float _maxHealth;

		private float _maxMeleeDamage;

		private float _maxRunSpeed;

		private FoeItem _item;

		protected override void OnCreate()
		{
			base.OnCreate();
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			_maxHealth = (_maxMeleeDamage = (_maxRunSpeed = 0f));
			List<FoeLevels> foesConfigurationsLevels = instance.UnitConfigurationManager.GetFoesConfigurationsLevels(0);
			foreach (FoeLevels item in foesConfigurationsLevels)
			{
				if (_maxHealth < item.Health)
				{
					_maxHealth = item.Health;
				}
				if (_maxRunSpeed < item.RunSpeed)
				{
					_maxRunSpeed = item.RunSpeed;
				}
			}
			foreach (BlasterConfig blasterConfig in instance.BlasterConfigManager.GetBlasterConfigs())
			{
				for (int i = 0; i < blasterConfig.levels.Count; i++)
				{
					BlasterLevels blasterLevels = blasterConfig.levels[i];
					if (_maxMeleeDamage < blasterLevels.Damage)
					{
						_maxMeleeDamage = blasterLevels.Damage;
					}
				}
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!(model is FoeItem))
			{
				return;
			}
			FoeItem foeItem = (_item = model as FoeItem);
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			EnemyType type = (EnemyType)Enum.Parse(typeof(EnemyType), foeItem.Type);
			FoeConfig foesConfig = instance.UnitConfigurationManager.GetFoesConfig(type);
			HumanoidLevels humanoidLevels = foesConfig.levels[foeItem.Level];
			BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(humanoidLevels.BlasterType, humanoidLevels.BlasterLevel);
			Localize(foesConfig);
			_vitalityProgress.Value = humanoidLevels.Health / _maxHealth;
			_damageProgress.Value = blasterParams.Damage / _maxMeleeDamage;
			_speedProgress.Value = humanoidLevels.RunSpeed / _maxRunSpeed;
			_vitalityCount.text = Mathf.RoundToInt(_vitalityProgress.Value * 100f).ToString();
			_damageCount.text = Mathf.RoundToInt(_damageProgress.Value * 100f).ToString();
			_speedCount.text = Mathf.RoundToInt(_speedProgress.Value * 100f).ToString();
			RecalculateStatus();
			RecalculateColorScheme();
			RecalculateButtons();
			RecalculateForceRate();
			RecalculateTabBadges();
			if (!IsItemLocked() && !GetIsShown(foeItem.Type))
			{
				_newBadge.SetActive(true);
				SetIsShown(foeItem.Type);
			}
			else
			{
				_newBadge.SetActive(false);
			}
			if (IsItemLocked())
			{
				string levelWhenBossWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenBossWillUnlocked(foeItem.Type);
				LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenBossWillUnlocked);
				if (levelItemByLevelName != null)
				{
					SetLockedText(MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name);
				}
			}
		}

		protected override bool AnyItemNotShownInStore()
		{
			foreach (FoeItem foe in MonoSingleton<AppController>.Instance.Model.ShopModel.Foes)
			{
				EnemyType type = (EnemyType)Enum.Parse(typeof(EnemyType), foe.Type);
				FoeConfig foesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetFoesConfig(type);
				if ((!foe.IsLocked || foe.IsBought) && foesConfig.isShowInStore && !GetIsShown(foe.Type))
				{
					return true;
				}
			}
			return false;
		}

		protected override float ForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetFoesConfig((EnemyType)Enum.Parse(typeof(EnemyType), _item.Type)).levels[_item.Level].forceRate;
		}

		protected override bool CanUpgrade()
		{
			return false;
		}

		protected override bool IsItemLocked()
		{
			return _item.IsLocked && !_item.IsBought;
		}

		protected override bool IsItemBought()
		{
			return _item.IsBought;
		}

		protected override bool IsEquipped()
		{
			return MonoSingleton<AppController>.Instance.Model.DuelSetting.Foe.Type == _item.Type;
		}

		private void SetIsShown(string slug)
		{
			PlayerPrefs.SetInt(slug + "IsShown", 1);
		}

		private bool GetIsShown(string slug)
		{
			return PlayerPrefs.GetInt(slug + "IsShown", 0) == 1;
		}

		private void Localize(FoeConfig config)
		{
			_title.text = Localization.GetValue(config.title, string.Empty);
			_description.text = Localization.GetValue(config.description, string.Empty);
		}
	}
}
