using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugItemFacade : ItemFacade
	{
		[SerializeField]
		private SlugBarViewController _slugBar;

		private SlugItem _model;

		private SlugGroup _modelGroup;

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

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_slugBar.gameObject.SetActive(true);
			healthRestoreManager.OnUpdateValues += OnRestorationValuesUpdatedHandler;
		}

		protected override void OnDisable()
		{
			_slugBar.gameObject.SetActive(false);
			if (healthRestoreManager != null)
			{
				healthRestoreManager.OnUpdateValues -= OnRestorationValuesUpdatedHandler;
			}
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			base.OnDisable();
		}

		protected override void OnApplyModel(object model)
		{
			if (model is SlugItem)
			{
				if (_model != null)
				{
					_model.RemoveObserver(this);
				}
				_model = model as SlugItem;
				_model.AddObserver(this);
				_modelGroup = SlugTypeHelper.GetGroup(_model.Type);
				_slugBar.Select(_modelGroup);
			}
		}

		protected override void OnMissionSettingChanged(MissionSetting settings)
		{
		}

		private void OnRestorationValuesUpdatedHandler()
		{
		}

		private void CheckHealthRestorationState()
		{
			if (!healthRestoreManager.GetHealthConfiguration(_model.Type).IsFullyRestored && _model.IsBought && IsCharged(_model))
			{
			}
		}

		private void SetChargeStyle()
		{
		}

		private void SetDischargeStyle()
		{
		}

		private void OnChargeOrDischarge()
		{
			if (IsCharged(_model))
			{
				SetDefaultSlug(SlugTypeHelper.GetGroup(_model.Type));
			}
			else
			{
				_missionSetting.SetSlug(_model);
			}
		}

		private void SetDefaultSlug(SlugGroup slugGroup)
		{
			switch (slugGroup)
			{
			case SlugGroup.Attack:
				_missionSetting.SetDefaultAttackSlug(MonoSingleton<AppController>.Instance.Model.ShopModel);
				break;
			case SlugGroup.Control:
				_missionSetting.SetDefaultDefenceSlug();
				break;
			case SlugGroup.Special:
				_missionSetting.SetDefaultSpecialSlug();
				break;
			}
		}

		private bool IsCharged(SlugItem slug)
		{
			if (slug != null)
			{
				SlugGroup slugGroup = SlugTypeHelper.GetGroup(slug.Type);
				SlugItem slugByGroup = _missionSetting.GetSlugByGroup(slugGroup);
				if (slugByGroup != null)
				{
					return slug.Type == slugByGroup.Type;
				}
			}
			return false;
		}
	}
}
