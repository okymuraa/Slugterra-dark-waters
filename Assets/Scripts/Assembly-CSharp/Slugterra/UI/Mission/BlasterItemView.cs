using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using Slugterra.Game;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class BlasterItemView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUseEvent__BackingField;

		[SerializeField]
		private tk2dButton _useButton;

		[SerializeField]
		private GameObject _blaster;

		private GameObject _blasterPrefab;

		private Transform _bpTransform;

		private float _rotateValue;

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
		private ProgressBarController _fireRateProgresBar;

		[SerializeField]
		private ProgressBarController _speedProgresBar;

		[SerializeField]
		private ProgressBarController _damageProgresBar;

		[SerializeField]
		private TextMesh _fireRateText;

		[SerializeField]
		private TextMesh _fireRateCountText;

		[SerializeField]
		private TextMesh _speedText;

		[SerializeField]
		private TextMesh _speedCountText;

		[SerializeField]
		private TextMesh _damageText;

		[SerializeField]
		private TextMesh _damageCountText;

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
			_useButton.ButtonPressedEvent += OnBuy;
		}

		private void OnDisable()
		{
			_useButton.ButtonPressedEvent -= OnBuy;
		}

		private void OnBuy(tk2dButton source)
		{
			if (OnUseEvent__BackingField != null)
			{
				OnUseEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!(model is BlasterItem))
			{
				return;
			}
			BlasterItem blasterItem = model as BlasterItem;
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			BlasterLevels blasterParams = instance.BlasterConfigManager.GetBlasterParams(blasterItem.Type, blasterItem.Level);
			_fireRateProgresBar.SetProgress(blasterParams.FireInterval / 4f, false);
			_fireRateCountText.text = blasterParams.FireInterval.ToString();
			_speedProgresBar.SetProgress(blasterParams.ProjectileSpeed / 20f, false);
			_speedCountText.text = blasterParams.ProjectileSpeed.ToString();
			_damageProgresBar.SetProgress(blasterParams.Damage / 30f, false);
			_damageCountText.text = blasterParams.Damage.ToString();
			if (_blasterPrefab != null)
			{
				UnityEngine.Object.Destroy(_blasterPrefab);
			}
			_blasterPrefab = UnityEngine.Object.Instantiate(GameFactory.GetBlasterPrefab(blasterItem.Type), Vector3.zero, Quaternion.Euler(-90f, 120f + Mathf.Sin(_rotateValue + 30f) * 15f, 0f));
			_bpTransform = _blasterPrefab.transform;
			_bpTransform.parent = _blaster.transform;
			_bpTransform.localPosition = new Vector3(-0.5f, 0f, 0f);
			_bpTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
			_nameText.text = blasterItem.Type.ToString();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (blasterItem.Type != missionSetting.Blaster.Type)
			{
				_activeLightObject.SetActive(false);
				_useButton.gameObject.SetActive(true);
			}
			else
			{
				_activeLightObject.SetActive(true);
				_useButton.gameObject.SetActive(false);
			}
			_levelText.text = blasterItem.Level.ToString();
			_upgradeBar.gameObject.SetActive(true);
			for (int i = 1; i < 6; i++)
			{
				if (i <= blasterItem.Level)
				{
					_upgradeLevels[i - 1].gameObject.SetActive(true);
				}
				else
				{
					_upgradeLevels[i - 1].gameObject.SetActive(false);
				}
			}
		}

		private void Update()
		{
			_rotateValue += Time.deltaTime * 2f;
			if (_bpTransform != null)
			{
				_bpTransform = _blasterPrefab.transform;
				_bpTransform.localEulerAngles = new Vector3(-90f, 120f + Mathf.Sin(_rotateValue + 30f) * 15f, 0f);
			}
		}
	}
}
