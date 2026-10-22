using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Mission
{
	public class SlugItemView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUseEvent__BackingField;

		[SerializeField]
		private tk2dButton _useButton;

		[SerializeField]
		private tk2dSprite _slugSprite;

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
		private ProgressBarController _damageProgresBar;

		[SerializeField]
		private ProgressBarController _speedProgresBar;

		[SerializeField]
		private ProgressBarController _splashAreaProgresBar;

		[SerializeField]
		private ProgressBarController _rechargeProgresBar;

		[SerializeField]
		private TextMesh _damageText;

		[SerializeField]
		private TextMesh _damageCountText;

		[SerializeField]
		private TextMesh _speedText;

		[SerializeField]
		private TextMesh _speedCountText;

		[SerializeField]
		private TextMesh _splashAreaText;

		[SerializeField]
		private TextMesh _splashAreaCountText;

		[SerializeField]
		private TextMesh _rechargeText;

		[SerializeField]
		private TextMesh _rechargeCountText;

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
			if (!(model is SlugItem))
			{
				return;
			}
			SlugItem slugItem = model as SlugItem;
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			SlugLevels slugParams = instance.SlugConfigManager.GetSlugParams(slugItem.Type, slugItem.Level);
			_damageProgresBar.SetProgress(slugParams.Damage / 30f, false);
			_damageCountText.text = slugParams.Damage.ToString();
			_speedProgresBar.SetProgress(slugParams.ProjectileSpeed / 20f, false);
			_speedCountText.text = slugParams.ProjectileSpeed.ToString();
			_splashAreaProgresBar.SetProgress(slugParams.HittingArea / 5f, false);
			_splashAreaCountText.text = slugParams.HittingArea.ToString();
			_rechargeProgresBar.SetProgress(slugParams.CallDown / 20f, false);
			_rechargeCountText.text = slugParams.CallDown.ToString();
			_slugSprite.SetSprite(string.Format("icon-shop_slug-{0}", slugItem.Type));
			_slugSprite.color = Color.white;
			_nameText.text = slugItem.Type.ToString();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (slugItem.Type == missionSetting.AttackSlug.Type || slugItem.Type == missionSetting.DefenceSlug.Type || slugItem.Type == missionSetting.SpecialSlug.Type)
			{
				_activeLightObject.SetActive(true);
				_useButton.gameObject.SetActive(false);
			}
			else
			{
				_activeLightObject.SetActive(false);
				_useButton.gameObject.SetActive(true);
			}
			_levelText.text = slugItem.Level.ToString();
			_upgradeBar.gameObject.SetActive(true);
			for (int i = 0; i < 5; i++)
			{
				if (i <= slugItem.Level)
				{
					_upgradeLevels[i - 1].gameObject.SetActive(true);
				}
				else
				{
					_upgradeLevels[i - 1].gameObject.SetActive(false);
				}
			}
		}
	}
}
