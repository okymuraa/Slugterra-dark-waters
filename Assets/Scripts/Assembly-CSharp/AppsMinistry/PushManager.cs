using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using UnityEngine;

namespace AppsMinistry
{
	public class PushManager : DonDestroyMonoSingleton<PushManager>
	{
		public Action createShedulesPushNotificationsCallback;

		public Action customRegisterForPushNotifications;

		private string[] _pushBodies = new string[19]
		{
			"push_text01",
			"push_text02",
			"push_text03",
			"push_text04",
			"push_text05",
			"push_text06",
			"push_text07",
			"push_text08",
			"push_text09",
			"push_text10",
			"push_text11",
			"push_text12",
			"push_text13",
			"push_text14",
			"push_text15",
			"push_text16",
			"push_text17",
			"push_text18",
			string.Empty
		};

		private List<string> _multiplayerTicketsPush = new List<string> { "push_multiplayer_tickets_text1", "push_multiplayer_tickets_text2" };

		private List<string> _dailyBonusPushBodies = new List<string> { "push_daily_bonus_text1", "push_daily_bonus_text2" };

		private IPushController _strategy;

		private bool testMode;

		private bool recreatePushNotifications;

		private int notificationIndex;

		protected override void OnCreate()
		{
			base.OnCreate();
			PushService.didReceiveRemoteNotificationEvent += DidReceiveRemoteNotification;
			PushService.didReceiveLocalNotificationEvent += DidReceiveRemoteNotification;
			AppsMinistrySdkBinding.Instance.AMEnableApplicationNotification();
			AppsMinistrySdkBinding.Instance.AppsMinistrySdkConfigure(278, "af8cd137d2bd194e8e979981c514b449");
			AppsMinistrySdkBinding.Instance.AMRichMediaStartSessionWithOptions(true, testMode, null, "landscape");
			_strategy = gameObject.AddComponent<PushController_Android>();
			Debug.Log("PushManager::OnCreate ");
			StartSession();
			CancelScheduledTasks();
		}

		private void DidReceiveRemoteNotification(string result)
		{
			Debug.Log("PushManager::DidReceiveRemoteNotification " + result);
		}

		private void DidReceiveLocalNotification(string result)
		{
			Debug.Log("PushManager::DidReceiveLocalNotification " + result);
		}

		private void OnApplicationPause(bool paused)
		{
			if (paused)
			{
				CancelScheduledTasks();
				CreateSchedulePushes();
			}
			else
			{
				CancelScheduledTasks();
			}
		}

		private void PlanningPushesWorker()
		{
			while (recreatePushNotifications && notificationIndex < 60)
			{
				switch (notificationIndex)
				{
				case 0:
					SchedulRandomBodyePush(1);
					break;
				case 1:
					SchedulRandomBodyePush(4);
					break;
				case 2:
					SchedulRandomBodyePush(7);
					break;
				case 3:
					SchedulRandomBodyePush(10);
					break;
				default:
					SchedulRandomBodyePush((notificationIndex - 3) * 5 + 10);
					break;
				}
				notificationIndex++;
				if (notificationIndex >= 60)
				{
					recreatePushNotifications = false;
					if (createShedulesPushNotificationsCallback != null)
					{
						createShedulesPushNotificationsCallback();
					}
					Debug.Log("PushManager::PlanningPushesWorker Already Finished" + recreatePushNotifications);
					break;
				}
			}
		}

		private void CreateDailyBonusShedule()
		{
			string empty = string.Empty;
			DateTime result;
			if (PlayerPrefsSystem.HasKey("NextDailyBonus") && DateTime.TryParse(PlayerPrefs.GetString("NextDailyBonus"), out result) && result.CompareTo(DateTime.Today) > 0)
			{
				CreateLocalPush("Slugterra", Localization.GetValue(_dailyBonusPushBodies.PeekRandomItem(), string.Empty), empty, result.AddHours(9.0));
			}
		}

		private void CreateTicketsShedule()
		{
			string empty = string.Empty;
			if (PlayerPrefsSystem.HasKey("NextTicketsDate"))
			{
				DateTime now = DateTime.Now;
				Debug.Log("today = " + now.ToString());
				DateTime result;
				if (DateTime.TryParse(PlayerPrefs.GetString("NextTicketsDate"), out result) && result.Ticks >= now.Ticks)
				{
					Debug.Log("nextDate = " + result.ToString() + " localTime " + result.ToLocalTime());
					CreateLocalPush("Slugterra", Localization.GetValue(_multiplayerTicketsPush.PeekRandomItem(), string.Empty), empty, result, false);
				}
			}
		}

