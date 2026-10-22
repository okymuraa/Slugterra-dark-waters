using System;
using AppsMinistry.Core;
using Slugterra.Domain;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SurvivalChallengeResult : MonoBehaviour
	{
		[SerializeField]
		private ChallengeResultView _defeated;

		[SerializeField]
		private ChallengeResultView _unDefeated;

		private void OnEnable()
		{
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			if (gameController != null && gameController.Model.State == GameState.Survival)
			{
				_defeated.gameObject.SetActive(false);
				_unDefeated.gameObject.SetActive(true);
				float time = MonoSingleton<MissionStatisticsController>.Instance.Model.Time;
				string text = ((int)(time / 60f)/*cast due to constrained. prefix*/).ToString();
				string text2 = Math.Round(time % 60f, 1).ToString();
				float value = (int)time + 1 + 3 * (MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount - 1);
				value = Mathf.Clamp(value, 0f, 2.1474836E+09f);
				if (value > (float)DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge.Score.Value)
				{
					_unDefeated.gameObject.SetActive(false);
					_defeated.gameObject.SetActive(true);
				}
			}
		}
	}
}
