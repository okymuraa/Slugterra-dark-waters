using AppsMinistry.Core;
using AppsMinistry.Core.Facebook;
using AppsMinistry.UI;
using Facebook.Unity;
using Slugterra.App;

namespace Slugterra.UI.Popup
{
	public class PopupFacebookGoalsController : UIController<PopupFacebookGoalsView>
	{
		private bool _isLikeButtonClicked;

		protected override void _OnEnable()
		{
			View.OnLoginEvent += OnLogin;
			View.OnInviteEvent += OnInvite;
			View.OnLikeEvent += OnLike;
			View.OnShareEvent += OnShare;
			FacebookController.OnLoggedIn += CheckFacebookActions;
			FacebookController.OnRewardSuccess += OnRewardSuccessHandler;
			CheckFacebookActions();
			View.WalletView.SetActive(false);
		}

		protected override void _OnDisable()
		{
			View.OnLoginEvent -= OnLogin;
			View.OnInviteEvent -= OnInvite;
			View.OnLikeEvent -= OnLike;
			View.OnShareEvent -= OnShare;
			FacebookController.OnLoggedIn -= CheckFacebookActions;
			FacebookController.OnRewardSuccess -= OnRewardSuccessHandler;
		}

		private void OnLogin()
		{
			DonDestroyMonoSingleton<FacebookController>.Instance.Login();
		}

		private void OnShare()
		{
			DonDestroyMonoSingleton<FacebookController>.Instance.ShareLink();
		}

		private void OnInvite()
		{
			DonDestroyMonoSingleton<FacebookController>.Instance.Invite();
		}

		private void OnLike()
		{
			_isLikeButtonClicked = true;
			if (DonDestroyMonoSingleton<FacebookController>.Instance.IsAppLiked)
			{
				DonDestroyMonoSingleton<FacebookController>.Instance.SetLikeRecievedThroughApp();
				CheckFacebookActions();
			}
			else
			{
				DonDestroyMonoSingleton<FacebookController>.Instance.LikeApp();
			}
		}

		private void CheckFacebookActions()
		{
			if (!FB.IsLoggedIn)
			{
				View.LoginStateView.SetEnabledState();
			}
			else
			{
				SetView(View.LoginStateView, DonDestroyMonoSingleton<FacebookController>.Instance.IsFirstLogin());
			}
			SetView(View.LikeStateView, !DonDestroyMonoSingleton<FacebookController>.Instance.IsLikeRecievedThroughApp());
			SetView(View.ShareStateView, DonDestroyMonoSingleton<FacebookController>.Instance.IsFirstShared());
			SetView(View.InviteStateView, DonDestroyMonoSingleton<FacebookController>.Instance.IsFirstInvite());
		}

		private void SetView(PopupFacebookGoalsView.FacebookActionStateView actionView, bool isNotRecievedReward)
		{
			if (!isNotRecievedReward)
			{
				actionView.SetRewardedState();
			}
			else if (FB.IsLoggedIn)
			{
				actionView.SetEnabledState();
			}
			else
			{
				actionView.SetDisabledState();
			}
		}

		private void OnRewardSuccessHandler(FacebookRewardType rewardType)
		{
			if (rewardType == FacebookRewardType.Like && _isLikeButtonClicked && DonDestroyMonoSingleton<FacebookController>.Instance.IsAppLiked)
			{
				DonDestroyMonoSingleton<FacebookController>.Instance.SetLikeRecievedThroughApp();
			}
			if (View.WalletView.activeSelf)
			{
				View.WalletView.SetActive(true);
			}
			CheckFacebookActions();
			MonoSingleton<AppController>.Instance.Model.Wallet.SetFacebookReward(FacebookController.GetRewardCost(rewardType));
		}
	}
}
