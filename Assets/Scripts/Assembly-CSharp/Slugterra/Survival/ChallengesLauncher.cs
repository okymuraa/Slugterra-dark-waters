using System;
using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Scene;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.Survival
{
	[RequireComponent(typeof(GameServiceContext))]
	public class ChallengesLauncher : MonoBehaviour
	{
		private void OnEnable()
		{
			if (transform.parent == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			DonDestroyMonoSingleton<GameServiceContext>.Instance.OnPlayerDidSelectChallenge += OnPlayerDidSelectChallenge;
		}

		private void OnDisable()
		{
			if (DonDestroyMonoSingleton<GameServiceContext>.IsAlive)
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.OnPlayerDidSelectChallenge -= OnPlayerDidSelectChallenge;
			}
		}

		private void OnPlayerDidSelectChallenge(GameServiceChallenge challenge)
		{
			if (challenge != null && challenge.Score != null)
			{
				Debug.Log("ChallengesLauncher::OnPlayerDidSelectChallenge Category: " + challenge.Score.Category);
				Type mission = null;
				for (int i = 0; i < MonoSingleton<AppController>.Instance.Model.Levels.Count; i++)
				{
					if (MonoSingleton<AppController>.Instance.Model.Levels[i].Name == "survival")
					{
						LeaderBoardIDAttribute leaderBoardIDAttribute = Attribute.GetCustomAttribute(MonoSingleton<AppController>.Instance.Model.Levels[i].Scene, typeof(LeaderBoardIDAttribute)) as LeaderBoardIDAttribute;
						if (leaderBoardIDAttribute.LeaderBoardId == challenge.Score.Category)
						{
							mission = MonoSingleton<AppController>.Instance.Model.Levels[i].Scene;
						}
					}
				}
				AppModel model = MonoSingleton<AppController>.Instance.Model;
				model.MissionSetting.Mission = mission;
				SceneLoader.LoadScene<BackpackSceneController>();
			}
			else
			{
				SceneLoader.LoadScene<SurvivalMapSceneController>();
			}
		}
	}
}
