using AppsMinistry.Core;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.UI.MainMenu
{
	public class RateMeBehaviour : MonoBehaviour
	{
		private void Start()
		{
			RateMe();
		}

		private void RateMe()
		{
			if (!PlayerPrefsSystem.HasKey("LaunchCount"))
			{
				return;
			}
			int num = PlayerPrefs.GetInt("LaunchCount");
			if (num <= 4)
			{
				return;
			}
			if (!PlayerPrefsSystem.HasKey("Rated") || PlayerPrefs.GetInt("Rated") <= 0)
			{
				string rateMeUrl = ((!PlatformUtil.IsAmazon()) ? "http://rate.appsministry.com/android/slugterra_gp" : "http://rate.appsministry.com/android/slugterra_gp");
				DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
				{
					AndroidRateUsPopUp androidRateUsPopUp = AndroidRateUsPopUp.Create(MonoSingleton<Localization>.Instance["rateme_title"], MonoSingleton<Localization>.Instance["rateme_body"], rateMeUrl, MonoSingleton<Localization>.Instance["rate_now_button_text"], MonoSingleton<Localization>.Instance["remind_me_later_button_text"], MonoSingleton<Localization>.Instance["no_thanks"]);
					androidRateUsPopUp.ActionComplete += OnRatePopUpClose;
				}, (string errorMsg) =>
				{
				});
			}
		}

		private void OnAlertButtonClicked(string buttonName)
		{
			if (buttonName == MonoSingleton<Localization>.Instance["rate_now_button_text"])
			{
				PlayerPrefs.SetInt("Rated", 2);
				PlayerPrefs.SetInt("LaunchCount", 0);
				Application.OpenURL("http://rate.appsministry.com/apple/slugterra/vote");
			}
			if (buttonName == MonoSingleton<Localization>.Instance["remind_me_later_button_text"])
			{
				PlayerPrefs.SetInt("Rated", 0);
				PlayerPrefs.SetInt("LaunchCount", 0);
			}
			if (buttonName == MonoSingleton<Localization>.Instance["no_thanks"])
			{
				PlayerPrefs.SetInt("Rated", 2);
				PlayerPrefs.SetInt("LaunchCount", 0);
			}
		}

		private void OnRatePopUpClose(AndroidDialogResult result)
		{
			switch (result)
			{
			case AndroidDialogResult.RATED:
				PlayerPrefs.SetInt("Rated", 2);
				PlayerPrefs.SetInt("LaunchCount", 0);
				break;
			case AndroidDialogResult.REMIND:
				PlayerPrefs.SetInt("Rated", 0);
				PlayerPrefs.SetInt("LaunchCount", 0);
				break;
			case AndroidDialogResult.DECLINED:
				PlayerPrefs.SetInt("Rated", 2);
				PlayerPrefs.SetInt("LaunchCount", 0);
				break;
			}
		}
	}
}
