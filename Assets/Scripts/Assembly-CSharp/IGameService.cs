using System;
using System.Collections.Generic;
using AppsMinistry.Domain;
using UnityEngine;

public interface IGameService
{
	event Action<string> OnProfilePhotoLoaded;

	event Action<string> OnProfilePhotoFailed;

	event Action OnPlayerAuthenticated;

	event Action<string> OnPlayerFailedToAuthenticate;

	event Action<string> OnTokenLoaded;

	event Action<string> OnReportScoreFailed;

	event Action<string> OnReportScoreFinished;

	event Action<string> OnRetrieveScoresFailed;

	event Action<List<GameServicePlayer>> OnPlayerDataLoaded;

	event Action<string> OnLoadPlayerDataFailed;

	event Action<List<GameServiceScore>> OnScoresForPlayerIdLoaded;

	event Action<string> OnScoresForPlayerIdFailed;

	event Action<List<GameServiceChallenge>> OnChallengesLoaded;

	event Action<GameServiceChallenge> OnPlayerDidSelectChallenge;

	event Action OnGiftResultSuccess;

	event Action<List<GPGameRequest>> OnGameRequestAccepted;

	void AuthenticateLocalPlayer();

	bool IsPlayerAuthenticated();

	string PlayerIdentifier();

	string PlayerAlias();

	void LoadProfilePhotoForLocalPlayer();

	void ConfigureChallengeBanners(bool showBannerForLocallyCompletedChallenge, bool showBannerForLocallyReceivedChallenge, bool showBannerForRemotelyCompletedChallenge);

	void LoadChallengesList();

	void ShowGameCenterViewController(GameServiceViewControllerState viewState, GameServiceLeaderboardTimeScope timeScope, string leaderboardId);

	void ReportScore(long score, string leaderboardId);

	void LoadPlayerData(string[] playerIdArray, bool loadProfileImages);

	void RetrieveFriends();

	void RetrieveScoresForPlayerId(string playerId, string leaderboardId);

	void ReportAchievment(string id);

	void SendGiftRequest(GPGameRequestType type, int requestLifetimeDays, Texture2D icon, string description, string playload = "");

	void ShowGameGifts();
}
