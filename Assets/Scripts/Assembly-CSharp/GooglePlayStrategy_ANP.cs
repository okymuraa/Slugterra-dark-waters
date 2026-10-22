using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AppsMinistry.Domain;
using AppsMinistry.Utils;
using SA.Common.Pattern;
using UnityEngine;

public class GooglePlayStrategy_ANP : MonoBehaviour, IGameService
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
		GooglePlayConnection.ActionPlayerConnected += OnPlayerConnected;
		GooglePlayConnection.ActionPlayerDisconnected += OnPlayerDisconnected;
		GooglePlayConnection.ActionConnectionResultReceived += OnConnectionResult;
		GooglePlayConnection.ActionConnectionStateChanged += OnConnectionStateChanged;
		GooglePlayManager.ActionAchievementUpdated += OnAchivementUpdated;
		GooglePlayManager.ActionAchievementsLoaded += OnAchievementLoaded;
		GooglePlayManager.ActionScoreSubmited += OnScoreSubmited;
		GooglePlayManager.ActionScoresListLoaded += OnScoreRequestReceived;
		GooglePlayManager.ActionSendGiftResultReceived += OnGiftResult;
		GooglePlayManager.ActionGameRequestsAccepted += OnGameRequestAcceptedEvent;
		GooglePlayManager.ActionPendingGameRequestsDetected += OnPendingGiftsDetectedEvent;
		GooglePlayManager.ActionAvailableDeviceAccountsLoaded += OnAccsLoaded;
		GooglePlayManager.ActionOAuthTokenLoaded += OnTokenLoadedEvent;
		GooglePlayManager.ActionFriendsListLoaded += OnFriendListLoaded;
		GooglePlayManager.ActionLeaderboardsLoaded += OnLoadLeaderboardsEvent;
	}

	private void OnDisable()
	{
		if (!Singleton<GooglePlayConnection>.IsDestroyed)
		{
			GooglePlayConnection.ActionPlayerConnected -= OnPlayerConnected;
			GooglePlayConnection.ActionPlayerDisconnected -= OnPlayerDisconnected;
			GooglePlayConnection.ActionConnectionResultReceived -= OnConnectionResult;
			GooglePlayConnection.ActionConnectionStateChanged -= OnConnectionStateChanged;
		}
		if (!Singleton<GooglePlayManager>.IsDestroyed)
		{
			GooglePlayManager.ActionAchievementUpdated -= OnAchivementUpdated;
			GooglePlayManager.ActionAchievementsLoaded -= OnAchievementLoaded;
			GooglePlayManager.ActionScoreSubmited -= OnScoreSubmited;
			GooglePlayManager.ActionScoresListLoaded -= OnScoreRequestReceived;
			GooglePlayManager.ActionSendGiftResultReceived -= OnGiftResult;
			GooglePlayManager.ActionPendingGameRequestsDetected -= OnPendingGiftsDetectedEvent;
			GooglePlayManager.ActionGameRequestsAccepted -= OnGameRequestAcceptedEvent;
			GooglePlayManager.ActionAvailableDeviceAccountsLoaded -= OnAccsLoaded;
			GooglePlayManager.ActionOAuthTokenLoaded -= OnTokenLoadedEvent;
			GooglePlayManager.ActionFriendsListLoaded -= OnFriendListLoaded;
			GooglePlayManager.ActionLeaderboardsLoaded -= OnLoadLeaderboardsEvent;
		}
	}

	public void AuthenticateLocalPlayer()
	{
		if (IsPlayerAuthenticated())
		{
			Singleton<GooglePlayConnection>.Instance.Disconnect();
		}
		if (!IsPlayerAuthenticated())
		{
			Singleton<GooglePlayConnection>.Instance.Connect();
		}
	}

	public bool IsPlayerAuthenticated()
	{
		return GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED;
	}

	public void ShowGameCenterViewController(GameServiceViewControllerState viewState, GameServiceLeaderboardTimeScope timeScope, string leaderboardId)
	{
		if (viewState == GameServiceViewControllerState.Achievements)
		{
			Singleton<GooglePlayManager>.Instance.ShowAchievementsUI();
		}
		else
		{
			Singleton<GooglePlayManager>.Instance.ShowLeaderBoardById(leaderboardId);
		}
	}

	public void ConfigureChallengeBanners(bool showBannerForLocallyCompletedChallenge, bool showBannerForLocallyReceivedChallenge, bool showBannerForRemotelyCompletedChallenge)
	{
	}

	public void LoadChallengesList()
	{
	}

	public void ReportScore(long score, string leaderboardId)
	{
		Singleton<GooglePlayManager>.Instance.SubmitScoreById(leaderboardId, score);
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
		Singleton<GooglePlayManager>.Instance.SendGiftRequest(type, requestLifetimeDays, icon, description, playload);
	}

	public void LoadPlayerData(string[] playerIdArray, bool loadProfileImages)
	{
		for (int i = 0; i < playerIdArray.Length; i++)
		{
			if (Singleton<GooglePlayManager>.Instance.players.ContainsKey(playerIdArray[i]))
			{
				GooglePlayerTemplate player = Singleton<GooglePlayManager>.Instance.players[playerIdArray[i]];
				OnLoadPlayerCompletedEvent(player, playerIdArray[i]);
			}
		}
	}

	public void RetrieveScoresForPlayerId(string playerId, string leaderboardId)
	{
		Singleton<GooglePlayManager>.Instance.LoadLeaderBoards();
	}

	public string PlayerIdentifier()
	{
		if (Singleton<GooglePlayManager>.Instance.player != null)
		{
			return Singleton<GooglePlayManager>.Instance.player.playerId;
		}
		return string.Empty;
	}

	public string PlayerAlias()
	{
		if (Singleton<GooglePlayManager>.Instance.player != null)
		{
			return Singleton<GooglePlayManager>.Instance.player.name;
		}
		return string.Empty;
	}

	public void ReportAchievment(string id)
	{
		Singleton<GooglePlayManager>.Instance.UnlockAchievementById(id);
	}

	public void LoadProfilePhotoForLocalPlayer()
	{
		OnProfilePhotoLoadedEvent(Path.Combine(Application.persistentDataPath, string.Format("{0}.png", Singleton<GooglePlayManager>.Instance.player.playerId).Replace(":", string.Empty)));
	}

	private void OnPlayerAuthenticatedEvent(string result)
	{
		UnityEngine.Debug.Log("GooglePlayStrategy::OnPlayerAuthenticatedEvent " + result);
		if (OnPlayerAuthenticated__BackingField != null)
		{
			OnPlayerAuthenticated__BackingField();
		}
	}

	private void OnPlayerFailedToAuthenticateEvent(string result)
	{
		UnityEngine.Debug.LogWarning("GooglePlayStrategy::OnPlayerFailedToAuthenticateEvent " + result);
		if (OnPlayerFailedToAuthenticate__BackingField != null)
		{
			OnPlayerFailedToAuthenticate__BackingField(result);
		}
	}

	private void OnReportScoreFinishedEvent(string key, Dictionary<string, object> result)
	{
		if (OnReportScoreFinished__BackingField != null)
		{
			OnReportScoreFinished__BackingField(key);
		}
	}

	private void OnReportScoreFailedEvent(string key, string errorMsg)
	{
		if (OnReportScoreFailed__BackingField != null)
		{
			OnReportScoreFailed__BackingField(key + " " + errorMsg);
		}
	}

	private void OnAchievmentUpdatedEvent(bool succes, string id)
	{
		if (succes && OnAchievmentUpdated__BackingField != null)
		{
			OnAchievmentUpdated__BackingField(id);
		}
		if (!succes && OnAchievmentUpdateFailed__BackingField != null)
		{
			OnAchievmentUpdateFailed__BackingField(id);
		}
	}

	private void OnLoadLeaderboardsEvent(GooglePlayResult result)
	{
		if (result.isSuccess && OnScoresForPlayerIdLoaded__BackingField != null)
		{
			OnScoresForPlayerIdLoaded__BackingField(null);
		}
	}

	private void OnLoadScoresSucceededEvent(List<GPScore> scores)
	{
		List<GameServiceScore> list = new List<GameServiceScore>();
		for (int i = 0; i < scores.Count; i++)
		{
			list.Add(new GameServiceScore(scores[i]));
		}
		if (OnScoresForPlayerIdLoaded__BackingField != null)
		{
			OnScoresForPlayerIdLoaded__BackingField(list);
		}
	}

	private void OnLoadScoresFailedEvent(string key, string msgError)
	{
		if (OnScoresForPlayerIdFailed__BackingField != null)
		{
			OnScoresForPlayerIdFailed__BackingField(msgError);
		}
	}

	private void OnLoadPlayerCompletedEvent(GooglePlayerTemplate player, string key)
	{
		List<GameServicePlayer> list = new List<GameServicePlayer>();
		GameServicePlayer gameServicePlayer = new GameServicePlayer();
		gameServicePlayer.Alias = player.name;
		gameServicePlayer.DisplayName = player.name;
		gameServicePlayer.PlayerId = player.playerId;
		gameServicePlayer.ProfilePhotoPath = Path.Combine(Application.persistentDataPath, string.Format("{0}.png", gameServicePlayer.PlayerId).Replace(":", string.Empty));
		if (player.hiResImageUrl != null)
		{
			StartCoroutine(DownloadAndSave(player.hiResImageUrl, player.playerId));
		}
		gameServicePlayer.LoadPhoto();
		list.Add(gameServicePlayer);
		if (OnPlayerDataLoaded__BackingField != null)
		{
			OnPlayerDataLoaded__BackingField(list);
		}
	}

	private void OnProfilePhotoLoadedEvent(string pathToImage)
	{
		if (OnProfilePhotoLoaded__BackingField != null)
		{
			OnProfilePhotoLoaded__BackingField(pathToImage);
		}
	}

	private void OnAchivementUpdated(GP_AchievementResult result)
	{
		if (result.isSuccess)
		{
			OnAchievmentUpdatedEvent(true, result.achievementId);
		}
		else
		{
			OnAchievmentUpdatedEvent(false, result.achievementId);
		}
	}

	private void OnAchievementLoaded(GooglePlayResult result)
	{
	}

	private void OnScoreSubmited(GP_LeaderboardResult result)
	{
		if (result.isSuccess)
		{
			OnReportScoreFinishedEvent(result.message, null);
		}
		else
		{
			OnReportScoreFailedEvent(result.response.ToString(), result.message);
		}
	}

	private void OnScoreRequestReceived(GP_LeaderboardResult result)
	{
	}

	private void OnPlayerDisconnected()
	{
	}

	private void OnPlayerConnected()
	{
	}

	private void OnConnectionResult(GooglePlayConnectionResult result)
	{
		if (result.IsSuccess)
		{
			OnPlayerAuthenticatedEvent(result.code.ToString());
			Singleton<GooglePlayManager>.Instance.LoadToken();
		}
		else
		{
			OnPlayerFailedToAuthenticateEvent(result.code.ToString());
		}
		if (OnConnectionResultReceived__BackingField != null)
		{
			OnConnectionResultReceived__BackingField(result);
		}
	}

	private void OnConnectionStateChanged(GPConnectionState connectionState)
	{
	}

	private void OnGiftResult(GooglePlayGiftRequestResult result)
	{
		if (result.isSuccess && OnGiftResultSuccess__BackingField != null)
		{
			OnGiftResultSuccess__BackingField();
		}
	}

	private void OnFriendListLoaded(GooglePlayResult result)
	{
		if (!result.IsSucceeded)
		{
		}
	}

	private void OnPendingGiftsDetectedEvent(List<GPGameRequest> requests)
	{
		if (OnPendingGiftsDetected__BackingField != null)
		{
			OnPendingGiftsDetected__BackingField(requests);
		}
	}

	public void ShowGameGifts()
	{
		Singleton<GooglePlayManager>.Instance.ShowRequestsAccepDialog();
	}

	private void OnGameRequestAcceptedEvent(List<GPGameRequest> gifts)
	{
		if (OnGameRequestAccepted__BackingField != null)
		{
			OnGameRequestAccepted__BackingField(gifts);
		}
	}

	private void OnAccsLoaded(List<string> accounts)
	{
		string text = "Device contains following google accounts:\n";
		foreach (string deviceGoogleAccount in Singleton<GooglePlayManager>.Instance.deviceGoogleAccountList)
		{
			text = text + deviceGoogleAccount + "\n";
		}
	}

	private void OnTokenLoadedEvent(string token)
	{
		UnityEngine.Debug.Log("GooglePlayStrategy_ANP::OnTokenLoadedEvent g+ token: " + Singleton<GooglePlayManager>.Instance.loadedAuthToken);
		if (OnTokenLoaded__BackingField != null)
		{
			OnTokenLoaded__BackingField(token);
		}
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