		private void CreateHealthRestorationShedule()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			CreateHealthRestorationShedule(missionSetting.Blaster.Type, "push_blaster_restored");
			CreateHealthRestorationShedule(missionSetting.AttackSlug.Type, "push_slug_recovered");
			if (missionSetting.DefenceSlug != null)
			{
				CreateHealthRestorationShedule(missionSetting.DefenceSlug.Type, "push_slug_recovered");
			}
			if (missionSetting.SpecialSlug != null)
			{
				CreateHealthRestorationShedule(missionSetting.SpecialSlug.Type, "push_slug_recovered");
			}
			if (missionSetting.Assistant != null)
			{
				CreateHealthRestorationShedule(missionSetting.Assistant.Type, "push_companion_healed");
			}
		}

		private void CreateHealthRestorationShedule(Enum restoreType, string localizationKey, double minMinuteLimit = 10.0)
		{
			string empty = string.Empty;
			RestoreHealthConfiguration restoreHealthConfigurationManager = MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			AbstractHealthData healthConfiguration = restoreHealthConfigurationManager.GetHealthConfiguration(restoreType);
			if (!healthConfiguration.IsFullyRestored && healthConfiguration.TimeToRestore.TotalMinutes > minMinuteLimit)
			{
				DateTime fireDate = healthConfiguration.TimeWhenObjectIsRestored.ToLocalTime();
				CreateLocalPush("Slugterra", Localization.GetValue(localizationKey, string.Empty), empty, fireDate);
			}
		}

		private void CreateSchedulePushes()
		{
			recreatePushNotifications = true;
			Debug.Log("PushManager::CreateSchedulePushes " + recreatePushNotifications);
			notificationIndex = 0;
			PlanningPushesWorker();
			CreateDailyBonusShedule();
			CreateTicketsShedule();
		}

		private void SchedulRandomBodyePush(int days)
		{
			DateTime dateTime = DateTime.Today.AddHours(12.0);
			string empty = string.Empty;
			if (days > 0)
			{
				dateTime = dateTime.AddDays(days);
				CreateLocalPush("Slugterra", GetRandomNameBody(), empty, dateTime.ToLocalTime());
			}
			else
			{
				Debug.Log("LocalPushManager : Push time can not be less \"0\"");
			}
		}

		private string GetRandomNameBody()
		{
			int num = UnityEngine.Random.Range(0, _pushBodies.Length - 1);
			string key = _pushBodies[num];
			return MonoSingleton<Localization>.Instance.GetValueByKey(key);
		}

		public void StartSession()
		{
			if (_strategy == null)
			{
				Debug.LogWarning("PushManager::StartSession. Create Strategy before start session");
				return;
			}
			int num = PlayerPrefs.GetInt("NotificationAllow", 0);
			Debug.Log("PushManager::StartSession is Notification Allowed: " + num);
			if (num == 1)
			{
				_strategy.Initialize();
				if (customRegisterForPushNotifications != null)
				{
					customRegisterForPushNotifications();
				}
				AppsMinistrySdkBinding.Instance.AMPushServiceStartSession(278, "af8cd137d2bd194e8e979981c514b449", "870032739341");
			}
		}

		public void PresentLocalNotificationNow(string alertAction, string alertBody, string soundName)
		{
			Debug.Log("PushManager::PresentLocalNotificationNow " + recreatePushNotifications);
			_strategy.PresentLocalNotificationNow(alertAction, alertBody, soundName);
		}

		public void CreateLocalPush(string alertAction, string alertBody, string soundName, DateTime fireDate, bool convertToLocalTime = true)
		{
			if (convertToLocalTime)
			{
				_strategy.CreateLocalPush(alertAction, alertBody, soundName, fireDate.ToLocalTime());
			}
			else
			{
				_strategy.CreateLocalPush(alertAction, alertBody, soundName, fireDate);
			}
		}

		public void CancelScheduledTasks()
		{
			_strategy.CancelScheduledTasks();
		}
	}
}
