using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Domain;
using AppsMinistry.Utils;
using UnityEngine;

public class DummyGameServiceStrategy : MonoBehaviour, IGameService
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
	private Action<string> OnTokenLoaded__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GPGameRequest>> OnPendingGiftsDetected__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnAchievmentUpdated__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string> OnAchievmentUpdateFailed__BackingField;

	private string playerId = string.Empty;

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

	public void AuthenticateLocalPlayer()
	{
	}

	public bool IsPlayerAuthenticated()
	{
		return false;
	}

	public void ShowGameCenterViewController(GameServiceViewControllerState viewState, GameServiceLeaderboardTimeScope timeScope, string leaderboardId)
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
	}

	public void RetrieveFriends()
	{
	}

	public void SendGiftRequest(GPGameRequestType type, int requestLifetimeDays, Texture2D icon, string description, string playload = "")
	{
	}

	public void LoadPlayerData(string[] playerIdArray, bool loadProfileImages)
	{
	}

	public void RetrieveScoresForPlayerId(string playerId, string leaderboardId)
	{
	}

	public string PlayerIdentifier()
	{
		if (string.IsNullOrEmpty(playerId))
		{
			playerId = NetworkService.GetUniqueID();
		}
		return playerId;
	}

	public string PlayerAlias()
	{
		return Environment.UserName;
	}

	public void ReportAchievment(string id)
	{
	}

	public void LoadProfilePhotoForLocalPlayer()
	{
	}

	public void ShowGameGifts()
	{
	}
}
