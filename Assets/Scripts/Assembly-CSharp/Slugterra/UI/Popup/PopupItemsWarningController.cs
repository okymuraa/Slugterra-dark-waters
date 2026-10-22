using System;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupItemsWarningController : UIController<PopupItemsWarningView>
	{
		private Action _transitionAction;

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			}
		}

		protected override void _OnEnable()
		{
			base._OnEnable();
			View.OnOkClickEvent += OnOkClick;
			SetPopupState();
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			View.OnOkClickEvent -= OnOkClick;
		}

		private void SetPopupState()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			BlasterItem blaster = missionSetting.Blaster;
			SlugItem attackSlug = missionSetting.AttackSlug;
			EquipStoreView equipStoreView = UnityEngine.Object.FindObjectOfType<EquipStoreView>();
			if (!healthRestoreManager.GetHealthConfiguration(blaster.Type).IsFullyRestored)
			{
				_transitionAction = () =>
				{
					equipStoreView.SendMessage("OnBtnWeaponClickExternal");
				};
			}
			else
			{
				_transitionAction = () =>
				{
					equipStoreView.SendMessage("OnBtnSlugsAtackClick");
				};
			}
		}

		private void OnOkClick()
		{
			if (_transitionAction != null)
			{
				_transitionAction();
			}
			View.gameObject.SetActive(false);
		}
	}
}
