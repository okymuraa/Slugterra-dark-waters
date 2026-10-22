using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using Slugterra.Game;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EliShopView : UIView
	{
		[SerializeField]
		private Renderer _armorRenderer;

		[SerializeField]
		private Transform _blasterAnchor;

		[SerializeField]
		private GameObject _defaultBlaster;

		[SerializeField]
		private float _rotationDelay = 3f;

		[SerializeField]
		private float _rotationSpeed = 25f;

		private GameObject _currentBlaster;

		private float _rotationStartTime;

		private bool _backwardRotation;

		private Quaternion _baseRotation;

		protected override void OnStart()
		{
			base.OnStart();
			_baseRotation = transform.rotation;
		}

		private void OnEnable()
		{
			_rotationStartTime = Time.time + _rotationDelay;
		}

		private void SetArmor(ArmorItem armor)
		{
			if (armor != null)
			{
				_armorRenderer.material = GameFactory.GetArmorMaterial(armor.Type);
			}
		}

		private void SetBlaster(BlasterItem blaster)
		{
			if (blaster != null)
			{
				if (_defaultBlaster != null)
				{
					_defaultBlaster.SetActive(false);
				}
				if (_currentBlaster != null)
				{
					Object.DestroyImmediate(_currentBlaster);
				}
				LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission);
				if (levelItem != null && (levelItem.Scene == typeof(ShaneTutor_0_1) || levelItem.Scene == typeof(ScorchedSparksTutorial_1_1) || levelItem.Scene == typeof(ScorchedSparks_1_2) || levelItem.Scene == typeof(QuietLawn_2_1) || levelItem.Scene == typeof(QuietLawn_2_2)))
				{
					_currentBlaster = GameFactory.GetBlasterPrefab(BlasterType.LeaveBlaster);
				}
				else
				{
					_currentBlaster = GameFactory.GetBlasterPrefab(blaster.Type);
				}
				if (_currentBlaster != null)
				{
					_currentBlaster = Object.Instantiate(_currentBlaster);
					_currentBlaster.transform.parent = _blasterAnchor;
					_currentBlaster.transform.localPosition = Vector3.zero;
					_currentBlaster.transform.localEulerAngles = Vector3.zero;
				}
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is MissionSetting)
			{
				MissionSetting missionSetting = model as MissionSetting;
				SetArmor(missionSetting.Armor);
				SetBlaster(missionSetting.Blaster);
			}
		}

		private void OnMouseDown()
		{
			_rotationStartTime = Time.time + _rotationDelay;
			_backwardRotation = true;
		}

		private void LateUpdate()
		{
			if (_rotationStartTime <= Time.time)
			{
				if (_backwardRotation)
				{
					_backwardRotation = false;
					transform.rotation = _baseRotation;
				}
				transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);
			}
			else if (_backwardRotation)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, _baseRotation, Time.unscaledDeltaTime * Mathf.Abs(_rotationSpeed));
			}
		}
	}
}
