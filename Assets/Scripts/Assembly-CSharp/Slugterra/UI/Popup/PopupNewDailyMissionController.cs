using AppsMinistry.Core;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.Scene;
using Slugterra.Scene.Daily;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupNewDailyMissionController : UIController<PopupNewDailyMissionView>
	{
		private bool _isShowPopUp;

		protected override void OnStart()
		{
			base.OnStart();
			View.closeEvent = OnCloseDaylyMissionPopUpBtn;
		}

		private void OnBtnNewDailyMission()
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			model.MissionSetting.Mission = DailySceneManager.GetDaily().Geometry;
			SceneLoader.LoadScene<ShopSceneController>();
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			View.closeEvent = null;
		}

		private void OnCloseDaylyMissionPopUpBtn()
		{
			HidePopUp();
		}

		public void ShowPopUp()
		{
			gameObject.SetActive(true);
			_isShowPopUp = true;
			transform.DOLocalMoveY(0f, 1f).SetUpdate(true).OnComplete(() =>
			{
				_isShowPopUp = true;
			});
		}

		public void HidePopUp()
		{
			_isShowPopUp = false;
			transform.DOLocalMoveY(-1.4f, 0.35f).SetDelay(0.6f).SetUpdate(true)
				.OnComplete(() =>
				{
					gameObject.SetActive(false);
				});
		}

		private void Update()
		{
			if (_isShowPopUp && Input.touchCount > 0)
			{
				OnCloseDaylyMissionPopUpBtn();
			}
		}
	}
}
