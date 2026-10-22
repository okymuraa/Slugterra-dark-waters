using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using Facebook.Unity;
using KHD;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Analytics;

namespace AppsMinistry.Plugins
{
	[AppSingleton]
	public class AnalyticsManager : DonDestroyMonoSingleton<AnalyticsManager>
	{
		private string _oldSceneName;

		private bool _debugLog;

		public bool multiplayerWithBot;

		private DateTime _initialDateTime;

		public string currentLevel = string.Empty;

		private static Action _validateSuccess;

		private static Action<string> _validateFailed;

		protected override void OnCreate()
		{
			_initialDateTime = DateTime.Now;
			int num = PlayerPrefs.GetInt("FIRST_LAUNCH_TIMESTAMP", 0);
			if (num <= 0)
			{
				PlayerPrefs.SetInt("FIRST_LAUNCH_TIMESTAMP", unix_timestamp());
				PlayerPrefs.Save();
			}
			AppsFlyer.init("JH3ndRA4Xtt822t9jSidZP");
			AppsFlyer.setAppID("com.appsministry.slugterra");
			AppsFlyer.createValidateInAppListener(gameObject.name, "onInAppBillingSuccess", "onInAppBillingFailure");
			Debug.Log("AppsFlyerId = " + AppsFlyer.getAppsFlyerId());
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.SetAppVersion(Application.version);
			SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.StartSession("FJZHBVWFYRBZR5NW9NPF", "HBNRSXPB7YN437W56QY9", true);
		}

		protected override void OnReleaseResource()
		{
		}

		public int unix_timestamp()
		{
			return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
		}

		public int GetSecondsFromStart()
		{
			return (DateTime.Now - _initialDateTime).Seconds;
		}

		public int GetMinutesFromFirstStart()
		{
			return (unix_timestamp() - PlayerPrefs.GetInt("FIRST_LAUNCH_TIMESTAMP", 0)) / 60;
		}

		public void TrackAppsFlyerAdsAttribution()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("af_placement", currentLevel);
			dictionary.Add("af_network", "Appodeal");
			dictionary.Add("af_result", "Finished");
			AppsFlyer.trackRichEvent("af_ad_watched", dictionary);
		}

		public void ShowVideoAds(string screenName)
		{
			LogEvent("ShowVideoAds", screenName, GetMinutesFromFirstStart().ToString());
			FlurryAnalyticsLogEventParams("ShowVideAds", new Dictionary<string, string>
			{
				{ "ScreenName", screenName },
				{ "LevelName", currentLevel },
				{
					"Minutes From Start",
					GetMinutesFromFirstStart().ToString()
				}
			});
		}

		public void ShowInterstitialAds(string screenName)
		{
			LogEvent("ShowInterstitialAds", screenName, GetMinutesFromFirstStart().ToString());
			FlurryAnalyticsLogEventParams("ShowInterstitialAds", new Dictionary<string, string>
			{
				{ "ScreenName", screenName },
				{ "LevelName", currentLevel },
				{
					"Minutes From Start",
					GetMinutesFromFirstStart().ToString()
				}
			});
		}

		public void ShowIncentivizedAds(string screenName)
		{
			LogEvent("ShowIncentivizedAds", screenName, GetMinutesFromFirstStart().ToString());
			FlurryAnalyticsLogEventParams("ShowIncentivizedAds", new Dictionary<string, string>
			{
				{ "ScreenName", screenName },
				{ "LevelName", currentLevel },
				{
					"Minutes From Start",
					GetMinutesFromFirstStart().ToString()
				}
			});
		}

		public void TrackAdsStatus(string AdsType, string status)
		{
			LogEvent(AdsType, status, GetMinutesFromFirstStart().ToString());
		}

