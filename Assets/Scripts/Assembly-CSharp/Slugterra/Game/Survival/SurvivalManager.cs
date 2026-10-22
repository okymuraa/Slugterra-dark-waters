using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain;
using Slugterra.Game.Mission;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Survival
{
	public class SurvivalManager : MonoSingleton<SurvivalManager>, IObserver
	{
		private GameServicePlayer _Me;

		private GameServicePlayer _Enemy;

		public GameServicePlayer Enemy
		{
			get
			{
				return _Enemy;
			}
		}

		public void Initialize()
		{
			Object.FindObjectOfType<GameController>().Model.AddObserver(this);
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge != null)
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.GetFriends();
			}
		}

		private void OnEnable()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.OnPlayerDataLoaded += OnPlayerDataLoadedEvent;
			DonDestroyMonoSingleton<GameServiceContext>.Instance.OnProfilePhotoLoaded += OnLocalPlayerPhotoLoadedEvent;
		}

		private void OnDisable()
		{
			if (DonDestroyMonoSingleton<GameServiceContext>.IsAlive)
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.OnPlayerDataLoaded -= OnPlayerDataLoadedEvent;
				DonDestroyMonoSingleton<GameServiceContext>.Instance.OnProfilePhotoLoaded -= OnLocalPlayerPhotoLoadedEvent;
			}
		}

		public void OnObjectChanged(object observable)
		{
			Debug.Log("SurvivalManager::OnObjectChanged");
			if (observable is GameModel)
			{
				GameModel gameModel = observable as GameModel;
				Debug.LogFormat("SurvivalManager::OnObjectChanged model == null ? {0}; model.State == Domain.GameState.Survival ?  ", gameModel == null, gameModel.State == GameState.Survival);
				if (gameModel != null && gameModel.State == GameState.Survival)
				{
					float time = MonoSingleton<MissionStatisticsController>.Instance.Model.Time;
					int killedEnemyCount = MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount;
					int num = (int)time + 1 + 3 * (killedEnemyCount - 1);
					string leaderBoardID = SurvivalController.GetLeaderBoardID();
					DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportScore(num, leaderBoardID);
				}
			}
		}

		private void InitLocalPlayer()
		{
		}

		private void OnPlayerDataLoadedEvent(List<GameServicePlayer> playerData)
		{
			foreach (GameServicePlayer playerDatum in playerData)
			{
				if (playerDatum.PlayerId == DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerIdentifier())
				{
					_Me = playerDatum;
					break;
				}
			}
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge == null)
			{
				return;
			}
			foreach (GameServicePlayer playerDatum2 in playerData)
			{
				if (playerDatum2.PlayerId == DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge.IssuingPlayerID)
				{
					_Enemy = playerDatum2;
					_Enemy.LoadPhoto();
					break;
				}
			}
		}

		private void OnLocalPlayerPhotoLoadedEvent(string pathToPhoto)
		{
		}
	}
}
