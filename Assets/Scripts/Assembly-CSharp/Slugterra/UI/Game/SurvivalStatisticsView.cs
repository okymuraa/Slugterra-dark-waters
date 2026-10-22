using AppsMinistry.Core;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	internal class SurvivalStatisticsView : UIView
	{
		[SerializeField]
		private GameObject _challengeMode;

		[SerializeField]
		private GameObject _singleMode;

		[SerializeField]
		private GameObject _btnChallenge;

		[SerializeField]
		private GameObject _btnLeaderboard;

		private void OnEnable()
		{
			_btnChallenge.SetActive(false);
			_btnLeaderboard.SetActive(true);
			_challengeMode.SetActive(false);
			_singleMode.SetActive(true);
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge != null)
			{
				_singleMode.SetActive(false);
				_challengeMode.SetActive(true);
			}
		}
	}
}
