using System;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SurvivalPauseResults : MonoBehaviour
	{
		[SerializeField]
		private TextMesh _score;

		[SerializeField]
		private ScoreBest _previousBestScore;

		[SerializeField]
		private ScoreBest _challengeScore;

		[SerializeField]
		private TextMesh _time;

		[SerializeField]
		private TextMesh _killedEnemies;

		private void OnEnable()
		{
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			_challengeScore.parent.SetActive(false);
			_previousBestScore.parent.SetActive(true);
			if (!(gameController != null) || !SurvivalController.IsSurvival(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission))
			{
				return;
			}
			float time = MonoSingleton<MissionStatisticsController>.Instance.Model.Time;
			string arg;
			string arg2;
			string arg3;
			if (TimeSpan.FromSeconds(time).Days == 0)
			{
				string[] array = TimeSpan.FromSeconds(time).ToString().Split(':', ':', '.');
				arg = array[2];
				arg2 = array[1];
				arg3 = array[0];
			}
			else
			{
				string[] array = TimeSpan.FromSeconds(time).ToString().Split(':', ':', ':', '.');
				arg = array[3];
				arg2 = array[2];
				int result;
				int.TryParse(array[1], out result);
				arg3 = (result + TimeSpan.FromSeconds(time).Days * 24).ToString();
			}
			int value = (int)time + 1 + 3 * (MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount - 1);
			value = Mathf.Clamp(value, 0, int.MaxValue);
			_score.text = value.ToString();
			float x = -0.1325f * (float)(value.ToString().Length - 1);
			_score.transform.parent.localPosition = new Vector3(x, 0f, 0f);
			string survivaBestScoresKey = SurvivalController.GetSurvivaBestScoresKey();
			if (!PlayerPrefsSystem.HasKey(survivaBestScoresKey))
			{
				PlayerPrefs.SetInt(survivaBestScoresKey, value);
			}
			int num = PlayerPrefs.GetInt(survivaBestScoresKey);
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge != null)
			{
				_previousBestScore.parent.SetActive(false);
				_challengeScore.parent.SetActive(true);
				float x2 = -0.085f * (float)(DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge.Score.Value.ToString().Length - 1);
				_challengeScore.score.transform.parent.localPosition = new Vector3(x2, 0f, 0f);
				_challengeScore.score.text = DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge.Score.Value.ToString();
			}
			else
			{
				_challengeScore.parent.SetActive(false);
				_previousBestScore.parent.SetActive(true);
				_previousBestScore.score.text = num.ToString();
				float x3 = -0.085f * (float)(num.ToString().Length - 1);
				_previousBestScore.score.transform.parent.localPosition = new Vector3(x3, 0f, 0f);
				if (num < value)
				{
					PlayerPrefs.SetInt(survivaBestScoresKey, value);
				}
			}
			_time.text = string.Format("{0}:{1}:{2}", arg3, arg2, arg);
			_killedEnemies.text = MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount.ToString();
		}
	}
}
