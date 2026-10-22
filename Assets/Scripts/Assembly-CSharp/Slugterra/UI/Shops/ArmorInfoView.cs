using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ArmorInfoView : UIView
	{
		[SerializeField]
		protected TextMesh _title;

		[SerializeField]
		protected TextMesh _description;

		[SerializeField]
		private ItemDashboardStats _damageAbsorptionStats;

		[SerializeField]
		private ItemDashboardStats _speedRatioStats;

		private ArmorItem _item;

		private float _maxDamageAbsorption;

		private float _maxSpeedRatio;

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

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (gameObject.activeSelf && model is ArmorItem)
			{
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				ArmorItem item = model as ArmorItem;
				_item = item;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				ArmorConfig armorConfiguration = instance.ArmorConfigManager.GetArmorConfiguration(_item.Type);
				_title.text = Localization.GetValue(armorConfiguration.title, string.Empty);
				_description.text = Localization.GetValue(armorConfiguration.description, string.Empty);
				ArmorLevels armorParams = instance.ArmorConfigManager.GetArmorParams(_item.Type, _item.Level);
				PlaceProgress(armorParams.DamageAbsorption, armorParams.DamageAbsorption, _maxDamageAbsorption, _damageAbsorptionStats.Progress, _damageAbsorptionStats.ProgressCompare, _damageAbsorptionStats.Arrow);
				PlaceProgress(armorParams.SpeedRatio, armorParams.SpeedRatio, _maxSpeedRatio, _speedRatioStats.Progress, _speedRatioStats.ProgressCompare, _speedRatioStats.Arrow, true);
				_damageAbsorptionStats.ProgressCompare.gameObject.SetActive(false);
				_speedRatioStats.ProgressCompare.gameObject.SetActive(false);
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
