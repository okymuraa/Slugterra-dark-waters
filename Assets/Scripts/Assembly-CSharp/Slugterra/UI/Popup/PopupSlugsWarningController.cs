using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupSlugsWarningController : UIController<PopupSlugsWarningView>
	{
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
			View.OnCheckClickEvent += OnCheckClickHandler;
			View.OnStartClickEvent += OnStartClickHandler;
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			View.OnCheckClickEvent -= OnCheckClickHandler;
			View.OnStartClickEvent -= OnStartClickHandler;
		}

		private void OnCheckClickHandler()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			EquipStoreView equipStoreView = Object.FindObjectOfType<EquipStoreView>();
			if (missionSetting.DefenceSlug != null && !healthRestoreManager.GetHealthConfiguration(missionSetting.DefenceSlug.Type).IsFullyRestored)
			{
				equipStoreView.SendMessage("OnBtnSlugsDefenseClick");
				View.Close();
			}
			else if (missionSetting.SpecialSlug != null && !healthRestoreManager.GetHealthConfiguration(missionSetting.SpecialSlug.Type).IsFullyRestored)
			{
				equipStoreView.SendMessage("OnBtnSlugsSpecialClick");
				View.Close();
			}
		}

		private void OnStartClickHandler()
		{
			EquipStoreView equipStoreView = Object.FindObjectOfType<EquipStoreView>();
			equipStoreView.SendMessage("OnBtnStartClick");
			View.Close();
		}
	}
}
