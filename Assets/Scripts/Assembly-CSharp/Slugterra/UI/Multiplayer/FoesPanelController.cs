using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.UI;
using ExitGames.Client.Photon.LoadBalancing;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Multiplayer.Lobby.Match;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class FoesPanelController : UIController<FoesPanelView>
	{
		[SerializeField]
		private MatchItemController _matchItemPrefab;

		private List<MatchItemController> _matchItems;

		private Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo> _model;

		private int _currentTabId = 1;

		protected override void OnStart()
		{
			View.OnTabActivate((LobbyTabEnum)_currentTabId);
		}

		public void FillModel(Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo> model)
		{
			_model = model;
			ClearMatchItems();
			_matchItems = CreateMatches(model);
		}

		private List<MatchItemController> CreateMatches(Dictionary<string, ExitGames.Client.Photon.LoadBalancing.RoomInfo> matchesInfo)
		{
			if (matchesInfo == null || matchesInfo.Count == 0)
			{
				return null;
			}
			List<MatchItemController> list = new List<MatchItemController>(matchesInfo.Count);
			foreach (ExitGames.Client.Photon.LoadBalancing.RoomInfo value in matchesInfo.Values)
			{
				if (!(value.Name == DonDestroyMonoSingleton<GameServiceContext>.Instance.PlayerIdentifier()) && value.MaxPlayers != value.PlayerCount)
				{
					MatchItemController matchItemController = Object.Instantiate(_matchItemPrefab);
					matchItemController.ApplyModel(value);
					View.AddMatchItem(matchItemController);
					list.Add(matchItemController);
				}
			}
			return list;
		}

		private List<MatchItemController> CreateMatches(int count, LobbyTabEnum lobbyType)
		{
			List<MatchItemController> list = new List<MatchItemController>(count);
			for (int i = 1; i <= count; i++)
			{
				MatchItemController matchItemController = Object.Instantiate(_matchItemPrefab);
				PlayerData playerData = new PlayerData();
				playerData.Name = "FakePlayer_" + ((lobbyType != LobbyTabEnum.Friends) ? "All" : "Friend") + "_" + i;
				playerData.Rank = Random.Range(1, 20);
				playerData.AvatarId = 0;
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				playerData.AttackSlug = new PlayerDataSlug(missionSetting.AttackSlug);
				matchItemController.ApplyModel(playerData.ToJson());
				View.AddMatchItem(matchItemController);
				list.Add(matchItemController);
			}
			return list;
		}

		private void ClearMatchItems()
		{
			if (_matchItems != null)
			{
				_matchItems.ForEach((MatchItemController m) =>
				{
					Object.DestroyImmediate(m.gameObject);
				});
				_matchItems.Clear();
			}
		}

		private void OnTabClick(LobbyTabEnum tabType)
		{
			if (_currentTabId != (int)tabType)
			{
				if (_currentTabId > -1)
				{
					View.OnTabDisactivate((LobbyTabEnum)_currentTabId);
				}
				_currentTabId = (int)tabType;
				View.OnTabActivate(tabType);
			}
		}

		private void OnBtnYourFriendsClick()
		{
			ClearMatchItems();
			OnTabClick(LobbyTabEnum.Friends);
			_matchItems = CreateMatches(4, LobbyTabEnum.Friends);
		}

		private void OnBtnAllPlayersClick()
		{
			ClearMatchItems();
			OnTabClick(LobbyTabEnum.All);
			_matchItems = CreateMatches(_model);
		}
	}
}