		public void StartLevel()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "GameStatus", "Play" } });
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["GameStatus"] = "Play";
				FB.LogAppEvent(currentLevel, null, dictionary);
			}
		}

		public void EnterOnLevel(string levelName)
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams("EnterLevel", new Dictionary<string, string>
				{
					{ "LevelName", levelName },
					{
						"Minutes From Start",
						GetMinutesFromFirstStart().ToString()
					}
				});
			}
		}

		public void LevelFailed()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "GameStatus", "Failed" } });
			}
		}

		public void LevelComplete()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "GameStatus", "Complete" } });
			}
		}

		public void LevelUncomplete()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "GameStatus", "UnComplete" } });
			}
		}

		public void ExitFromLevel()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "GameStatus", "Exit" } });
			}
		}

		public void RestartLevel()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "GameStatus", "Restart" } });
			}
		}

		public void RespawnOnLevel(int respawnCount)
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { 
				{
					"RespawnForMoney",
					respawnCount.ToString()
				} });
			}
		}

		public void RespawnOnLevelForAds(int respawnCount)
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { 
				{
					"RespawnForAds",
					respawnCount.ToString()
				} });
			}
		}

		public void BonusForWin()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "Bonus", "Win" } });
			}
		}

		public void BonusForLoose()
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "Bonus", "Loose" } });
			}
		}

		public void InAppPurchased(string inAppID)
		{
			FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { { "InApp", inAppID } });
		}

		public void TrackPurchase(string eventName, string inAppID, string state)
		{
			FlurryAnalyticsLogEventParams(eventName, new Dictionary<string, string> { { inAppID, state } });
		}

		public void MultiplayerLevelWon(bool playWithBot = false)
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { 
				{
					"MultiplayerGameStatus",
					(!playWithBot) ? "OpponentLoose" : "BotLoose"
				} });
				LogEvent("MultiplayerGameStatus", (!playWithBot) ? "OpponentLoose" : "BotLoose", string.Empty);
			}
		}

		public void MultiplayerLevelLoose(bool playWithBot = false)
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { 
				{
					"MultiplayerGameStatus",
					(!playWithBot) ? "OpponentWin" : "BotWin"
				} });
				LogEvent("MultiplayerGameStatus", (!playWithBot) ? "OpponentWin" : "BotWin", string.Empty);
			}
		}

		public void TrackMultiplayerParameters(bool playWithBot = false)
		{
			if (!(currentLevel == "BackpackSceneController") && !(currentLevel == "ShopSceneController"))
			{
				FlurryAnalyticsLogEventParams(currentLevel, new Dictionary<string, string> { 
				{
					"PlayWithBot",
					playWithBot.ToString()
				} });
				LogEvent(currentLevel, playWithBot.ToString(), string.Empty);
			}
		}

		public static void InitAnalytics()
		{
		}

		public static void LogException(string description, bool isFatal = false)
		{
			GoogleAnalyticsV4.instance.LogException(description, isFatal);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Desription", description);
			dictionary.Add("isFatal", isFatal.ToString());
		}

		public static void LogScene(string screenName)
		{
			GoogleAnalyticsV4.instance.LogScreen(screenName);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("sceneOpened", screenName);
		}

		public static void LogEvent(string category, string action, string label = "", int value = 0)
		{
			GoogleAnalyticsV4.instance.LogEvent(category, action, label, value);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("action", action);
			dictionary.Add("label", label);
			dictionary.Add("value", value.ToString());
		}

		public static void LogInApp(string transactionId, string name, string SKU, float price, int quantity = 1, string category = "", string currencyCode = "")
		{
			GoogleAnalyticsV4.instance.LogItem(transactionId, name, SKU, category, price, quantity, currencyCode);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("transactionId", transactionId);
			dictionary.Add("productName", name);
			dictionary.Add("productId", SKU);
			dictionary.Add("price", price.ToString());
			dictionary.Add("currencyCode", currencyCode);
			UnityAnalyticsLogTransaction(transactionId, (decimal)price, currencyCode, null, null);
			if (category == "BuyServerSuccess")
			{
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				dictionary2.Add("af_revenue", ((int)price/*cast due to constrained. prefix*/).ToString());
				dictionary2.Add("af_content_type", "inApps");
				dictionary2.Add("af_content_id", SKU);
				dictionary2.Add("af_currency", currencyCode);
				dictionary2.Add("af_quantity", "1");
				Debug.Log("Send InApp Analytics To AppsFlyer");
				AppsFlyer.trackRichEvent("af_purchase", dictionary2);
				string text = JsonConvert.SerializeObject(dictionary2);
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				dictionary3["productId"] = SKU;
				FB.LogPurchase(price, currencyCode, dictionary3);
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.InAppPurchased(SKU);
			}
		}

		private static void UnityAnalyticsLogEventParams(string eventName, Dictionary<string, string> dict)
		{
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, string> item in dict)
			{
				dictionary.Add(item.Key, item.Value);
			}
			Analytics.CustomEvent(eventName, dictionary);
		}

		private static void FlurryAnalyticsLogEventParams(string eventName, Dictionary<string, string> dict, bool isTimed = false)
		{
			if (dict == null)
			{
				SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.LogEvent(eventName);
			}
			else
			{
				SingletonCrossSceneAutoCreate<FlurryAnalytics>.Instance.LogEventWithParameters(eventName, dict, isTimed);
			}
		}

		private static void UnityAnalyticsLogTransaction(string productId, decimal price, string currency, string receipt, string signature)
		{
			Analytics.Transaction(productId, price, currency, receipt, signature);
		}

		public void AppsFlyerValidatePurchase(string publicKey, string originalJSON, string signature, string price, string currency, Dictionary<string, string> extraParams, Action validateSuccess, Action<string> validateFailed)
		{
			_validateSuccess = validateSuccess;
			_validateFailed = validateFailed;
			AppsFlyer.validateReceipt("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwalJH+sXbT8++wOUgsj61xw5ksRag/9cDBsgqvejh7RpCaUsO5SMaw73Stxe3pm/DAcfEOUph8ueGxgeV08gaxjhhGG5Qquy4JpTKD1PuBvbWny+kvbzX5Icu4nfSVs4/vyNc1cQ2eqNBU+hArPa7v6FuDiIblNvBT381Z2N6P3MdeohJdNTWiHB/PC2GT1FHPO77u4Ip66aVoAOeKCFOlhC6y76hdGnfvL6VB7MBoqQX9At+71LTTbmAlJI7yWvXQf6UcxMR653j8KjHuvZgGiarAMw/Nwv9S8KqG8G1keHpN+LR9tOY8Oa2zm7dTInYlf38MHR/scovXm/gOeA0QIDAQAB", originalJSON, signature, price, currency, null);
		}

		public void onInAppBillingSuccess()
		{
			Debug.Log("GoogleMarketStrategy_ANP::onInAppBillingSuccess Success");
			if (_validateSuccess != null)
			{
				_validateSuccess();
			}
			_validateSuccess = null;
			_validateFailed = null;
		}

		public void onInAppBillingFailure(string error)
		{
			Debug.Log("GoogleMarketStrategy_ANP::onInAppBillingFailure Failure " + error);
			if (_validateFailed != null)
			{
				_validateFailed(error);
			}
			_validateSuccess = null;
			_validateFailed = null;
		}
	}
}
