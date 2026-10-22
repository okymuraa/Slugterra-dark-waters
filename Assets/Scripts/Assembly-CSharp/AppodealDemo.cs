using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

public class AppodealDemo : MonoBehaviour, IInterstitialAdListener, IBannerAdListener, INonSkippableVideoAdListener, IRewardedVideoAdListener, IPermissionGrantedListener
{
	private string appKey = "fee50c333ff3825fd6ad6d38cff78154de3025546d47a84f";

	private string interstitialLabel = "CACHE INTERSTITIAL";

	private int buttonWidth;

	private int buttonHeight;

	private int toggleSize;

	private GUIStyle buttonStyle;

	private bool testingToggle;

	private bool loggingToggle;

	private void Awake()
	{
		Appodeal.requestAndroidMPermissions(this);
	}

	public void Init()
	{
		if (loggingToggle)
		{
			Appodeal.setLogLevel(Appodeal.LogLevel.Verbose);
		}
		else
		{
			Appodeal.setLogLevel(Appodeal.LogLevel.None);
		}
		Appodeal.setTesting(testingToggle);
		UserSettings userSettings = new UserSettings();
		userSettings.setAge(25).setGender(UserSettings.Gender.OTHER).setUserId("best_user_ever");
		Appodeal.disableNetwork("appnext");
		Appodeal.disableNetwork("amazon_ads", 4);
		Appodeal.disableLocationPermissionCheck();
		Appodeal.disableWriteExternalStoragePermissionCheck();
		Appodeal.setTriggerOnLoadedOnPrecache(3, true);
		Appodeal.setSmartBanners(true);
		Appodeal.setBannerAnimation(false);
		Appodeal.setTabletBanners(false);
		Appodeal.setBannerBackground(false);
		Appodeal.setChildDirectedTreatment(false);
		Appodeal.muteVideosIfCallsMuted(true);
		Appodeal.setAutoCache(3, false);
		Appodeal.initialize(appKey, 195);
		Appodeal.setBannerCallbacks(this);
		Appodeal.setInterstitialCallbacks(this);
		Appodeal.setRewardedVideoCallbacks(this);
		Appodeal.setCustomRule("newBoolean", true);
		Appodeal.setCustomRule("newInt", 1234567890);
		Appodeal.setCustomRule("newDouble", 123.123456789);
		Appodeal.setCustomRule("newString", "newStringFromSDK");
	}

