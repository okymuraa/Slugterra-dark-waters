using System;
using ColorTvPlugin;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemoSceneController : MonoBehaviour
{
	private const string DISCOVERY_CENTER_PLACEMENT = "Pause";

	private const string APP_FEATURE_PLACEMENT = "InAppPurchase";

	private const string DIRECT_ENGAGEMENT_PLACEMENT = "StageOpen";

	private const string VIDEO_PLACEMENT = "BetweenLevels";

	private const string RANDOM_AD_PLACEMENT = "MainMenu";

	public string AndroidAppId = string.Empty;

	public string AppleTVAppId = "9778cfab-2c1d-47bf-929d-a1545f37534f";

	private string ChosenAppId = string.Empty;

	private void Start()
	{
		ChosenAppId = AndroidAppId;
		StartSDK();
		EventSystem.current.SetSelectedGameObject(GameObject.Find("ShowDiscoveryCenterButton"));
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void StartSDK()
	{
		MonoBehaviour.print("Starting SDK");
		ColorTv.SetDebugMode(true);
		ColorTv.Init(ChosenAppId);
		ColorTvCallbacks.CurrencyEarned = (Action<ColorTvCurrency>)Delegate.Combine(ColorTvCallbacks.CurrencyEarned, new Action<ColorTvCurrency>(OnCurrencyEarned));
		ColorTvCallbacks.AdLoaded = (Action<string>)Delegate.Combine(ColorTvCallbacks.AdLoaded, new Action<string>(OnAdLoaded));
		ColorTvCallbacks.AdClosed = (Action<string>)Delegate.Combine(ColorTvCallbacks.AdClosed, new Action<string>(OnAdClosed));
		ColorTvCallbacks.AdError = (Action<ColorTvError>)Delegate.Combine(ColorTvCallbacks.AdError, new Action<ColorTvError>(OnError));
		ColorTvCallbacks.AdExpired = (Action<string>)Delegate.Combine(ColorTvCallbacks.AdExpired, new Action<string>(OnAdExpired));
	}

	public void OnShowDiscoveryCenter()
	{
		MonoBehaviour.print("Show Discovery Center button pressed");
		ColorTv.LoadAd("Pause");
	}

	public void OnShowAppFeature()
	{
		MonoBehaviour.print("Show App Feature button pressed");
		ColorTv.LoadAd("InAppPurchase");
	}

	public void OnShowDirectEngagement()
	{
		MonoBehaviour.print("Show Direct Engagement button pressed");
		ColorTv.LoadAd("StageOpen");
	}

	public void OnShowVideo()
	{
		MonoBehaviour.print("Show Video button pressed");
		ColorTv.LoadAd("BetweenLevels");
	}

	public void OnShowRandomAd()
	{
		MonoBehaviour.print("Show Random Ad button pressed");
		ColorTv.LoadAd("MainMenu");
	}

	public void OnAdLoaded(string placementId)
	{
		Debug.Log("Ad is available for placement: " + placementId);
		ColorTv.ShowAd(placementId);
	}

	public void OnAdClosed(string placementId)
	{
		Debug.Log("Ad has been closed for placement: " + placementId);
	}

	public void OnAdExpired(string placementId)
	{
		Debug.Log("Ad has expired for placement: " + placementId);
	}

	public void OnError(ColorTvError error)
	{
		Debug.Log(string.Concat("Ad error occured for placement: ", error.PlacementId, ", with error code: ", error.ErrorCode, " and error message: ", error.ErrorMessage));
	}

	public void OnCurrencyEarned(ColorTvCurrency coins)
	{
		Debug.Log("User has been awarded for placement " + coins.Placement + ": " + coins.Amount + " x " + coins.Type);
	}
}
