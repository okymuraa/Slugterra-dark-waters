using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using Slugterra.UI.Multiplayer;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugInfoView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnInfoClickEvent__BackingField;

		[SerializeField]
		protected TextMesh _title;

		[SerializeField]
		protected TextMesh _description;

		[SerializeField]
		private ItemDashboardStats _damageStats;

		[SerializeField]
		private ItemDashboardStats _speedStats;

		[SerializeField]
		private ItemDashboardStats _splashAreaStats;

		[SerializeField]
		private ItemDashboardStats _rechargeStats;

		[SerializeField]
		private SlugItem _item;

		private float _maxDamage;

		private float _maxHittingArea;

		private float _maxCallDown;

		private float _maxProjectileSpeed;

		[SerializeField]
		private PreviewSlugs _slugPreview;

		[SerializeField]
		private GameObject _UIEquip;

		[SerializeField]
		private GameObject _slugPreviewCam;

		[SerializeField]
		private GameObject _slugPreviewUI;

		[SerializeField]
		private tk2dButton _btnBack;

		private readonly Color CUSTOM_COLOR_RED = new Color(1f, 0f, 30f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_GREEN = new Color(0f, 226f / 255f, 50f / 255f, 1f);

		private readonly Color CUSTOM_COLOR_YELLOW = new Color(1f, 229f / 255f, 0f, 1f);

		public event Action OnBtnInfoClickEvent
		{
			add
			{
				Action action = OnBtnInfoClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnInfoClickEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnInfoClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnInfoClickEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnEnable()
		{
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			_maxDamage = (_maxProjectileSpeed = (_maxHittingArea = (_maxCallDown = 0f)));
			foreach (SlugConfig slugConfig in instance.SlugConfigManager.GetSlugConfigs())
			{
				for (int i = 0; i < slugConfig.levels.Count; i++)
				{
					SlugLevels slugLevels = slugConfig.levels[i];
					if (_maxDamage < slugLevels.Damage)
					{
						_maxDamage = slugLevels.Damage;
					}
					if (_maxProjectileSpeed < slugLevels.ProjectileSpeed)
					{
						_maxProjectileSpeed = slugLevels.ProjectileSpeed;
					}
					if (_maxHittingArea < slugLevels.HittingArea)
					{
						_maxHittingArea = slugLevels.HittingArea;
					}
					if (_maxCallDown < slugLevels.CallDown)
					{
						_maxCallDown = slugLevels.CallDown;
					}
				}
			}
		}

		private void OnDisable()
		{
		}

		protected override void OnStart()
		{
			if (_slugPreview == null)
			{
				_slugPreview = UnityEngine.Object.FindObjectOfType<PreviewSlugs>();
			}
			if (_UIEquip == null && (bool)UnityEngine.Object.FindObjectOfType<BackpackController>())
			{
				_UIEquip = UnityEngine.Object.FindObjectOfType<BackpackController>().gameObject;
			}
			if (_UIEquip == null && (bool)UnityEngine.Object.FindObjectOfType<LobbyBackpackController>())
			{
				_UIEquip = UnityEngine.Object.FindObjectOfType<LobbyBackpackController>().gameObject;
			}
			if (_slugPreviewCam == null)
			{
				_slugPreviewCam = GameObject.Find("SlugPreviewRoot").transform.Find("Misc/SlugPreviewDetector/Animaton/SlugCamera").gameObject;
			}
			if (_slugPreviewUI == null)
			{
				_slugPreviewUI = GameObject.Find("UIEquip").transform.Find("SlugPreview").gameObject;
			}
			if (_btnBack == null)
			{
				_btnBack = _slugPreviewUI.transform.Find("UpperLeft/btnBackGameService/btnBack/btn").GetComponent<tk2dButton>();
			}
			if (_btnBack != null)
			{
				_btnBack.ButtonUpEvent += OnBtnBackClick;
			}
		}

		protected override void OnReleaseResource()
		{
			if (_btnBack != null)
			{
				_btnBack.ButtonUpEvent -= OnBtnBackClick;
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (gameObject.activeSelf && model is SlugItem)
			{
				_item = model as SlugItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				SlugConfig slugConfiguration = instance.SlugConfigManager.GetSlugConfiguration(_item.Type);
				_title.text = Localization.GetValue(slugConfiguration.title, string.Empty);
				_description.text = Localization.GetValue(slugConfiguration.description, string.Empty);
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				SlugLevels slugParams = instance.SlugConfigManager.GetSlugParams(_item.Type, _item.Level);
				PlaceProgress(slugParams.Damage, slugParams.Damage, _maxDamage, _damageStats.Progress, _damageStats.ProgressCompare, _damageStats.Arrow);
				PlaceProgress(slugParams.ProjectileSpeed, slugParams.ProjectileSpeed, _maxProjectileSpeed, _speedStats.Progress, _speedStats.ProgressCompare, _speedStats.Arrow);
				PlaceProgress(slugParams.HittingArea, slugParams.HittingArea, _maxHittingArea, _splashAreaStats.Progress, _splashAreaStats.ProgressCompare, _splashAreaStats.Arrow);
				PlaceProgress(slugParams.CallDown, slugParams.CallDown, _maxCallDown, _rechargeStats.Progress, _rechargeStats.ProgressCompare, _rechargeStats.Arrow, true);
				_damageStats.ProgressCompare.gameObject.SetActive(false);
				_speedStats.ProgressCompare.gameObject.SetActive(false);
				_splashAreaStats.ProgressCompare.gameObject.SetActive(false);
				_rechargeStats.ProgressCompare.gameObject.SetActive(false);
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

		private void OnBtnPreviewClick()
		{
			if (_item != null)
			{
				if (OnBtnInfoClickEvent__BackingField != null)
				{
					OnBtnInfoClickEvent__BackingField();
				}
				_UIEquip.SetActive(false);
				_slugPreviewUI.SetActive(true);
				_slugPreviewCam.SetActive(true);
				_slugPreview.Type = _item.Type;
				_slugPreview.Attack();
			}
		}

		private void OnBtnBackClick(tk2dButton source)
		{
			_UIEquip.SetActive(true);
			_slugPreviewUI.SetActive(false);
			_slugPreviewCam.SetActive(false);
			_slugPreview.StopElliFire();
		}
	}
}