	private void OnGUI()
	{
		InitStyles();
		if (GUI.Toggle(new Rect(Screen.width / 10, Screen.height / 10 - Screen.height / 15, toggleSize * 3, toggleSize), testingToggle, new GUIContent("Testing")))
		{
			testingToggle = true;
		}
		else
		{
			testingToggle = false;
		}
		if (GUI.Toggle(new Rect(Screen.width / 2, Screen.height / 10 - Screen.height / 15, toggleSize * 3, toggleSize), loggingToggle, new GUIContent("Logging")))
		{
			loggingToggle = true;
		}
		else
		{
			loggingToggle = false;
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10, buttonWidth, buttonHeight), "INITIALIZE", buttonStyle))
		{
			Init();
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + Screen.height / 10, buttonWidth, buttonHeight), interstitialLabel, buttonStyle))
		{
			showInterstitial();
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + 2 * Screen.height / 10, buttonWidth, buttonHeight), "SHOW REWARDED VIDEO", buttonStyle))
		{
			showRewardedVideo();
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + 3 * Screen.height / 10, buttonWidth, buttonHeight), "SHOW BANNER", buttonStyle))
		{
			showBanner();
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + 4 * Screen.height / 10, buttonWidth, buttonHeight), "HIDE BANNER", buttonStyle))
		{
			hideBanner();
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + 5 * Screen.height / 10, buttonWidth, buttonHeight), "SHOW BANNER VIEW", buttonStyle))
		{
			showBannerView();
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + 6 * Screen.height / 10, buttonWidth, buttonHeight), "HIDE BANNER VIEW", buttonStyle))
		{
			hideBannerView();
		}
		if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + 7 * Screen.height / 10, buttonWidth, buttonHeight), "SHOW TEST SCREEN", buttonStyle))
		{
			Appodeal.showTestScreen();
		}
	}

	private void InitStyles()
	{
		if (buttonStyle == null)
		{
			buttonWidth = Screen.width - Screen.width / 5;
			buttonHeight = Screen.height / 15;
			toggleSize = Screen.height / 20;
			buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.fontSize = buttonHeight / 2;
			buttonStyle.normal.textColor = Color.red;
			buttonStyle.hover.textColor = Color.red;
			buttonStyle.active.textColor = Color.red;
			buttonStyle.focused.textColor = Color.red;
			buttonStyle.active.background = MakeTexure(buttonWidth, buttonHeight, Color.grey);
			buttonStyle.focused.background = MakeTexure(buttonWidth, buttonHeight, Color.grey);
			buttonStyle.normal.background = MakeTexure(buttonWidth, buttonHeight, Color.white);
			buttonStyle.hover.background = MakeTexure(buttonWidth, buttonHeight, Color.white);
			GUI.skin.toggle = buttonStyle;
		}
	}

	private Texture2D MakeTexure(int width, int height, Color color)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	public void showInterstitial()
	{
		if (Appodeal.isLoaded(3) && !Appodeal.isPrecache(3))
		{
			Appodeal.show(3);
		}
		else
		{
			Appodeal.cache(3);
		}
	}

	public void showRewardedVideo()
	{
		Debug.Log("Reward currency: " + Appodeal.getRewardParameters().Key + ", amount: " + Appodeal.getRewardParameters().Value);
		if (Appodeal.canShow(128))
		{
			Appodeal.show(128);
		}
	}

	public void showBanner()
	{
		Appodeal.show(8, "banner_button_click");
	}

	public void showBannerView()
	{
		Appodeal.showBannerView(Screen.currentResolution.height - Screen.currentResolution.height / 10, -2, "banner_view");
	}

	public void hideBanner()
	{
		Appodeal.hide(4);
	}

	public void hideBannerView()
	{
		Appodeal.hideBannerView();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			Appodeal.onResume();
		}
	}

	public void onBannerLoaded(bool isPrecache)
	{
		Debug.Log("Banner loaded, isPrecache:" + isPrecache);
	}

	public void onBannerFailedToLoad()
	{
		Debug.Log("Banner failed");
	}

	public void onBannerShown()
	{
		Debug.Log("Banner opened");
	}

	public void onBannerClicked()
	{
		Debug.Log("banner clicked");
	}

	public void onInterstitialLoaded(bool isPrecache)
	{
		interstitialLabel = "SHOW INTERSTITIAL";
		Debug.Log("Interstitial loaded");
	}

	public void onInterstitialFailedToLoad()
	{
		Debug.Log("Interstitial failed to load");
	}

	public void onInterstitialShown()
	{
		interstitialLabel = "CACHE INTERSTITIAL";
		Debug.Log("Interstitial opened");
	}

	public void onInterstitialClicked()
	{
		Debug.Log("Interstitial clicked");
	}

	public void onInterstitialClosed()
	{
		Debug.Log("Interstitial closed");
	}

	public void onNonSkippableVideoLoaded()
	{
		Debug.Log("NonSkippable Video loaded");
	}

	public void onNonSkippableVideoFailedToLoad()
	{
		Debug.Log("NonSkippable Video failed to load");
	}

	public void onNonSkippableVideoShown()
	{
		Debug.Log("NonSkippable Video opened");
	}

	public void onNonSkippableVideoClosed(bool isFinished)
	{
		Debug.Log("NonSkippable Video, finished:" + isFinished);
	}

	public void onNonSkippableVideoFinished()
	{
		Debug.Log("NonSkippable Video finished");
	}

	public void onRewardedVideoLoaded()
	{
		Debug.Log("Rewarded Video loaded");
	}

	public void onRewardedVideoFailedToLoad()
	{
		Debug.Log("Rewarded Video failed to load");
	}

	public void onRewardedVideoShown()
	{
		Debug.Log("Rewarded Video opened");
	}

	public void onRewardedVideoClosed(bool isFinished)
	{
		Debug.Log("Rewarded Video closed, finished:" + isFinished);
	}

	public void onRewardedVideoFinished(int amount, string name)
	{
		Debug.Log("Rewarded Video Reward: " + amount + " " + name);
	}

	public void writeExternalStorageResponse(int result)
	{
		if (result == 0)
		{
			Debug.Log("WRITE_EXTERNAL_STORAGE permission granted");
		}
		else
		{
			Debug.Log("WRITE_EXTERNAL_STORAGE permission grant refused");
		}
	}

	public void accessCoarseLocationResponse(int result)
	{
		if (result == 0)
		{
			Debug.Log("ACCESS_COARSE_LOCATION permission granted");
		}
		else
		{
			Debug.Log("ACCESS_COARSE_LOCATION permission grant refused");
		}
	}
}
