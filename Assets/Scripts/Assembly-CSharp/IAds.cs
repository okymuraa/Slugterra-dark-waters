using System;

public interface IAds
{
	Action RewardSuccessEvent { get; set; }

	Action RewardFailEvent { get; set; }

	bool CanShowAd { set; }

	bool IncentivizeAdsAvailable { get; }

	bool VideoAdsAvailable { get; }

	bool InterstitialAdsAvailable { get; }

	void Initialize();

	bool ShowIncentivizedAds();

	void ShowAds(string screenName);
}
