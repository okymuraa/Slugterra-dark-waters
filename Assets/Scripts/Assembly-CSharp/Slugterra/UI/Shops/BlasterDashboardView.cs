using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Motion;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.InAppPurchases;
using Slugterra.UI.Announcements;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BlasterDashboardView : ItemDashboardView
	{
		[Serializable]
		private struct RepairSubView
		{
			public GameObject root;

			public tk2dUIProgressBar progressBar;

			public tk2dBaseSprite progressSprite;

			public tk2dBaseSprite repairSprite;

			public ScaleMotion motion;

			public string progressGreenSpriteName;

			public string progressOrangeSpriteName;

			public string progressRedSpriteName;

			public string repairGreenSpriteName;

			public string repairOrangeSpriteName;

			public string repairRedSpriteName;

			public float orangeStateBorder;

			public float greenStateBorder;
		}

		[SerializeField]
		private RecommendedUpgradeController _recommendedUpgrade;

		[SerializeField]
		private ItemDashboardStats _damageStats;

		[SerializeField]
		private ItemDashboardStats _fireRateStats;

		[SerializeField]
		private ItemDashboardStats _energyStats;

		[SerializeField]
		private RepairSubView _repairSubView;

		[SerializeField]
		private TextMesh _damageCount;

		[SerializeField]
		private TextMesh _fireRateCount;

		[SerializeField]
		private TextMesh _rechargeCount;

		[SerializeField]
		private CountDownBarViewController _countdownBar;

		[SerializeField]
		private GameObject _eliSpecs;

		[SerializeField]
		private InAppAnnouncementsController _announcementsController;

		private BlasterItem _item;

		private float _maxDamage;

		private float _maxSlugReloadTime;

		private float _maxFireInterval;

		private readonly Color CUSTOM_COLOR_RED = new Color(1f, 0f, 30f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_GREEN = new Color(0f, 226f / 255f, 50f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_YELLOW = new Color(1f, 229f / 255f, 0f, 1f);

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
			if (_announcementsController != null)
			{
				_announcementsController.SetBadgeOfferBlaster(null);
			}
			if (_countdownBar.gameObject.activeSelf)
			{
				_countdownBar.Show(false);
				_countdownBar.gameObject.SetActive(false);
			}
			_newBadge.SetActive(false);
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			_maxDamage = (_maxSlugReloadTime = (_maxFireInterval = 0f));
			foreach (BlasterConfig blasterConfig in instance.BlasterConfigManager.GetBlasterConfigs())
			{
				for (int i = 0; i < blasterConfig.levels.Count; i++)
				{
					BlasterLevels blasterLevels = blasterConfig.levels[i];
					if (_maxDamage < blasterLevels.Damage)
					{
						_maxDamage = blasterLevels.Damage;
					}
					if (_maxSlugReloadTime < blasterLevels.SlugReloadTime)
					{
						_maxSlugReloadTime = blasterLevels.SlugReloadTime;
					}
					if (_maxFireInterval < blasterLevels.FireInterval)
					{
						_maxFireInterval = blasterLevels.FireInterval;
					}
				}
			}
		}

		public override void OnObjectChanged(object model)
		{
			OnApplyModel(_item);
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!gameObject.activeSelf || !(model is BlasterItem))
			{
				return;
			}
			Localize(model);
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			BlasterItem blasterItem = (_item = model as BlasterItem);
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			float maxBlasterEnergy = instance.BlasterConfigManager.MaxBlasterEnergy;
			BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(blasterItem.Type, blasterItem.Level);
			float num = _item.Energy / blasterParams.Energy;
			bool flag = false;
			if (!IsItemLocked() && IsItemBought() && CanUpgrade() && IsUpgradeAvailable() && blasterItem.Level < 6)
			{
				BlasterLevels blasterParams2 = instance.BlasterConfigManager.GetBlasterParams(blasterItem.Type, blasterItem.Level + 1);
				PlaceProgress(blasterParams2.Damage, blasterParams.Damage, _maxDamage, _damageStats);
				PlaceProgress(blasterParams2.FireInterval, blasterParams.FireInterval, _maxFireInterval, _fireRateStats, true);
				PlaceProgress(blasterParams2.Energy, blasterParams.Energy, maxBlasterEnergy, _energyStats);
			}
			else
			{
				PlaceProgress(blasterParams.Damage, blasterParams.Damage, _maxDamage, _damageStats);
				PlaceProgress(blasterParams.FireInterval, blasterParams.FireInterval, _maxFireInterval, _fireRateStats, true);
				PlaceProgress(blasterParams.Energy, blasterParams.Energy, maxBlasterEnergy, _energyStats);
				_damageStats.ProgressCompare.gameObject.SetActive(false);
				_fireRateStats.ProgressCompare.gameObject.SetActive(false);
			}
			_repairSubView.root.SetActive(flag & !IsItemLocked() & blasterItem.IsBought);
			SetRepairProgress(num);
			_countdownBar.Show(false);
			if (_eliSpecs != null)
			{
				_eliSpecs.SetActive(true);
			}
			RecalculateStatus();
			RecalculateColorScheme();
			RecalculateButtons();
			RecalculateForceRate();
			RecalculateTabBadges();
			if (_announcementsController != null)
			{
				_announcementsController.SetBadgeOfferBlaster(blasterItem);
			}
			BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blasterItem.Type);
			if (!IsItemLocked() && blusterConfiguration.isShowInStore && !GetIsShown(blasterItem.Type))
			{
				if (_newBadge != null)
				{
					_newBadge.SetActive(true);
				}
				SetIsShown(blasterItem.Type);
			}
			else if (_newBadge != null)
			{
				_newBadge.SetActive(false);
			}
			string levelWhenBlasterWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenBlasterWillUnlocked(blasterItem.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenBlasterWillUnlocked);
			if (!IsItemLocked())
			{
				if (blasterItem.IsLocked && levelItemByLevelName != null)
				{
					SetLockedText(MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name);
				}
				return;
			}
			if (blasterItem.IsBought)
			{
				if (num < _repairSubView.greenStateBorder)
				{
					if (!healthRestoreManager.GetHealthConfiguration(blasterItem.Type).IsFullyRestored)
					{
						if (_eliSpecs != null)
						{
							_eliSpecs.SetActive(false);
						}
						_countdownBar.SetTimer(healthRestoreManager.GetHealthConfiguration(blasterItem.Type).TimeToRestore);
						_countdownBar.SetIcon("icon_repair-red").SetTitle("blaster_recovery").Show(true);
					}
				}
				else if (_countdownBar.gameObject.activeSelf)
				{
					_countdownBar.Show(false);
				}
			}
			if (blasterItem.IsLocked && levelItemByLevelName != null)
			{
				SetLockedText(MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name);
			}
			if (_recommendedUpgrade != null)
			{
				_recommendedUpgrade.SetRecommendedUpgrade(new RecommendationDataContainer(blasterItem.Level, blasterItem.RecommendedLevel, blasterItem.IsRequired));
			}
		}

		protected override bool AnyItemIsNotFullyRestored()
		{
			foreach (BlasterItem blaster in MonoSingleton<AppController>.Instance.Model.ShopModel.Blasters)
			{
				BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blaster.Type);
				if ((!blaster.IsLocked || blaster.IsBought) && blusterConfiguration.isShowInStore && !healthRestoreManager.GetHealthConfiguration(blaster.Type).IsFullyRestored)
				{
					return true;
				}
			}
			return false;
		}

		protected override bool AnyItemNotShownInStore()
		{
			foreach (BlasterItem blaster in MonoSingleton<AppController>.Instance.Model.ShopModel.Blasters)
			{
				BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blaster.Type);
				if ((!blaster.IsLocked || blaster.IsBought) && blusterConfiguration.isShowInStore && !GetIsShown(blaster.Type))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool CanUnequip()
		{
			return _item.Type != BlasterType.Default;
		}

		protected override float NextLevelForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(_item.Type, _item.Level + 1).forceRate;
		}

		protected override float ForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(_item.Type, _item.Level).forceRate;
		}

		protected override bool CanUpgrade()
		{
			return false;
		}

		protected override float RecoverPrice()
		{
			return MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(_item.Type, _item.Level).restorationPrice;
		}

		protected override float UpgradePrice()
		{
			return MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(_item.Type, _item.Level + 1);
		}

		protected override int ItemLevel()
		{
			return _item.Level;
		}

		protected override bool IsItemBought()
		{
			string levelWhenBlasterWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenBlasterWillUnlocked(_item.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenBlasterWillUnlocked);
			if (levelItemByLevelName != null && levelItemByLevelName.Status != LevelStatus.Completed)
			{
				return false;
			}
			return _item.IsBought;
		}

		protected override string BuyPrice()
		{
			string blasterInApp = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterInApp(_item.Type, _item.Level);
			if (!string.IsNullOrEmpty(blasterInApp))
			{
				return DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(blasterInApp);
			}
			float blasterPriceGems = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGems(_item.Type, _item.Level);
			if (blasterPriceGems > 0f)
			{
				return blasterPriceGems.ToString();
			}
			return MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(_item.Type, _item.Level).ToString();
		}

		protected override PaymentButtonCurrency BuyCurrency()
		{
			string blasterInApp = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterInApp(_item.Type, _item.Level);
			if (!string.IsNullOrEmpty(blasterInApp))
			{
				return PaymentButtonCurrency.Money;
			}
			float blasterPriceGems = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGems(_item.Type, _item.Level);
			float blasterPriceGold = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterPriceGold(_item.Type, _item.Level);
			if (blasterPriceGems > 0f)
			{
				return PaymentButtonCurrency.Gems;
			}
			return PaymentButtonCurrency.Coins;
		}

		protected override bool IsItemLocked()
		{
			string levelWhenBlasterWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenBlasterWillUnlocked(_item.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenBlasterWillUnlocked);
			if (levelItemByLevelName != null && levelItemByLevelName.Status != LevelStatus.Completed)
			{
				return true;
			}
			return _item.IsLocked && !_item.IsBought;
		}

		protected override bool IsUpgradeAvailable()
		{
			if (IsItemLocked())
			{
				return false;
			}
			return IsUpgradeUnlocked(_item.Level);
		}

		protected override bool IsEquipped()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (_item != null && missionSetting.Blaster != null)
			{
				return _item.Type == missionSetting.Blaster.Type;
			}
			return false;
		}

		private void SetIsShown(BlasterType blaster)
		{
			PlayerPrefs.SetInt(blaster.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(BlasterType blaster)
		{
			return PlayerPrefs.GetInt(blaster.ToString() + "IsShown", 0) == 1;
		}

		private void Localize(object model)
		{
			if (model is BlasterItem)
			{
				BlasterItem blasterItem = model as BlasterItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				BlasterConfig blusterConfiguration = instance.BlasterConfigManager.GetBlusterConfiguration(blasterItem.Type);
				_title.text = Localization.GetValue(blusterConfiguration.title, string.Empty);
				_description.text = Localization.GetValue(blusterConfiguration.description, string.Empty);
			}
		}

		private void PlaceProgress(float value, float valueOwned, float maxValue, ItemDashboardStats dashboardStats, bool invertAlgoritm = false)
		{
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
			Color color = CUSTOM_COLOR_GREEN;
			dashboardStats.Progress.Value = num;
			if (value == valueOwned)
			{
				color = CUSTOM_COLOR_YELLOW;
				dashboardStats.ProgressCompare.gameObject.SetActive(false);
				dashboardStats.Arrow.gameObject.SetActive(false);
			}
			else
			{
				dashboardStats.ProgressCompare.gameObject.SetActive(true);
				dashboardStats.Arrow.gameObject.SetActive(true);
				if (num > num2)
				{
					dashboardStats.ProgressCompare.Value = num;
					dashboardStats.Progress.Value = num2;
					dashboardStats.Arrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				}
				else
				{
					color = CUSTOM_COLOR_RED;
					dashboardStats.ProgressCompare.Value = num2;
					dashboardStats.Arrow.transform.rotation = Quaternion.identity;
				}
			}
			dashboardStats.ProgressCompare.slicedSpriteBar.color = color;
			dashboardStats.Arrow.color = color;
		}

		private void SetRepairProgress(float progress)
		{
			_repairSubView.progressBar.Value = progress;
			_repairSubView.motion.enabled = false;
			if (progress >= _repairSubView.greenStateBorder)
			{
				_repairSubView.progressSprite.SetSprite(_repairSubView.progressGreenSpriteName);
				_repairSubView.repairSprite.SetSprite(_repairSubView.repairGreenSpriteName);
			}
			else if (progress >= _repairSubView.orangeStateBorder)
			{
				_repairSubView.progressSprite.SetSprite(_repairSubView.progressOrangeSpriteName);
				_repairSubView.repairSprite.SetSprite(_repairSubView.repairOrangeSpriteName);
			}
			else
			{
				_repairSubView.progressSprite.SetSprite(_repairSubView.progressRedSpriteName);
				_repairSubView.repairSprite.SetSprite(_repairSubView.repairRedSpriteName);
				_repairSubView.motion.enabled = true;
			}
		}

		private void OnRestorationValuesUpdatedHandler()
		{
			CheckHealthRestorationState();
		}

		private void CheckHealthRestorationState()
		{
			if (_item != null && healthRestoreManager.GetHealthConfiguration(_item.Type).IsFullyRestored && _item.Energy <= 0f)
			{
				BlasterLevels blasterParams = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(_item.Type, _item.Level);
				_item.Energy += Mathf.Round(blasterParams.Energy * 0.1f);
			}
			OnApplyModel(_item);
		}
	}
}
