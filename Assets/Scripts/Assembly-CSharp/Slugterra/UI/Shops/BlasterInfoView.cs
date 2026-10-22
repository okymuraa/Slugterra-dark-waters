using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BlasterInfoView : UIView
	{
		[SerializeField]
		protected TextMesh _title;

		[SerializeField]
		protected TextMesh _description;

		[SerializeField]
		private ItemDashboardStats _damageStats;

		[SerializeField]
		private ItemDashboardStats _fireRateStats;

		[SerializeField]
		private ItemDashboardStats _energyStats;

		[SerializeField]
		private TextMesh _damageCount;

		[SerializeField]
		private TextMesh _fireRateCount;

		[SerializeField]
		private TextMesh _rechargeCount;

		private BlasterItem _item;

		private float _maxDamage;

		private float _maxSlugReloadTime;

		private float _maxFireInterval;

		private readonly Color CUSTOM_COLOR_RED = new Color(1f, 0f, 30f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_GREEN = new Color(0f, 226f / 255f, 50f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_YELLOW = new Color(1f, 229f / 255f, 0f, 1f);

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		protected void OnCreate()
		{
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

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (gameObject.activeSelf && model is BlasterItem)
			{
				_item = model as BlasterItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				BlasterConfig blusterConfiguration = instance.BlasterConfigManager.GetBlusterConfiguration(_item.Type);
				_title.text = Localization.GetValue(blusterConfiguration.title, string.Empty);
				_description.text = Localization.GetValue(blusterConfiguration.description, string.Empty);
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				float maxBlasterEnergy = instance.BlasterConfigManager.MaxBlasterEnergy;
				BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(_item.Type, _item.Level);
				float num = _item.Energy / blasterParams.Energy;
				PlaceProgress(blasterParams.Damage, blasterParams.Damage, _maxDamage, _damageStats);
				PlaceProgress(blasterParams.FireInterval, blasterParams.FireInterval, _maxFireInterval, _fireRateStats, true);
				PlaceProgress(blasterParams.Energy, blasterParams.Energy, maxBlasterEnergy, _energyStats);
				_damageStats.ProgressCompare.gameObject.SetActive(false);
				_fireRateStats.ProgressCompare.gameObject.SetActive(false);
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
	}
}
