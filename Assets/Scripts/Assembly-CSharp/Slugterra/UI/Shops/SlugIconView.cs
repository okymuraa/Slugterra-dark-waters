using AppsMinistry.Core;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.Configuration;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugIconView : UIView
	{
		private static readonly Color32 RECOVERY_LEVEL_COLOR = new Color32(byte.MaxValue, 0, 30, byte.MaxValue);

		private static readonly Color32 NORMAL_LEVEL_COLOR = new Color32(185, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public const float ScaleAnimationTime = 0.35f;

		[SerializeField]
		private tk2dSprite _slugIcon;

		[SerializeField]
		private tk2dSprite _containerIcon;

		[SerializeField]
		private GameObject _iconRoot;

		[SerializeField]
		private GameObject _selectedFrame;

		[SerializeField]
		private GameObject _deselectedFrame;

		[SerializeField]
		private GameObject _normalBackground;

		[SerializeField]
		private GameObject _recoveryBackground;

		[SerializeField]
		private GameObject _glowAnimation;

		[SerializeField]
		private TextMesh _badge;

		[SerializeField]
		private tk2dSlicedSprite _badgeRoot;

		[SerializeField]
		private GameObject _alertBadge;

		[SerializeField]
		private tk2dUIProgressBar _cooldownBar;

		private bool _selected;

		private Tweener _transformTweener;

		private SlugItem _slug;

		private SlugLevels _slugConfig;

		private AbstractHealthData _healthData;

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
			healthRestoreManager.OnUpdateValues += OnHealthValuesUpdateHandler;
		}

		private void OnDisable()
		{
			if (healthRestoreManager != null)
			{
				healthRestoreManager.OnUpdateValues -= OnHealthValuesUpdateHandler;
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is SlugItem)
			{
				_slug = model as SlugItem;
				_healthData = healthRestoreManager.GetHealthConfiguration(_slug.Type);
				_slugConfig = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugParams(_slug.Type, _slug.Level);
				SetViewState(_slug);
			}
		}

		private void SetViewState(SlugItem slug)
		{
			Color color = _slugIcon.color;
			if (slug == null || slug.Type == SlugType.None)
			{
				_badge.text = string.Empty;
				_alertBadge.SetActive(false);
				_slugIcon.gameObject.SetActive(false);
				_badgeRoot.SetSprite("message_grey");
				_cooldownBar.gameObject.SetActive(false);
				_normalBackground.SetActive(true);
				_recoveryBackground.SetActive(false);
			}
			else if (_healthData != null && !_healthData.IsFullyRestored)
			{
				_badge.text = TextUtils.GetSlugLevel(slug.Level);
				_badge.color = RECOVERY_LEVEL_COLOR;
				_alertBadge.SetActive(false);
				_slugIcon.gameObject.SetActive(true);
				_slugIcon.SetSprite(SpriteNamesHelper.GetSlugSpriteName(slug.Type) + "_grey");
				_badgeRoot.SetSprite("message_red");
				_cooldownBar.gameObject.SetActive(true);
				_normalBackground.SetActive(false);
				_recoveryBackground.SetActive(true);
			}
			else
			{
				_badge.text = TextUtils.GetSlugLevel(slug.Level);
				_badge.color = NORMAL_LEVEL_COLOR;
				_alertBadge.SetActive(false);
				_slugIcon.gameObject.SetActive(true);
				_slugIcon.SetSprite(SpriteNamesHelper.GetSlugSpriteName(slug.Type));
				_badgeRoot.SetSprite("message_indigo");
				_cooldownBar.gameObject.SetActive(false);
				_normalBackground.SetActive(true);
				_recoveryBackground.SetActive(false);
			}
			_slugIcon.color = color;
		}

		private void Update()
		{
			if (_healthData != null && _slugConfig != null && !_healthData.IsFullyRestored)
			{
				float value = 1f - (float)_healthData.TimeToRestore.TotalMinutes / _slugConfig.restorationTimeInMinutes;
				_cooldownBar.Value = value;
			}
		}

		public void Select(bool value)
		{
			if (_selected != value)
			{
				_selectedFrame.SetActive(value);
				_glowAnimation.SetActive(value);
				_deselectedFrame.SetActive(!value);
				_selected = value;
			}
		}

		private void OnHealthValuesUpdateHandler()
		{
			SetViewState(_slug);
		}
	}
}
