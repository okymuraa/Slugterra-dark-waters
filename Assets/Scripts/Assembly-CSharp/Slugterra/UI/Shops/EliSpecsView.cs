using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliSpecsView : UIView
	{
		[SerializeField]
		private tk2dUIProgressBar _vitalityProgress;

		[SerializeField]
		private TextMesh _vitalityCount;

		[SerializeField]
		private tk2dUIProgressBar _damageProgress;

		[SerializeField]
		private TextMesh _damageCount;

		[SerializeField]
		private tk2dUIProgressBar _speedProgress;

		[SerializeField]
		private TextMesh _speedCount;

		private float _maxVitality;

		private float _maxDamage;

		private float _maxSpeed;

		private bool _calculatedMaxParams;

		private void CalculateMaxParams()
		{
			HumanoidLevels alliesConfigurationLevel = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(AllyType.Eli, 0);
			_maxVitality = alliesConfigurationLevel.Health + alliesConfigurationLevel.Health * GetArmorMaxDamageAbsorption();
			_maxDamage = GetBlasterMaxDamage();
			_maxSpeed = alliesConfigurationLevel.RunSpeed;
		}

		private float GetArmorMaxDamageAbsorption()
		{
			float num = 0f;
			foreach (ArmorConfig armorConfig in MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfigs())
			{
				for (int i = 0; i < armorConfig.levels.Count; i++)
				{
					ArmorLevels armorLevels = armorConfig.levels[i];
					if (num < armorLevels.DamageAbsorption)
					{
						num = armorLevels.DamageAbsorption;
					}
				}
			}
			return num;
		}

		private float GetBlasterMaxDamage()
		{
			float num = 0f;
			foreach (BlasterConfig blasterConfig in MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterConfigs())
			{
				for (int i = 0; i < blasterConfig.levels.Count; i++)
				{
					BlasterLevels blasterLevels = blasterConfig.levels[i];
					if (num < blasterLevels.Damage)
					{
						num = blasterLevels.Damage;
					}
				}
			}
			return num;
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is MissionSetting)
			{
				if (!_calculatedMaxParams)
				{
					CalculateMaxParams();
					_calculatedMaxParams = true;
				}
				MissionSetting missionSetting = model as MissionSetting;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(missionSetting.Blaster.Type, missionSetting.Blaster.Level);
				ArmorLevels armorParams = instance.ArmorConfigManager.GetArmorParams(missionSetting.Armor.Type, missionSetting.Armor.Level);
				HumanoidLevels alliesConfigurationLevel = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(AllyType.Eli, 0);
				_vitalityProgress.Value = (alliesConfigurationLevel.Health + alliesConfigurationLevel.Health * armorParams.DamageAbsorption) / _maxVitality;
				_damageProgress.Value = blasterParams.Damage / _maxDamage;
				_speedProgress.Value = alliesConfigurationLevel.RunSpeed * armorParams.SpeedRatio / _maxSpeed;
				_vitalityCount.text = Mathf.RoundToInt(_vitalityProgress.Value * 100f).ToString();
				_damageCount.text = Mathf.RoundToInt(blasterParams.Damage).ToString();
				_speedCount.text = Mathf.RoundToInt(_speedProgress.Value * 100f).ToString();
			}
		}
	}
}
