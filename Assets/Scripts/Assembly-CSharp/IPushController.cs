using System;

public interface IPushController
{
	void CancelScheduledTasks();

	void Initialize();

	void CreateLocalPush(string alertAction, string alertBody, string soundName, DateTime fireDate);

	void PresentLocalNotificationNow(string alertAction, string alertBody, string soundName);
}
