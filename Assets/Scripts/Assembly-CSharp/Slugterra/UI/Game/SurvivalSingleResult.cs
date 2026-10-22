using System;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SurvivalSingleResult : MonoBehaviour
	{
		[SerializeField]
		private TextMesh _score;

		[SerializeField]
		private TextMesh _previousBestScore;

		[SerializeField]
		private TextMesh _time;

		[SerializeField]
		private TextMesh _killedEnemies;

		[SerializeField]
		private GameObject[] _bestScoresObject;

		[SerializeField]
		private GameObject[] _normalScoresObject;

		private void OnEnable()
		{
			SetBestScoresGroupActive(false);
			SetNormalScoresGroupActive(true);
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
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
			_previousBestScore.text = num.ToString();
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge != null)
			{
				_previousBestScore.text = DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge.Score.Value.ToString();
			}
			else
			{
				float x2 = -0.085f * (float)(num.ToString().Length - 1);
				_previousBestScore.transform.parent.localPosition = new Vector3(x2, 0f, 0f);
				if (num < value)
				{
					SetNormalScoresGroupActive(false);
					SetBestScoresGroupActive(true);
					PlayerPrefs.SetInt(survivaBestScoresKey, value);
				}
			}
			_time.text = string.Format("{0}:{1}:{2}", arg3, arg2, arg);
			_killedEnemies.text = MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount.ToString();
		}

		private void SetBestScoresGroupActive(bool isActive)
		{
			if (_bestScoresObject == null)
			{
				return;
			}
			for (int i = 0; i < _bestScoresObject.Length; i++)
			{
				if (_bestScoresObject[i] != null)
				{
					_bestScoresObject[i].SetActive(isActive);
				}
			}
		}

		private void SetNormalScoresGroupActive(bool isActive)
		{
			if (_normalScoresObject == null)
			{
				return;
			}
			for (int i = 0; i < _normalScoresObject.Length; i++)
			{
				if (_normalScoresObject[i] != null)
				{
					_normalScoresObject[i].SetActive(isActive);
				}
			}
		}
	}
}
