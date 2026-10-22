using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class AllyInfoView : UIView
	{
		[SerializeField]
		protected TextMesh _title;

		[SerializeField]
		protected TextMesh _description;

		[SerializeField]
		private ItemDashboardStats _healthStats;

		[SerializeField]
		private ItemDashboardStats _meleeDamageStats;

		[SerializeField]
		private ItemDashboardStats _runSpeedStats;

		private AssistantItem _item;

		private float _maxHealth;

		private float _maxMeleeDamage;

		private float _maxRunSpeed;

		private readonly Color CUSTOM_COLOR_RED = new Color(1f, 0f, 30f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_GREEN = new Color(0f, 226f / 255f, 50f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_YELLOW = new Color(1f, 229f / 255f, 0f, 1f);

		private void OnDisable()
		{
		}

		protected void OnEnable()
		{
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

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (gameObject.activeSelf && model is AssistantItem)
			{
				_item = model as AssistantItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				HumanoidLevels alliesConfigurationLevel = instance.UnitConfigurationManager.GetAlliesConfigurationLevel(_item.Type, _item.Level);
				BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(alliesConfigurationLevel.BlasterType, alliesConfigurationLevel.BlasterLevel);
				string arg = _item.Type.ToString().ToLower();
				string key = string.Format("companion_{0}_title", arg);
				string key2 = string.Format("companion_{0}_desc", arg);
				_title.text = Localization.GetValue(key, string.Empty);
				_description.text = Localization.GetValue(key2, string.Empty);
				PlaceProgress(alliesConfigurationLevel.Health, alliesConfigurationLevel.Health, _maxHealth, _healthStats.Progress, _healthStats.ProgressCompare, _healthStats.Arrow);
				PlaceProgress(blasterParams.Damage, blasterParams.Damage, _maxMeleeDamage, _meleeDamageStats.Progress, _meleeDamageStats.ProgressCompare, _meleeDamageStats.Arrow);
				PlaceProgress(alliesConfigurationLevel.RunSpeed, alliesConfigurationLevel.RunSpeed, _maxRunSpeed, _runSpeedStats.Progress, _runSpeedStats.ProgressCompare, _runSpeedStats.Arrow);
				_healthStats.ProgressCompare.gameObject.SetActive(false);
				_meleeDamageStats.ProgressCompare.gameObject.SetActive(false);
				_runSpeedStats.ProgressCompare.gameObject.SetActive(false);
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
