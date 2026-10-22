using System;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.UI.SurvivalMenu
{
	public class MultiplayerMenuController : UIController<MultiplayerMenuView>
	{
		private float updateDelay = 1f;

		private float updateTime;

		protected override void OnStart()
		{
			base.OnStart();
			string text = string.Format("{0:D2}:{1:D2}:{2:D2}", 0, 0, 0);
			View.textTicketsCountdown.text = text;
			View.ApplyModel(MonoSingleton<AppController>.Instance.Model.Levels);
			updateTime = Time.time + updateDelay;
		}

		protected override void _OnEnable()
		{
			View.OnBackEvent += OnBack;
			View.OnBtnGameServiceEvent += OnBtnGameService;
			View.OnBtnNextClickEvent += OnBtnNext;
		}

		protected override void _OnDisable()
		{
			View.OnBackEvent -= OnBack;
			View.OnBtnGameServiceEvent -= OnBtnGameService;
			View.OnBtnNextClickEvent -= OnBtnNext;
		}

		private void OnBack()
		{
			SceneLoader.LoadScene<MainSceneController>();
		}

		private void OnBtnGameService()
		{
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Leaderboards, GameServiceLeaderboardTimeScope.AllTime, "CgkIjZj4j6kZEAIQHQ");
		}

		private void OnBtnNext()
		{
			View._lobby.SetActive(true);
			gameObject.SetActive(false);
		}

		protected override void OnUpdate()
		{
			if (!(updateTime >= Time.time))
			{
				return;
			}
			if (View.CurTicketCount <= 0f)
			{
				TimeSpan addTicketsData = GetAddTicketsData();
				if (addTicketsData.Ticks == 0)
				{
					MonoSingleton<AppController>.Instance.Model.Wallet.Ticket += 10f;
					View.CurTicketCount += 10f;
					View.UpdateItem();
					PlayerPrefs.DeleteKey("NextTicketsDate");
				}
				else
				{
					string text = string.Format("{0:D2}:{1:D2}:{2:D2}", addTicketsData.Hours, addTicketsData.Minutes, addTicketsData.Seconds);
					View.textTicketsCountdown.text = text;
				}
			}
			updateTime = Time.time + updateDelay;
		}

		private TimeSpan GetAddTicketsData()
		{
			DateTime now = DateTime.Now;
			TimeSpan result = default(TimeSpan);
			if (PlayerPrefsSystem.HasKey("NextTicketsDate"))
			{
				DateTime result2;
				if (DateTime.TryParse(PlayerPrefs.GetString("NextTicketsDate"), out result2))
				{
					if (result2.Ticks >= now.Ticks)
					{
						return result2 - now;
					}
					PlayerPrefs.SetString("NextTicketsDate", now.AddMinutes(90.0).ToString());
				}
				return result;
			}
			PlayerPrefs.SetString("NextTicketsDate", now.AddMinutes(90.0).ToString());
			return now.AddMinutes(90.0) - now;
		}
	}
}
