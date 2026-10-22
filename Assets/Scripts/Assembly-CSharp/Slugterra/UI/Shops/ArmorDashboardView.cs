using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ArmorDashboardView : ItemDashboardView
	{
		[SerializeField]
		private RecommendedUpgradeController _recommendedUpgrade;

		[SerializeField]
		private tk2dUIProgressBar _defenceProgress;

		[SerializeField]
		private tk2dSprite _defenceArrow;

		[SerializeField]
		private tk2dUIProgressBar _defenceProgressCompare;

		[SerializeField]
		private tk2dUIProgressBar _weightProgress;

		[SerializeField]
		private tk2dSprite _weightArrow;

		[SerializeField]
		private tk2dUIProgressBar _weightProgressCompare;

		[SerializeField]
		private TextMesh _defenceCount;

		[SerializeField]
		private TextMesh _weightCount;

		private ArmorItem _item;

		private float _maxDamageAbsorption;

		private float _maxSpeedRatio;

		public PaymentButtonController PaymentButton
		{
			get
			{
				return _paymentButton;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			_maxDamageAbsorption = (_maxSpeedRatio = 0f);
			foreach (ArmorConfig armorConfig in instance.ArmorConfigManager.GetArmorConfigs())
			{
				for (int i = 0; i < armorConfig.levels.Count; i++)
				{
					ArmorLevels armorLevels = armorConfig.levels[i];
					if (_maxDamageAbsorption < armorLevels.DamageAbsorption)
					{
						_maxDamageAbsorption = armorLevels.DamageAbsorption;
					}
					if (_maxSpeedRatio < armorLevels.SpeedRatio)
					{
						_maxSpeedRatio = armorLevels.SpeedRatio;
					}
				}
			}
		}

		public override void OnObjectChanged(object model)
		{
			OnApplyModel(_item);
		}

		private bool IsWearing(ArmorItem armor)
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (armor != null && missionSetting.Armor != null)
			{
				return armor.Type == missionSetting.Armor.Type;
			}
			return false;
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!(model is ArmorItem))
			{
				return;
			}
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			ArmorItem armorItem = (_item = model as ArmorItem);
			Localize(armorItem);
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			ArmorLevels armorParams = instance.ArmorConfigManager.GetArmorParams(armorItem.Type, armorItem.Level);
			if (!IsItemLocked() && IsItemBought() && CanUpgrade() && IsUpgradeAvailable() && armorItem.Level < 6)
			{
				ArmorLevels armorParams2 = instance.ArmorConfigManager.GetArmorParams(armorItem.Type, armorItem.Level + 1);
				PlaceProgress(armorParams2.DamageAbsorption, armorParams.DamageAbsorption, _maxDamageAbsorption, _defenceProgress, _defenceProgressCompare, _defenceArrow);
				PlaceProgress(armorParams2.SpeedRatio, armorParams.SpeedRatio, _maxSpeedRatio, _weightProgress, _weightProgressCompare, _weightArrow, true);
			}
			else
			{
				PlaceProgress(armorParams.DamageAbsorption, armorParams.DamageAbsorption, _maxDamageAbsorption, _defenceProgress, _defenceProgressCompare, _defenceArrow);
				PlaceProgress(armorParams.SpeedRatio, armorParams.SpeedRatio, _maxSpeedRatio, _weightProgress, _weightProgressCompare, _weightArrow, true);
				_defenceProgressCompare.gameObject.SetActive(false);
				_weightProgressCompare.gameObject.SetActive(false);
			}
			ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armorItem.Type);
			if (!IsItemLocked() && armorConfiguration.isShowInStore && !GetIsShown(armorItem.Type))
			{
				if (_newBadge != null)
				{
					_newBadge.SetActive(true);
				}
				SetIsShown(armorItem.Type);
			}
			else if (_newBadge != null)
			{
				_newBadge.SetActive(false);
			}
			RecalculateStatus();
			RecalculateColorScheme();
			RecalculateButtons(armorItem.IsBackpack);
			RecalculateForceRate();
			RecalculateTabBadges();
			string levelWhenArmorWillUnlocked = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelWhenArmorWillUnlocked(armorItem.Type);
			LevelItem levelItemByLevelName = MonoSingleton<AppController>.Instance.Model.GetLevelItemByLevelName(levelWhenArmorWillUnlocked);
			if (!IsItemLocked())
			{
				if (levelItemByLevelName != null)
				{
					SetLockedText(MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name);
				}
				return;
			}
			if (armorItem.IsLocked && levelItemByLevelName != null)
			{
				SetLockedText(MonoSingleton<Localization>.Instance.GetValueByKey("menu_item_unlock_title") + levelItemByLevelName.Name);
			}
			if (_recommendedUpgrade != null)
			{
				_recommendedUpgrade.SetRecommendedUpgrade(new RecommendationDataContainer(armorItem.Level, armorItem.RecommendedLevel, armorItem.IsRequired));
			}
		}

		protected override bool IsEquipped()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (_item != null && missionSetting.Armor != null)
			{
				return _item.Type == missionSetting.Armor.Type;
			}
			return false;
		}

		protected override float NextLevelForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(_item.Type, _item.Level + 1).forceRate;
		}

		protected override float ForceRate()
		{
			return MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(_item.Type, _item.Level).forceRate;
		}

		protected override bool IsItemLocked()
		{
			return _item.IsLocked && !_item.IsBought;
		}

		protected override bool IsItemBought()
		{
			return _item.IsBought;
		}

		protected override bool IsUpgradeAvailable()
		{
			if (IsItemLocked())
			{
				return false;
			}
			return IsUpgradeUnlocked(_item.Level);
		}

		protected override bool AnyItemNotShownInStore()
		{
			foreach (ArmorItem armor in MonoSingleton<AppController>.Instance.Model.ShopModel.Armors)
			{
				ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armor.Type);
				if ((!armor.IsLocked || armor.IsBought) && armorConfiguration.isShowInStore && !GetIsShown(armor.Type))
				{
					return true;
				}
			}
			return false;
		}

		protected override int ItemLevel()
		{
			return _item.Level;
		}

		protected override float UpgradePrice()
		{
			return MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorPriceGold(_item.Type, _item.Level + 1);
		}

		protected override bool CanUpgrade()
		{
			return false;
		}

		protected override string BuyPrice()
		{
			return MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorPriceGold(_item.Type, _item.Level).ToString();
		}

		private void SetIsShown(ArmorType ally)
		{
			Debug.Log(ally);
			PlayerPrefs.SetInt(ally.ToString() + "IsShown", 1);
		}

		private bool GetIsShown(ArmorType ally)
		{
			return PlayerPrefs.GetInt(ally.ToString() + "IsShown", 0) == 1;
		}

		private void Localize(object model)
		{
			if (model is ArmorItem)
			{
				ArmorItem armorItem = model as ArmorItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				ArmorConfig armorConfiguration = instance.ArmorConfigManager.GetArmorConfiguration(armorItem.Type);
				_title.text = Localization.GetValue(armorConfiguration.title, string.Empty);
				_description.text = Localization.GetValue(armorConfiguration.description, string.Empty);
			}
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
	}
}
