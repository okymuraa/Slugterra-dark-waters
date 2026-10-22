using System;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Slugterra.Domain;
using Slugterra.Game.Mission;
using Slugterra.Game.Survival;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class ChallengeResultView : MonoBehaviour
	{
		[SerializeField]
		private TextMesh _score;

		[SerializeField]
		private TextMesh _challengeScore;

		[SerializeField]
		private TextMesh _rivalName;

		[SerializeField]
		private TextMesh _time;

		[SerializeField]
		private TextMesh _killedEnemies;

		[SerializeField]
		private tk2dSpriteFromTexture _enemyAvatar;

		private void OnEnable()
		{
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			if (gameController != null && gameController.Model.State == GameState.Survival)
			{
				float time = MonoSingleton<MissionStatisticsController>.Instance.Model.Time;
				string text = ((int)(time / 60f)/*cast due to constrained. prefix*/).ToString();
				string text2 = Math.Round(time % 60f, 1).ToString();
				int value = (int)time + 1 + 3 * (MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount - 1);
				value = Mathf.Clamp(value, 0, int.MaxValue);
				_score.text = value.ToString();
				string leaderBoardID = SurvivalController.GetLeaderBoardID();
				string survivaBestScoresKey = SurvivalController.GetSurvivaBestScoresKey();
				if (!PlayerPrefsSystem.HasKey(survivaBestScoresKey))
				{
					PlayerPrefs.SetInt(survivaBestScoresKey, value);
				}
				_challengeScore.text = DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge.Score.Value.ToString();
				_rivalName.text = MonoSingleton<SurvivalManager>.Instance.Enemy.Alias;
				if (!string.Equals(text, "0"))
				{
					_time.text = string.Format("{0}m {1}s", text, text2);
				}
				else
				{
					_time.text = string.Format("{0}s", text2);
				}
				_enemyAvatar.texture = MonoSingleton<SurvivalManager>.Instance.Enemy.ProfilePhoto;
				_enemyAvatar.ForceBuild();
				_killedEnemies.text = MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount.ToString();
			}
		}
	}
}
