using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.UI.MapMenu;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class AssistantDashboardView : ItemDashboardView
	{
		public const int MaxAssistantLevel = 5;

		[SerializeField]
		private RecommendedUpgradeController _recommendedUpgrade;

		[SerializeField]
		private tk2dUIProgressBar _vitalityProgress;

		[SerializeField]
		private tk2dSprite _vitalityArrow;

		[SerializeField]
		private tk2dUIProgressBar _vitalityProgressCompare;

		[SerializeField]
		private tk2dUIProgressBar _damageProgress;

		[SerializeField]
		private tk2dSprite _damageArrow;

		[SerializeField]
		private tk2dUIProgressBar _damageProgressCompare;

		[SerializeField]
		private tk2dUIProgressBar _speedProgress;

		[SerializeField]
		private tk2dSprite _speedArrow;

		[SerializeField]
		private tk2dUIProgressBar _speedProgressCompare;

		[SerializeField]
		private TextMesh _vitalityCount;

		[SerializeField]
		private TextMesh _damageCount;

		[SerializeField]
		private TextMesh _speedCount;

		[SerializeField]
		private CountDownBarViewController _countdownBar;

		[SerializeField]
		private NextUpgradeLevel _nextUpgradeLevel;

		private AssistantItem _item;

		private float _maxHealth;

		private float _maxMeleeDamage;

		private float _maxRunSpeed;

		public PaymentButtonController PaymentButton
		{
			get
			{
				return _paymentButton;
			}
		}

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				if (MonoSingleton<GameConfiguration>.Instance != null)
				{
					return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
				}
				return null;
			}
		}

		private void OnEnable()
		{
			healthRestoreManager.OnUpdateValues += OnRestorationValuesUpdatedHandler;
		}

		private void OnDisable()
		{
			if (healthRestoreManager != null)
			{
				healthRestoreManager.OnUpdateValues -= OnRestorationValuesUpdatedHandler;
			}
			if (_countdownBar.gameObject.activeSelf)
			{
				_countdownBar.Show(false);
				_countdownBar.gameObject.SetActive(false);
			}
			_nextUpgradeLevel.parent.SetActive(false);
			_newBadge.SetActive(false);
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			_maxHealth = (_maxMeleeDamage = (_maxRunSpeed = 0f));
			List<HumanoidLevels> assistantConfigurationsLevels = instance.UnitConfigurationManager.GetAssistantConfigurationsLevels(5);
			foreach (HumanoidLevels item in assistantConfigurationsLevels)
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

		private bool IsWithPlayer(AssistantItem assistant)
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (assistant != null && missionSetting.Assistant != null)
			{
				return assistant.Type == missionSetting.Assistant.Type;
			}
			return false;
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!gameObject.activeSelf)
			{
				return;
			}
			_nextUpgradeLevel.parent.SetActive(false);
			if (!(model is AssistantItem) || model == null)
			{
				return;
			}
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			AssistantItem assistantItem = model as AssistantItem;
			if (assistantItem.Type == AllyType.None)
			{
				return;
			}
			_item = assistantItem;
			int itemLvl = 0;
			Localize(assistantItem);
			ShopConfig shopConfig = MonoSingleton<AppConfiguration>.Instance.ShopConfig;
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			HumanoidLevels alliesConfigurationLevel = instance.UnitConfigurationManager.GetAlliesConfigurationLevel(assistantItem.Type, assistantItem.Level);
			BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(alliesConfigurationLevel.BlasterType, alliesConfigurationLevel.BlasterLevel);
			if (CanUpgrade() && IsUpgradeAvailable() && assistantItem.Level < 5)
			{
				HumanoidLevels alliesConfigurationLevel2 = instance.UnitConfigurationManager.GetAlliesConfigurationLevel(assistantItem.Type, assistantItem.Level + 1);
				BlasterLevels blasterParams2 = instance.BlasterConfigManager.GetBlasterParams(alliesConfigurationLevel2.BlasterType, alliesConfigurationLevel2.BlasterLevel);
				PlaceProgress(alliesConfigurationLevel2.Health, alliesConfigurationLevel.Health, _maxHealth, _vitalityProgress, _vitalityProgressCompare, _vitalityArrow);
				PlaceProgress(blasterParams2.Damage, blasterParams.Damage, _maxMeleeDamage, _damageProgress, _damageProgressCompare, _damageArrow);
				PlaceProgress(alliesConfigurationLevel2.RunSpeed, alliesConfigurationLevel.RunSpeed, _maxRunSpeed, _speedProgress, _speedProgressCompare, _speedArrow);
				if (assistantItem.IsBought)
				{
					_nextUpgradeLevel.parent.SetActive(true);
					_newBadge.SetActive(false);
					int num = ItemLevel() + 1;
					string text = "lvl_0{0}_title";
					if (num == 5)
					{
						text += "_short";
					}
					TextMesh shadowNextLevel = _nextUpgradeLevel.shadowNextLevel;
					string valueByKey = MonoSingleton<Localization>.Instance.GetValueByKey(string.Format(text, num));
					_nextUpgradeLevel.nextLevel.text = valueByKey;
					shadowNextLevel.text = valueByKey;
				}
			}
			else
			{
				PlaceProgress(alliesConfigurationLevel.Health, alliesConfigurationLevel.Health, _maxHealth, _vitalityProgress, _vitalityProgressCompare, _vitalityArrow);
				PlaceProgress(blasterParams.Damage, blasterParams.Damage, _maxMeleeDamage, _damageProgress, _damageProgressCompare, _damageArrow);
				PlaceProgress(alliesConfigurationLevel.RunSpeed, alliesConfigurationLevel.RunSpeed, _maxRunSpeed, _speedProgress, _speedProgressCompare, _speedArrow);
				_vitalityProgressCompare.gameObject.SetActive(false);
				_damageProgressCompare.gameObject.SetActive(false);
				_speedProgressCompare.gameObject.SetActive(false);
			}
			HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(assistantItem.Type);
			if ((assistantItem.IsBought || !assistantItem.IsLocked) && alliesConfig.isShowInStore && !GetIsShown(assistantItem.Type))
			{
				if (_newBadge != null)
				{
					_newBadge.SetActive(!_nextUpgradeLevel.parent.activeSelf);
				}
				SetIsShown(_item.Type);
			}
			else if (_newBadge != null)
			{
				_newBadge.SetActive(false);
			}
			RecalculateStatus();
			RecalculateColorScheme();
			RecalculateButtons();
			RecalculateForceRate();
			RecalculateTabBadges();
			if (assistantItem.IsLocked)
			{
				string levelWhenAssistantWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenAssistantWillUnlocked(assistantItem.Type);
				LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenAssistantWillUnlocked);
				if (levelItemByLevelName != null)
				{
					SetLockedText(MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name);
				}
				return;
			}
			if (assistantItem.IsBought)
			{
				if (!healthRestoreManager.GetHealthConfiguration(assistantItem.Type).IsFullyRestored)
				{
					_countdownBar.SetTimer(healthRestoreManager.GetHealthConfiguration(assistantItem.Type).TimeToRestore);
					_countdownBar.SetIcon("icon_injury").SetTitle("asistant_recovery").Show(true);
				}
				else if (_countdownBar.gameObject.activeSelf)
				{
					_countdownBar.Show(false);
				}
			}
			if (_recommendedUpgrade != null)
			{
				_recommendedUpgrade.SetRecommendedUpgrade(new RecommendationDataContainer(itemLvl, assistantItem.RecommendedLevel, assistantItem.IsRequired));
			}
		}

		public override void RecalculateTabBadges()
		{
			base.RecalculateTabBadges();
			if (LevelItemController.SelectedLevel != null && LevelItemController.SelectedLevel.Model.Type == LevelType.Arena && _newItemInCategoryBadge != null && _newItemInCategoryBadge.activeSelf)
			{
				_newItemInCategoryBadge.SetActive(false);
			}
		}

		public void OnUpgrade(object model)
		{
			if (model is AssistantItem && model != null)
			{
				AssistantItem assistantItem = model as AssistantItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				HumanoidLevels alliesConfigurationLevel = instance.UnitConfigurationManager.GetAlliesConfigurationLevel(assistantItem.Type, assistantItem.Level);
				if ((assistantItem.IsBought || !assistantItem.IsLocked) && CanUpgrade() && IsUpgradeAvailable() && assistantItem.Level < 5)
				{
					HumanoidLevels alliesConfigurationLevel2 = instance.UnitConfigurationManager.GetAlliesConfigurationLevel(assistantItem.Type, assistantItem.Level + 1);
					BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(alliesConfigurationLevel.BlasterType, alliesConfigurationLevel.BlasterLevel);
					BlasterLevels blasterParams2 = instance.BlasterConfigManager.GetBlasterParams(alliesConfigurationLevel2.BlasterType, alliesConfigurationLevel2.BlasterLevel);
					PlaceProgress(alliesConfigurationLevel2.Health, alliesConfigurationLevel.Health, _maxHealth, _vitalityProgress, _vitalityProgressCompare, _vitalityArrow);
					PlaceProgress(blasterParams2.Damage, blasterParams.Damage, _maxMeleeDamage, _damageProgress, _damageProgressCompare, _damageArrow);
					PlaceProgress(alliesConfigurationLevel2.RunSpeed, alliesConfigurationLevel.RunSpeed, _maxRunSpeed, _speedProgress, _speedProgressCompare, _speedArrow);
				}
			}
		}

		protected override bool AnyItemIsNotFullyRestored()
		{
			foreach (AssistantItem assistant in MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants)
			{
				HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(assistant.Type);
				if (!assistant.IsLocked && alliesConfig.isShowInStore && !healthRestoreManager.GetHealthConfiguration(assistant.Type).IsFullyRestored)
				{
					return true;
				}
			}
			return false;
		}

		protected override bool AnyItemNotShownInStore()
		{
			foreach (AssistantItem assistant in MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants)
			{
				HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(assistant.Type);
				if ((!assistant.IsLocked || assistant.IsBought) && alliesConfig.isShowInStore && !GetIsShown(assistant.Type))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool IsUpgradeAvailable()
		{
			if (IsItemLocked())
			{
				return false;
			}
			return IsUpgradeUnlocked(_item.Level + 1);
		}

		protected override float UpgradePrices(int levelTo, int levelFrom = -1)
		{
			float num = 0f;
			if (levelFrom == -1)
			{
				levelFrom = _item.Level;
			}
			for (int i = levelFrom; i <= levelTo; i++)
			{
				num += UpgradePriceByLevel(i);
			}
			return num;
		}

		protected override float UpgradePriceByLevel(int level)
		{
			return MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(_item.Type, level).priceGold;
		}

		protected override float UpgradePrice()
		{
			return MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(_item.Type, _item.Level + 1).priceGold;
		}

		protected override float NextLevelForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(_item.Type, _item.Level + 1).forceRate;
		}

		protected override float ForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(_item.Type, _item.Level).forceRate;
		}

		protected override bool IsItemRecovered()
		{
			return healthRestoreManager.GetHealthConfiguration(_item.Type).IsFullyRestored;
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
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (_item != null && missionSetting.Assistant != null)
			{
				return _item.Type == missionSetting.Assistant.Type;
			}
			return false;
		}

		protected override int ItemLevel()
		{
			return _item.Level;
		}

		protected override bool CanUpgrade()
		{
			return _item.Level < 5;
		}

		protected override float RecoverPrice()
		{
			return healthRestoreManager.GetHealPrice(_item.Type, _item.Level);
		}

		protected override string BuyPrice()
		{
			return MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(_item.Type, _item.Level).GamePrice.Value.ToString();
		}

		private void SetIsShown(AllyType ally)
		{
			PlayerPrefs.SetInt(ally.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(AllyType ally)
		{
			return PlayerPrefs.GetInt(ally.ToString() + "IsShown", 0) == 1;
		}

		private IEnumerator UpdateRecoverPrice()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();
				float price = healthRestoreManager.GetHealPrice(_item.Type, _item.Level);
				_paymentButton.View.SetPrice(PaymentButtonCurrency.Coins, price.ToString());
			}
		}

		public override void OnObjectChanged(object model)
		{
			OnApplyModel(_item);
		}

		private void Localize(AssistantItem item)
		{
			string arg = item.Type.ToString().ToLower();
			string key = string.Format("companion_{0}_title", arg);
			string key2 = string.Format("companion_{0}_desc", arg);
			_title.text = Localization.GetValue(key, string.Empty);
			_description.text = Localization.GetValue(key2, string.Empty);
		}

		private void PlaceProgress(float value, float valueOwned, float maxValue, tk2dUIProgressBar progress, tk2dUIProgressBar progressCompare, tk2dSprite arrow, bool invertAlgoritm = false)
		{
			Color color = new Color(1f, 0f, 30f / 255f, 1f);
			Color color2 = new Color(0f, 226f / 255f, 50f / 255f, 1f);
			Color color3 = new Color(1f, 229f / 255f, 0f, 1f);
			float num;
			float num2;
			if (invertAlgoritm)
			{
				num = 1f - value / maxValue;
				num2 = 1f - valueOwned / maxValue;
			}
			else
			{
				num = value / maxValue;
				num2 = valueOwned / maxValue;
			}
			Color color4 = color2;
			progress.Value = num;
			if (value == valueOwned)
			{
				color4 = color3;
				progressCompare.gameObject.SetActive(false);
				arrow.gameObject.SetActive(false);
			}
			else
			{
				progressCompare.gameObject.SetActive(true);
				arrow.gameObject.SetActive(true);
				if (num > num2)
				{
					progressCompare.Value = num;
					progress.Value = num2;
					arrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				}
				else
				{
					color4 = color;
					progressCompare.Value = num2;
					arrow.transform.rotation = Quaternion.identity;
				}
			}
			progressCompare.slicedSpriteBar.color = color4;
			arrow.color = color4;
		}

		private void OnRestorationValuesUpdatedHandler()
		{
			CheckHealthRestorationState();
		}

		private void CheckHealthRestorationState()
		{
			OnApplyModel(_item);
		}
	}
}
