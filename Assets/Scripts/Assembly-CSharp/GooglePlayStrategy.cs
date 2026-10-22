using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AppsMinistry.Domain;
using AppsMinistry.Utils;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TinyTeam.Debuger;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GooglePlayStrategy : MonoBehaviour, IGameService
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnPlayerAuthenticated__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnPlayerFailedToAuthenticate__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnLoadPlayerDataFailed__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GameServicePlayer>> OnPlayerDataLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GameServiceScore>> OnScoresForPlayerIdLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnScoresForPlayerIdFailed__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnProfilePhotoLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnProfilePhotoFailed__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnReportScoreFailed__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnReportScoreFinished__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnRetrieveScoresFailed__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GameServiceChallenge>> OnChallengesLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<GameServiceChallenge> OnPlayerDidSelectChallenge__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnGiftResultSuccess__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GPGameRequest>> OnGameRequestAccepted__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GPGameRequest>> OnPendingGiftsDetected__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnAchievmentUpdated__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnAchievmentUpdateFailed__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<GooglePlayConnectionResult> OnConnectionResultReceived__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnTokenLoaded__BackingField;

	public event Action OnPlayerAuthenticated
	{
		add
		{
			Action action = OnPlayerAuthenticated__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerAuthenticated__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnPlayerAuthenticated__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerAuthenticated__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnPlayerFailedToAuthenticate
	{
		add
		{
			Action<string> action = OnPlayerFailedToAuthenticate__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerFailedToAuthenticate__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnPlayerFailedToAuthenticate__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerFailedToAuthenticate__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnLoadPlayerDataFailed
	{
		add
		{
			Action<string> action = OnLoadPlayerDataFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLoadPlayerDataFailed__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnLoadPlayerDataFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnLoadPlayerDataFailed__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<List<GameServicePlayer>> OnPlayerDataLoaded
	{
		add
		{
			Action<List<GameServicePlayer>> action = OnPlayerDataLoaded__BackingField;
			Action<List<GameServicePlayer>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerDataLoaded__BackingField, (Action<List<GameServicePlayer>>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<List<GameServicePlayer>> action = OnPlayerDataLoaded__BackingField;
			Action<List<GameServicePlayer>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerDataLoaded__BackingField, (Action<List<GameServicePlayer>>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<List<GameServiceScore>> OnScoresForPlayerIdLoaded
	{
		add
		{
			Action<List<GameServiceScore>> action = OnScoresForPlayerIdLoaded__BackingField;
			Action<List<GameServiceScore>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnScoresForPlayerIdLoaded__BackingField, (Action<List<GameServiceScore>>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<List<GameServiceScore>> action = OnScoresForPlayerIdLoaded__BackingField;
			Action<List<GameServiceScore>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnScoresForPlayerIdLoaded__BackingField, (Action<List<GameServiceScore>>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnScoresForPlayerIdFailed
	{
		add
		{
			Action<string> action = OnScoresForPlayerIdFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnScoresForPlayerIdFailed__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnScoresForPlayerIdFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnScoresForPlayerIdFailed__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnProfilePhotoLoaded
	{
		add
		{
			Action<string> action = OnProfilePhotoLoaded__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnProfilePhotoLoaded__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnProfilePhotoLoaded__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnProfilePhotoLoaded__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnProfilePhotoFailed
	{
		add
		{
			Action<string> action = OnProfilePhotoFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnProfilePhotoFailed__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnProfilePhotoFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnProfilePhotoFailed__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnReportScoreFailed
	{
		add
		{
			Action<string> action = OnReportScoreFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnReportScoreFailed__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnReportScoreFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnReportScoreFailed__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnReportScoreFinished
	{
		add
		{
			Action<string> action = OnReportScoreFinished__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnReportScoreFinished__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnReportScoreFinished__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnReportScoreFinished__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnRetrieveScoresFailed
	{
		add
		{
			Action<string> action = OnRetrieveScoresFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnRetrieveScoresFailed__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnRetrieveScoresFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnRetrieveScoresFailed__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<List<GameServiceChallenge>> OnChallengesLoaded
	{
		add
		{
			Action<List<GameServiceChallenge>> action = OnChallengesLoaded__BackingField;
			Action<List<GameServiceChallenge>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChallengesLoaded__BackingField, (Action<List<GameServiceChallenge>>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<List<GameServiceChallenge>> action = OnChallengesLoaded__BackingField;
			Action<List<GameServiceChallenge>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChallengesLoaded__BackingField, (Action<List<GameServiceChallenge>>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<GameServiceChallenge> OnPlayerDidSelectChallenge
	{
		add
		{
			Action<GameServiceChallenge> action = OnPlayerDidSelectChallenge__BackingField;
			Action<GameServiceChallenge> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerDidSelectChallenge__BackingField, (Action<GameServiceChallenge>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<GameServiceChallenge> action = OnPlayerDidSelectChallenge__BackingField;
			Action<GameServiceChallenge> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerDidSelectChallenge__BackingField, (Action<GameServiceChallenge>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnGiftResultSuccess
	{
		add
		{
			Action action = OnGiftResultSuccess__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGiftResultSuccess__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnGiftResultSuccess__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGiftResultSuccess__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<List<GPGameRequest>> OnGameRequestAccepted
	{
		add
		{
			Action<List<GPGameRequest>> action = OnGameRequestAccepted__BackingField;
			Action<List<GPGameRequest>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGameRequestAccepted__BackingField, (Action<List<GPGameRequest>>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<List<GPGameRequest>> action = OnGameRequestAccepted__BackingField;
			Action<List<GPGameRequest>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGameRequestAccepted__BackingField, (Action<List<GPGameRequest>>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<List<GPGameRequest>> OnPendingGiftsDetected
	{
		add
		{
			Action<List<GPGameRequest>> action = OnPendingGiftsDetected__BackingField;
			Action<List<GPGameRequest>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPendingGiftsDetected__BackingField, (Action<List<GPGameRequest>>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<List<GPGameRequest>> action = OnPendingGiftsDetected__BackingField;
			Action<List<GPGameRequest>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPendingGiftsDetected__BackingField, (Action<List<GPGameRequest>>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnAchievmentUpdated
	{
		add
		{
			Action<string> action = OnAchievmentUpdated__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAchievmentUpdated__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnAchievmentUpdated__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAchievmentUpdated__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnAchievmentUpdateFailed
	{
		add
		{
			Action<string> action = OnAchievmentUpdateFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAchievmentUpdateFailed__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnAchievmentUpdateFailed__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnAchievmentUpdateFailed__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<GooglePlayConnectionResult> OnConnectionResultReceived
	{
		add
		{
			Action<GooglePlayConnectionResult> action = OnConnectionResultReceived__BackingField;
			Action<GooglePlayConnectionResult> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnConnectionResultReceived__BackingField, (Action<GooglePlayConnectionResult>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<GooglePlayConnectionResult> action = OnConnectionResultReceived__BackingField;
			Action<GooglePlayConnectionResult> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnConnectionResultReceived__BackingField, (Action<GooglePlayConnectionResult>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public event Action<string> OnTokenLoaded
	{
		add
		{
			Action<string> action = OnTokenLoaded__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnTokenLoaded__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnTokenLoaded__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnTokenLoaded__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private void OnEnable()
	{
		TinyTeam.Debuger.Console.RegisterCommand("getEmail", CMDGetEmail, "Return current account email");
		TinyTeam.Debuger.Console.RegisterCommand("getPlayerStats", CMDGetPlayerStats, "Request Player Stats");
	}

	public void AuthenticateLocalPlayer()
	{
		if (PlayGamesPlatform.Instance.IsAuthenticated())
		{
			return;
		}
		PlayGamesPlatform.Instance.Authenticate((bool state) =>
		{
			if (state)
			{
				UnityEngine.Debug.Log("GooglePlayStrategy::AuthenticateLocalPlayer authenticate result: " + state);
				if (OnPlayerAuthenticated__BackingField != null)
				{
					OnPlayerAuthenticated__BackingField();
				}
			}
			else
			{
				UnityEngine.Debug.LogError("GooglePlayStrategy::AuthenticateLocalPlayer authenticate result: " + state);
				if (OnPlayerFailedToAuthenticate__BackingField != null)
				{
					OnPlayerFailedToAuthenticate__BackingField("Error Occured. Something goes wrong");
				}
			}
		});
	}

	public bool IsPlayerAuthenticated()
	{
		return PlayGamesPlatform.Instance.IsAuthenticated();
	}

	public void ShowGameCenterViewController(GameServiceViewControllerState viewState, GameServiceLeaderboardTimeScope timeScope, string leaderboardId)
	{
		if (viewState == GameServiceViewControllerState.Achievements)
		{
			PlayGamesPlatform.Instance.ShowAchievementsUI();
		}
		else
		{
			PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
		}
	}

	public void ShowGameGifts()
	{
	}

	public void ConfigureChallengeBanners(bool showBannerForLocallyCompletedChallenge, bool showBannerForLocallyReceivedChallenge, bool showBannerForRemotelyCompletedChallenge)
	{
	}

	public void LoadChallengesList()
	{
	}

	public void ReportScore(long score, string leaderboardId)
	{
		PlayGamesPlatform.Instance.ReportScore(score, leaderboardId, (bool state) =>
		{
			if (state)
			{
				if (OnReportScoreFinished__BackingField != null)
				{
					OnReportScoreFinished__BackingField(string.Format("Success Score {0} report to {1}", score, leaderboardId));
				}
			}
			else if (OnReportScoreFailed__BackingField != null)
			{
				OnReportScoreFailed__BackingField(string.Format("Error Score {0} report to {1}", score, leaderboardId));
			}
		});
	}

	public void RetrieveFriends()
	{
	}

	public void SendGiftRequest(GPGameRequestType type, int requestLifetimeDays, Texture2D icon, string description, string playload = "")
	{
		if (icon == null)
		{
			icon = new Texture2D(32, 32, TextureFormat.ARGB32, false);
		}
	}

	public void LoadPlayerData(string[] playerIdArray, bool loadProfileImages)
	{
		PlayGamesPlatform.Instance.LoadUsers(playerIdArray, (IUserProfile[] playersData) =>
		{
			foreach (IUserProfile userProfile in playersData)
			{
				List<GameServicePlayer> list = new List<GameServicePlayer>();
				GameServicePlayer gameServicePlayer = new GameServicePlayer();
				gameServicePlayer.Alias = userProfile.userName;
				gameServicePlayer.DisplayName = userProfile.userName;
				gameServicePlayer.PlayerId = userProfile.id;
				gameServicePlayer.ProfilePhotoPath = Path.Combine(Application.persistentDataPath, string.Format("{0}.png", gameServicePlayer.PlayerId).Replace(":", string.Empty));
				if (userProfile.image != null)
				{
					gameServicePlayer.SavePhoto(userProfile.image);
				}
				gameServicePlayer.LoadPhoto();
				list.Add(gameServicePlayer);
				if (OnPlayerDataLoaded__BackingField != null)
				{
					OnPlayerDataLoaded__BackingField(list);
				}
			}
		});
	}

	public void RetrieveScoresForPlayerId(string playerId, string leaderboardId)
	{
		PlayGamesPlatform.Instance.LoadScores(leaderboardId, LeaderboardStart.TopScores, 5, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (LeaderboardScoreData scoreData) =>
		{
			if (scoreData != null)
			{
				IScore[] scores = scoreData.Scores;
				List<GameServiceScore> list = new List<GameServiceScore>();
				for (int i = 0; i < scores.Length; i++)
				{
					list.Add(new GameServiceScore(scores[i]));
				}
				if (OnScoresForPlayerIdLoaded__BackingField != null)
				{
					OnScoresForPlayerIdLoaded__BackingField(list);
				}
			}
			else if (OnScoresForPlayerIdFailed__BackingField != null)
			{
				OnScoresForPlayerIdFailed__BackingField("Error. While Getting scores");
			}
		});
	}

	public string PlayerIdentifier()
	{
		return PlayGamesPlatform.Instance.GetUserId();
	}

	public string PlayerAlias()
	{
		return PlayGamesPlatform.Instance.GetUserDisplayName();
	}

	public void ReportAchievment(string achievementId)
	{
		PlayGamesPlatform.Instance.ReportProgress(achievementId, 100.0, (bool state) =>
		{
			if (state)
			{
				if (OnAchievmentUpdated__BackingField != null)
				{
					OnAchievmentUpdated__BackingField(achievementId);
				}
			}
			else if (OnAchievmentUpdateFailed__BackingField != null)
			{
				OnAchievmentUpdateFailed__BackingField(achievementId);
			}
		});
	}

	public void LoadProfilePhotoForLocalPlayer()
	{
		string userImageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
		string userId = PlayGamesPlatform.Instance.GetUserId();
		UnityEngine.Debug.Log(string.Format("GooglePlayStrategy::LoadProfilePhotoForLocalPlayer for userId {0} from {1}", userId, userImageUrl));
		StartCoroutine(DownloadAndSave(userImageUrl, userId));
	}

	private object CMDGetEmail(string[] args)
	{
		string empty = string.Empty;
		empty = (PlayGamesPlatform.Instance.IsAuthenticated() ? PlayGamesPlatform.Instance.GetUserEmail() : "Error. Need Authenticated before!");
		if (string.IsNullOrEmpty(empty))
		{
			empty = "Error. eMail Not Ready Yet";
		}
		return empty;
	}

	private object CMDGetPlayerStats(string[] args)
	{
		UnityEngine.Debug.Log("GooglePlayStrategy::CMDGetPlayerStats");
		PlayGamesPlatform.Instance.GetPlayerStats((CommonStatusCodes statusCode, PlayerStats playerStats) =>
		{
			string text = string.Format("PlayerStats statusCode {0}", statusCode);
			if (statusCode == CommonStatusCodes.Success)
			{
				text += string.Format("Valid {0}\n", playerStats.Valid);
				text = ((!playerStats.HasNumberOfPurchases()) ? (text + string.Format("NumberOfPurchases {0}\n", "***no info***")) : (text + string.Format("NumberOfPurchases {0}\n", playerStats.NumberOfPurchases)));
				text = ((!playerStats.HasNumberOfSessions()) ? (text + string.Format("NumberOfSessions {0}\n", "***no info***")) : (text + string.Format("NumberOfSessions {0}\n", playerStats.NumberOfSessions)));
				text = ((!playerStats.HasAvgSessonLength()) ? (text + string.Format("AvgSessonLength {0}\n", "***no info***")) : (text + string.Format("AvgSessonLength {0}\n", playerStats.AvgSessonLength)));
				text = ((!playerStats.HasDaysSinceLastPlayed()) ? (text + string.Format("DaysSinceLastPlayed {0}\n", "***no info***")) : (text + string.Format("DaysSinceLastPlayed {0}\n", playerStats.DaysSinceLastPlayed)));
				text = ((!playerStats.HasSessPercentile()) ? (text + string.Format("SessPercentile {0}\n", "***no info***")) : (text + string.Format("SessPercentile {0}\n", playerStats.SessPercentile)));
				if (playerStats.HasChurnProbability())
				{
					text += string.Format("ChurnProbability {0}\n", playerStats.ChurnProbability);
				}
				else
				{
					text += string.Format("ChurnProbability {0}\n", "***no info***");
				}
			}
		});
		return "Request Send";
	}

	private IEnumerator DownloadAndSave(string url, string playerId)
	{
		string imageFileName = string.Format("{0}.png", playerId).Replace(":", string.Empty);
		string folderToSave = Application.persistentDataPath;
		if (!IOSystem.IsFileExist(folderToSave, imageFileName))
		{
			WWW www = new WWW(url);
			yield return www;
			if (string.IsNullOrEmpty(www.error) && !IOSystem.IsFileExist(folderToSave, imageFileName))
			{
				byte[] data = www.texture.EncodeToPNG();
				IOSystem.SaveBytes(Application.persistentDataPath, imageFileName, ref data);
			}
		}
	}
}
