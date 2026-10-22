using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Scene;

namespace Slugterra.UI.SurvivalMenu
{
	public class SurvivalMenuController : UIController<SurvivalMenuView>
	{
		protected override void OnStart()
		{
			base.OnStart();
			View.ApplyModel(MonoSingleton<AppController>.Instance.Model.Levels);
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
			DonDestroyMonoSingleton<GameServiceContext>.Instance.SelectedChallenge = null;
			DonDestroyMonoSingleton<GameServiceContext>.Instance.ShowGameCenterViewController(GameServiceViewControllerState.Challenges, GameServiceLeaderboardTimeScope.AllTime, null);
		}

		private void OnBtnNext()
		{
			SceneLoader.LoadScene<BackpackSceneController>();
		}
	}
}
