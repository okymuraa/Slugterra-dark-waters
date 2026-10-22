using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Utils;
using Slugterra.App;
using UnityEngine;

[AppSingleton]
public class GameServiceContext : DonDestroyMonoSingleton<GameServiceContext>
{
	private IGameService _strategy;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GameServicePlayer>> OnPlayerDataLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnProfilePhotoLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnPlayerAuthenticated__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnPlayerFailedAuthenticated__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GameServiceScore>> OnScoresForPlayerIdLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GameServiceChallenge>> OnChallengesLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<GameServiceChallenge> OnPlayerDidSelectChallenge__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnGiftRequestSentSuccess__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnTokenLoaded__BackingField;

	private GameServiceChallenge selectedChallenge;

	public const string welcomeToSlugterraAchievement = "CgkIjZj4j6kZEAIQAQ";

	public const string wayOfSlugslingerAchievement = "CgkIjZj4j6kZEAIQAg";

	public const string newSheriffInTownAchievement = "CgkIjZj4j6kZEAIQAw";

	public const string bullEyeAchievement = "CgkIjZj4j6kZEAIQBA";

	public const string pumpBusterAchievement = "CgkIjZj4j6kZEAIQBQ";

	public const string chainsBreaker = "CgkIjZj4j6kZEAIQGA";

	public const string twistDefeated = "CgkIjZj4j6kZEAIQHA";

	public const string quietLawnCh1 = "CgkIjZj4j6kZEAIQCw";

	public const string quietLawnCh2 = "CgkIjZj4j6kZEAIQDA";

	public const string quietLawnCh3 = "CgkIjZj4j6kZEAIQDQ";

	public const string quietLawnCh4 = "CgkIjZj4j6kZEAIQDg";

	public const string quietLawnCh5 = "CgkIjZj4j6kZEAIQDw";

	public const string darkPeripharyCh1 = "CgkIjZj4j6kZEAIQEA";

	public const string darkPeripharyCh2 = "CgkIjZj4j6kZEAIQEQ";

	public const string darkPeripharyCh3 = "CgkIjZj4j6kZEAIQEg";

	public const string deadWeedCh1 = "CgkIjZj4j6kZEAIQEw";

	public const string deadWeedCh2 = "CgkIjZj4j6kZEAIQFQ";

	public const string deadWeedCh3 = "CgkIjZj4j6kZEAIQFA";

	public const string ghoulDeepCh1 = "CgkIjZj4j6kZEAIQFg";

	public const string ghoulDeepCh2 = "CgkIjZj4j6kZEAIQFw";

	public const string trainCh1 = "CgkIjZj4j6kZEAIQGQ";

	public const string trainCh2 = "CgkIjZj4j6kZEAIQGg";

	public const string trainCh3 = "CgkIjZj4j6kZEAIQGw";

	public const string muliplayerLeaderBoard = "CgkIjZj4j6kZEAIQHQ";

	private Texture2D _coinsLogoForGift;

	public static string FaceBookPlayerID = string.Empty;

	private List<GameServiceChallenge> _gameCenterChallenges = new List<GameServiceChallenge>();

	public int challengeCount;

	public GameServiceChallenge SelectedChallenge
	{
		get
		{
			return selectedChallenge;
		}
		set
		{
			selectedChallenge = value;
		}
	}

	public List<GameServiceChallenge> GameServiceChallenges
	{
		get
		{
			return _gameCenterChallenges;
		}
	}

