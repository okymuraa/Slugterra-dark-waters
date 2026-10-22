using System;
using SA.Common.Pattern;
using UnityEngine;

namespace AppsMinistry
{
	public class PushController_Android : MonoBehaviour, IPushController
	{
		public void CancelScheduledTasks()
		{
			Singleton<AndroidNotificationManager>.Instance.CancelAllLocalNotifications();
		}

		public void CreateLocalPush(string alertAction, string alertBody, string soundName, DateTime fireDate)
		{
			TimeSpan timeSpan = fireDate - DateTime.Now;
			Singleton<AndroidNotificationManager>.Instance.ScheduleLocalNotification(alertAction, alertBody, (int)timeSpan.TotalSeconds);
		}

		public void PresentLocalNotificationNow(string alertAction, string alertBody, string soundName)
		{
			Singleton<AndroidNotificationManager>.Instance.ScheduleLocalNotification(alertAction, alertBody, 0);
		}

		public void Initialize()
		{
		}
	}
}
