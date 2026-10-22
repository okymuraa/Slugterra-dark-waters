using UnityEngine;

public class AppsMinistrySDK : MonoBehaviour
{
	private int showCounter;

	private bool _showRichMedia = true;

	private void OnLevelWasLoaded(int leveNumber)
	{
		if (PlayerPrefs.HasKey("LaunchCount"))
		{
			showCounter = PlayerPrefs.GetInt("LaunchCount");
		}
		if (Application.loadedLevelName == "MainScene" && showCounter > 1)
		{
			AppsMinistrySdkBinding.Instance.AMRichMediaShow(string.Empty);
		}
	}

	public void UIApplicationDidBecomeActiveNotification()
	{
		Debug.Log("AppsMinistrySDK::UIApplicationDidBecomeActiveNotification " + _showRichMedia);
		if (_showRichMedia)
		{
			if (Application.loadedLevelName == "MainScene")
			{
				AppsMinistrySdkBinding.Instance.AMRichMediaShow(string.Empty);
			}
			_showRichMedia = false;
		}
	}

	public void UIApplicationDidEnterBackgroundNotification()
	{
		Debug.Log("AppsMinistrySDK::UIApplicationDidEnterBackgroundNotification " + _showRichMedia);
		_showRichMedia = true;
	}

	public void didChangeNetworkState(string networkStatus)
	{
		Debug.LogWarning("AppsMinistrySDK::didChangeNetworkState " + networkStatus);
	}
}
