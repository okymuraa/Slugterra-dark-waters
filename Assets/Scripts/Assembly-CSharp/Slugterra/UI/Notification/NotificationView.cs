using System;
using AppsMinistry.UI;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Notification
{
	public class NotificationView : UIView
	{
		[SerializeField]
		private PopupGiftForWatchAdsController giftForAdsPopUp;

		[SerializeField]
		private PopupNewDailyMissionController newDailyMissionPopUp;

		[SerializeField]
		private Transform inGameTransform;

		[SerializeField]
		private Transform mainMenuTransform;

		[SerializeField]
		private GameObject tempObject;

		private void OnEnable()
		{
			if (Application.loadedLevelName == "MainScene")
			{
				tempObject = UnityEngine.Object.Instantiate(newDailyMissionPopUp.gameObject);
				tempObject.transform.parent = mainMenuTransform;
				tempObject.transform.localPosition = new Vector3(0f, -1.4f, 0f);
				tempObject.gameObject.SetActive(false);
			}
		}

		public void ShowAdsNotification(string notificationText = "", Action callback = null)
		{
		}

		public void ShowDailyMissionNotification()
		{
		}
	}
}