	public string PlayerAlias
	{
		get
		{
			return _strategy.PlayerAlias();
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

	public event Action<string> OnPlayerFailedAuthenticated
	{
		add
		{
			Action<string> action = OnPlayerFailedAuthenticated__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerFailedAuthenticated__BackingField, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<string> action = OnPlayerFailedAuthenticated__BackingField;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnPlayerFailedAuthenticated__BackingField, (Action<string>)Delegate.Remove(action2, value), action);
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

	public event Action OnGiftRequestSentSuccess
	{
		add
		{
			Action action = OnGiftRequestSentSuccess__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGiftRequestSentSuccess__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnGiftRequestSentSuccess__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnGiftRequestSentSuccess__BackingField, (Action)Delegate.Remove(action2, value), action);
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

	protected override void OnCreate()
	{
		_strategy = gameObject.AddComponent<GooglePlayStrategy>();
		_strategy.OnTokenLoaded += OnTokenLoadedEvent;
		_strategy.OnPlayerDataLoaded += OnPlayerDataLoadedEvent;
		_strategy.OnProfilePhotoLoaded += OnProfilePhotoLoadedEvent;
		_strategy.OnPlayerAuthenticated += OnPlayerAuthenticatedEvent;
		_strategy.OnPlayerFailedToAuthenticate += OnPlayerFailedAuthenticatedEvent;
		_strategy.OnChallengesLoaded += OnChallengesLoadedEvent;
		_strategy.OnPlayerDidSelectChallenge += OnPlayerDidSelectChallengeEvent;
		_strategy.OnReportScoreFailed += OnReportScoreFailedEvent;
		_strategy.OnReportScoreFinished += OnReportScoreFinishedEvent;
		_strategy.OnRetrieveScoresFailed += OnRetrieveScoresFailedEvent;
		_strategy.OnScoresForPlayerIdLoaded += OnScoresForPlayerIdLoadedEvent;
		_strategy.OnGiftResultSuccess += OnGiftResultSuccessEvent;
		_strategy.OnGameRequestAccepted += OnGameRequestAcceptedEvent;
		Invoke("AuthenticateLocalPlayer", 1.1f);
	}

	private void OnDisable()
	{
		_strategy.OnTokenLoaded -= OnTokenLoadedEvent;
		_strategy.OnPlayerDataLoaded -= OnPlayerDataLoadedEvent;
		_strategy.OnProfilePhotoLoaded -= OnProfilePhotoLoadedEvent;
		_strategy.OnPlayerAuthenticated -= OnPlayerAuthenticatedEvent;
		_strategy.OnChallengesLoaded -= OnChallengesLoadedEvent;
		_strategy.OnPlayerDidSelectChallenge -= OnPlayerDidSelectChallengeEvent;
		_strategy.OnReportScoreFailed -= OnReportScoreFailedEvent;
		_strategy.OnReportScoreFinished -= OnReportScoreFinishedEvent;
		_strategy.OnRetrieveScoresFailed -= OnRetrieveScoresFailedEvent;
		_strategy.OnScoresForPlayerIdLoaded -= OnScoresForPlayerIdLoadedEvent;
		_strategy.OnGiftResultSuccess -= OnGiftResultSuccessEvent;
		_strategy.OnGameRequestAccepted -= OnGameRequestAcceptedEvent;
	}

	private void OnPlayerAuthenticatedEvent()
	{
		if (OnPlayerAuthenticated__BackingField != null)
		{
			OnPlayerAuthenticated__BackingField();
		}
		LoadChallengesList();
		ConfigureChallengeBanners(true, true, true);
	}

	private void OnPlayerFailedAuthenticatedEvent(string result)
	{
		if (OnPlayerFailedAuthenticated__BackingField != null)
		{
			OnPlayerFailedAuthenticated__BackingField(result);
		}
	}

	private void OnPlayerDataLoadedEvent(List<GameServicePlayer> playerData)
	{
		if (OnPlayerDataLoaded__BackingField != null)
		{
			OnPlayerDataLoaded__BackingField(playerData);
		}
	}

	private void OnProfilePhotoLoadedEvent(string pathToPhoto)
	{
		if (OnProfilePhotoLoaded__BackingField != null)
		{
			OnProfilePhotoLoaded__BackingField(pathToPhoto);
		}
	}

	public void AuthenticateLocalPlayer()
	{
		DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
		{
			UnityEngine.Debug.Log("GameServiceContext::AuthenticateLocalPlayer Success");
			_strategy.AuthenticateLocalPlayer();
		}, (string errorMsg) =>
		{
			UnityEngine.Debug.Log("GameServiceContext::AuthenticateLocalPlayer Fail");
		});
	}

	public void RetrieveScoresForPlayerId(string playerID, string leaderboardID)
	{
		UnityEngine.Debug.LogFormat("GameServiceContext::RetrieveScoresForPlayerId Player {0} LeaderBoard {1}", playerID, leaderboardID);
		_strategy.RetrieveScoresForPlayerId(playerID, leaderboardID);
	}

	public bool IsPlayerAuthenticated()
	{
		return _strategy.IsPlayerAuthenticated();
	}

	public string PlayerIdentifier()
	{
		return _strategy.PlayerIdentifier();
	}

	public void ReportAchievment(string id)
	{
		_strategy.ReportAchievment(id);
	}

	public void ConfigureChallengeBanners(bool showBannerForLocallyCompletedChallenge, bool showBannerForLocallyReceivedChallenge, bool showBannerForRemotelyCompletedChallenge)
	{
		_strategy.ConfigureChallengeBanners(showBannerForLocallyCompletedChallenge, showBannerForLocallyReceivedChallenge, showBannerForRemotelyCompletedChallenge);
	}

	public void LoadPlayerData(string[] playerIdArray, bool loadProfileImages)
	{
		_strategy.LoadPlayerData(playerIdArray, loadProfileImages);
	}

	public void LoadProfilePhotoForLocalPlayer()
	{
		_strategy.LoadProfilePhotoForLocalPlayer();
	}

	public void LoadChallengesList()
	{
		_strategy.LoadChallengesList();
	}

	public void ShowGameCenterViewController(GameServiceViewControllerState viewState, GameServiceLeaderboardTimeScope timeScope, string leaderboardId)
	{
		UnityEngine.Debug.Log("GameServiceContext::ShowGameCenterViewController " + IsPlayerAuthenticated());
		if (!IsPlayerAuthenticated())
		{
			AuthenticateLocalPlayer();
		}
		_strategy.ShowGameCenterViewController(viewState, timeScope, leaderboardId);
	}

	public void ReportScore(long score, string leaderboardId)
	{
		_strategy.ReportScore(score, leaderboardId);
	}

	public void GetFriends()
	{
		_strategy.RetrieveFriends();
	}

	public void SendGiftRequest(GPGameRequestType type, int requestLifetimeDays, Texture2D icon, string description, string playload = "")
	{
		if (icon == null)
		{
			if (_coinsLogoForGift == null)
			{
				_coinsLogoForGift = Resources.Load("Textures/Slugterra_Coin") as Texture2D;
			}
			_strategy.SendGiftRequest(type, requestLifetimeDays, _coinsLogoForGift, description, playload);
		}
		else
		{
			_strategy.SendGiftRequest(type, requestLifetimeDays, icon, description, playload);
		}
	}

	public void ShowGameGifts()
	{
		_strategy.ShowGameGifts();
	}

	public void OnChallengesLoadedEvent(List<GameServiceChallenge> gameCenterChallenges)
	{
		challengeCount = 0;
		List<GameServiceChallenge> list = new List<GameServiceChallenge>();
		for (int i = 0; i < gameCenterChallenges.Count; i++)
		{
			if (gameCenterChallenges[i].State == GameServiceChallengeState.Pending)
			{
				list.Add(gameCenterChallenges[i]);
				challengeCount++;
			}
		}
		_gameCenterChallenges = gameCenterChallenges;
		if (OnChallengesLoaded__BackingField != null)
		{
			OnChallengesLoaded__BackingField(list);
		}
	}

	public void OnPlayerDidSelectChallengeEvent(GameServiceChallenge endlessChallenge)
	{
		SelectedChallenge = endlessChallenge;
		if (OnPlayerDidSelectChallenge__BackingField != null)
		{
			OnPlayerDidSelectChallenge__BackingField(endlessChallenge);
		}
	}

	private void OnReportScoreFailedEvent(string message)
	{
		UnityEngine.Debug.LogWarning("GameServiceContext::OnReportScoreFailedEvent " + message);
	}

	private void OnReportScoreFinishedEvent(string message)
	{
		UnityEngine.Debug.Log("GameServiceContext::OnReportScoreFinishedEvent " + message);
	}

	private void OnRetrieveScoresFailedEvent(string message)
	{
		UnityEngine.Debug.Log("GameServiceContext::OnRetrieveScoresFailedEvent " + message);
	}

	private void OnScoresForPlayerIdLoadedEvent(List<GameServiceScore> scores)
	{
		UnityEngine.Debug.Log("GameServiceContext::OnScoresForPlayerIdLoadedEvent " + scores.Count);
		if (OnScoresForPlayerIdLoaded__BackingField != null)
		{
			OnScoresForPlayerIdLoaded__BackingField(scores);
		}
	}

	private void OnGiftResultSuccessEvent()
	{
		if (OnGiftRequestSentSuccess__BackingField != null)
		{
			OnGiftRequestSentSuccess__BackingField();
		}
	}

	private void OnTokenLoadedEvent(string token)
	{
		if (OnTokenLoaded__BackingField != null)
		{
			OnTokenLoaded__BackingField(token);
		}
	}

	private void OnGameRequestAcceptedEvent(List<GPGameRequest> gifts)
	{
		foreach (GPGameRequest gift in gifts)
		{
			UnityEngine.Debug.Log(("GameServiceContext::OnGameRequestAcceptedEvent Request " + gift.id != null) ? (gift.id + " accepted. Playload: " + gift.playload) : " ");
			int num = 0;
			try
			{
				num = Convert.ToInt32(gift.playload);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("GameServiceContext::OnGameRequestAcceptedEvent Exception " + ex.ToString() + " source data " + gift.playload);
				UnityEngine.Debug.LogException(ex);
				num = 0;
			}
			MonoSingleton<AppController>.Instance.Model.Wallet.Gold += num;
		}
	}
}
