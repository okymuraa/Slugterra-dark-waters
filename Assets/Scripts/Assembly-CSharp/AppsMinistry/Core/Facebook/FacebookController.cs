using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Facebook.Unity;
using Slugterra.UI.Popup;
using TinyTeam.Debuger;
using UnityEngine;

namespace AppsMinistry.Core.Facebook
{
	public class FacebookController : DonDestroyMonoSingleton<FacebookController>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnInitialized__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnLoggedIn__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnInviteSuccess__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnShared__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnUploadScreenshot__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnAppLiked__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<FacebookRewardType> OnRewardSuccess__BackingField;

		private bool _isExecuting;

		private PopupConfirmView _popupConfirm;

		public const int ACTION_COST_LOGIN = 500;

		public const int ACTION_COST_SHARE = 300;

		public const int ACTION_COST_LIKE = 200;

		public const int ACTION_COST_INVITE = 500;

		private const string SHARE_PIC_URL = "https://lh3.googleusercontent.com/9C_0n9qm8po_dlf3QQ_19iqOzQTYuHmD5oc75GG_4z6vfciGfIkkiVKf-bOyYL6ncCP7=w300-rw";

		private const string APP_LINK = "https://fb.me/83877282958204";

		public string postTitle = "win_title";

		public bool IsExecuting
		{
			get
			{
				return _isExecuting;
			}
		}

		public bool IsAppLiked { get; private set; }

		public static event Action OnInitialized
		{
			add
			{
				Action action = OnInitialized__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInitialized__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnInitialized__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInitialized__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action OnLoggedIn
		{
			add
			{
				Action action = OnLoggedIn__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLoggedIn__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnLoggedIn__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnLoggedIn__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action OnInviteSuccess
		{
			add
			{
				Action action = OnInviteSuccess__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInviteSuccess__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnInviteSuccess__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnInviteSuccess__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action OnShared
		{
			add
			{
				Action action = OnShared__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShared__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnShared__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShared__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action OnUploadScreenshot
		{
			add
			{
				Action action = OnUploadScreenshot__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUploadScreenshot__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnUploadScreenshot__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUploadScreenshot__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action OnAppLiked
		{
			add
			{
				Action action = OnAppLiked__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAppLiked__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAppLiked__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAppLiked__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action<FacebookRewardType> OnRewardSuccess
		{
			add
			{
				Action<FacebookRewardType> action = OnRewardSuccess__BackingField;
				Action<FacebookRewardType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRewardSuccess__BackingField, (Action<FacebookRewardType>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<FacebookRewardType> action = OnRewardSuccess__BackingField;
				Action<FacebookRewardType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRewardSuccess__BackingField, (Action<FacebookRewardType>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			if (DonDestroyMonoSingleton<FacebookController>.Instance != this)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			else if (!FB.IsInitialized)
			{
				FB.Init(InitCallback, OnHideUnity);
			}
			else
			{
				FB.ActivateApp();
			}
		}

		private void OnEnable()
		{
			OnInitialized += OnInitializedHandler;
			OnAppLiked += OnAppLikedHandler;
			OnInviteSuccess += OnInviteSuccessHandler;
			OnLoggedIn += OnLoginHandler;
			OnShared += OnSharedHandler;
			OnUploadScreenshot += OnUploadScreenshotHandler;
			TinyTeam.Debuger.Console.RegisterCommand("recommendGame", CMDRecommendGame, "recommend game over facebook friends");
			TinyTeam.Debuger.Console.RegisterCommand("sendFbGift", CMDSendFBGift, "Send Gift");
			TinyTeam.Debuger.Console.RegisterCommand("SendBrag", CMDBrag, "Seng Game");
		}

		private void OnDisable()
		{
			OnInitialized -= OnInitializedHandler;
			OnAppLiked -= OnAppLikedHandler;
			OnInviteSuccess -= OnInviteSuccessHandler;
			OnLoggedIn -= OnLoginHandler;
			OnShared -= OnSharedHandler;
			OnUploadScreenshot -= OnUploadScreenshotHandler;
			TinyTeam.Debuger.Console.UnRegisterCommand("recommendGame");
			TinyTeam.Debuger.Console.UnRegisterCommand("sendFbGift");
			TinyTeam.Debuger.Console.UnRegisterCommand("SendBrag");
		}

		public static int GetRewardCost(FacebookRewardType rewardType)
		{
			switch (rewardType)
			{
			case FacebookRewardType.Login:
				return 500;
			case FacebookRewardType.Like:
				return 200;
			case FacebookRewardType.Share:
				return 300;
			case FacebookRewardType.Invite:
				return 500;
			default:
				return 0;
			}
		}

		public void Login()
		{
			if (IsExecuting)
			{
				return;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsInitialized(Login) && !FB.IsLoggedIn)
				{
					SetExecuteState(true);
					List<string> permissions = new List<string> { "publish_actions" };
					FB.LogInWithPublishPermissions(permissions, AuthCallback);
				}
			}, (string errorMsg) =>
			{
				_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		public void Logout()
		{
			if (FB.IsInitialized && FB.IsLoggedIn)
			{
				FB.LogOut();
			}
		}

		public bool IsLoggedIn()
		{
			if (!FB.IsInitialized)
			{
				return false;
			}
			if (!FB.IsLoggedIn)
			{
				return false;
			}
			return true;
		}

		public string GetUserID()
		{
			if (AccessToken.CurrentAccessToken != null)
			{
				return AccessToken.CurrentAccessToken.UserId;
			}
			return string.Empty;
		}

		public void Invite()
		{
			UnityEngine.Debug.Log("FacebookController::Invite " + IsExecuting);
			if (IsExecuting)
			{
				return;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsInitialized(Invite))
				{
					SetExecuteState(true);
					FB.AppRequest("Come play this game!", null, null, null, null, null, null, AppRequestCallback);
				}
			}, (string errorMsg) =>
			{
				_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		public void ShareLink()
		{
			UnityEngine.Debug.Log("FacebookController::ShareLink " + IsExecuting);
			if (IsExecuting)
			{
				return;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsLogined(ShareLink))
				{
					SetExecuteState(true);
					FB.ShareLink(new Uri("https://www.facebook.com/slugterra.darkwaters/?fref=ts"), "Slugterra. Dark waters", "Play the game!", new Uri("https://lh3.googleusercontent.com/9C_0n9qm8po_dlf3QQ_19iqOzQTYuHmD5oc75GG_4z6vfciGfIkkiVKf-bOyYL6ncCP7=w300-rw"), ShareCallback);
				}
			}, (string errorMsg) =>
			{
				_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		public void UploadScreenshot()
		{
			UnityEngine.Debug.Log("FacebookController::UploadScreenshot " + IsExecuting);
			if (IsExecuting)
			{
				return;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsLogined(UploadScreenshot))
				{
					if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
					{
						UnityEngine.Debug.LogWarning("FacebookController::UploadScreenshot have \"publish_actions\" permission");
						SetExecuteState(true);
						StartCoroutine(TakeScreenshot());
					}
					else
					{
						UnityEngine.Debug.LogWarning("FacebookController::UploadScreenshot have \"publish_actions\" permission");
					}
				}
			}, (string errorMsg) =>
			{
				_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		public void FeedShare()
		{
			if (!IsExecuting && IsLogined(FeedShare))
			{
				FB.FeedShare(string.Empty, new Uri("https://www.youtube.com/watch?v=nZU3UftvxPc?autoplay=1"), "Slugterra: Dark Waters", string.Empty, MonoSingleton<Localization>.Instance.GetValueByKey("facebook_text_1"), null, "https://www.youtube.com/watch?v=nZU3UftvxPc", FeedCallback);
			}
		}

		public void LikeApp()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsLogined(LikeApp))
				{
					string url = "https://www.facebook.com/slugterra.darkwaters/";
					Application.OpenURL(url);
				}
			}, (string errorMsg) =>
			{
				_popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		public bool IsFirstLogin()
		{
			return GetBoolPrefs("facebookLogined");
		}

		public bool IsFirstInvite()
		{
			return GetBoolPrefs("facebookInvited");
		}

		public bool IsFirstShared()
		{
			return GetBoolPrefs("facebookShared");
		}

		public bool IsLikeRecievedThroughApp()
		{
			return GetBoolPrefs("facebookLikedApp", false);
		}

		public void SetLikeRecievedThroughApp()
		{
			SetBoolPrefs("facebookLikedApp", true);
		}

		public string GetAccessToken()
		{
			string result = string.Empty;
			if (AccessToken.CurrentAccessToken != null && !string.IsNullOrEmpty(AccessToken.CurrentAccessToken.TokenString))
			{
				result = AccessToken.CurrentAccessToken.TokenString;
			}
			return result;
		}

		private void SetExecuteState(bool isActive)
		{
			_isExecuting = isActive;
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (FB.IsInitialized && !pauseStatus)
			{
				FetchDeepLinks();
				FetchUserLikes();
			}
		}

		private void FetchUserLikes()
		{
			if (FB.IsLoggedIn)
			{
				FB.API("me/likes", HttpMethod.GET, LikeAppCallback);
			}
		}

		private void FetchDeepLinks()
		{
			FB.Mobile.FetchDeferredAppLinkData(DefferedAppLinkCallback);
			FB.GetAppLink(AppLinkCallback);
		}

		private void FetchFacebookActionsData()
		{
		}

		private IEnumerator TakeScreenshot()
		{
			UnityEngine.Debug.Log("FacebookController::TakeScreenshot");
			yield return new WaitForEndOfFrame();
			int width = Screen.width;
			int height = Screen.height;
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
			tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			tex.Apply();
			byte[] screenshot = tex.EncodeToPNG();
			WWWForm wwwForm = new WWWForm();
			wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");
			string applicationUrl = "GooglePlay: https://play.google.com/store/apps/details?id=com.appsministry.slugterra";
			if (string.IsNullOrEmpty(postTitle))
			{
				postTitle = "win_title";
			}
			string localizedTitle = MonoSingleton<Localization>.Instance.GetValueByKey(postTitle, postTitle);
			wwwForm.AddField("caption", localizedTitle + "\nhttps://fb.me/83877282958204\nFacebook Group: https://fb.me/83877282958204");
			FB.API("me/photos", HttpMethod.POST, ScreenshotUploadCallback, wwwForm);
		}

		private bool IsCorrectResult(IResult result)
		{
			if (result == null || !string.IsNullOrEmpty(result.Error))
			{
				string text = ((result != null) ? result.Error : "null");
				UnityEngine.Debug.Log("something goes wrong with result:" + text);
				return false;
			}
			return true;
		}

		private bool IsInitialized(Action callback)
		{
			OnInitialized -= callback;
			if (!FB.IsInitialized)
			{
				OnInitialized += callback;
				FB.Init(InitCallback, OnHideUnity);
				return false;
			}
			return true;
		}

		private bool IsLogined(Action callback)
		{
			OnLoggedIn -= callback;
			if (!FB.IsLoggedIn)
			{
				OnLoggedIn += callback;
				Login();
				return false;
			}
			return true;
		}

		private bool GetBoolPrefs(string key, bool defaultValue = true)
		{
			int defaultValue2 = (defaultValue ? 1 : 0);
			return PlayerPrefs.GetInt(key, defaultValue2) == 1;
		}

		private void SetBoolPrefs(string key, bool value)
		{
			PlayerPrefs.SetInt(key, value ? 1 : 0);
		}

		private void AuthCallback(ILoginResult result)
		{
			SetExecuteState(false);
			UnityEngine.Debug.Log("FacebookController::Login raw: \n" + result.RawResult);
			if (FB.IsLoggedIn)
			{
				AccessToken currentAccessToken = AccessToken.CurrentAccessToken;
				List<string> list = new List<string>(currentAccessToken.Permissions);
				string text = string.Empty;
				for (int i = 0; i < list.Count; i++)
				{
					text = text + list[i] + " ";
				}
				UnityEngine.Debug.Log("User id:" + currentAccessToken.UserId + " Permissions: " + text);
				GameServiceContext.FaceBookPlayerID = AccessToken.CurrentAccessToken.UserId;
				FetchUserLikes();
				RaiseOnLoggedInEvent();
			}
			else
			{
				UnityEngine.Debug.Log("User cancelled login");
			}
		}

		private void InitCallback()
		{
			SetExecuteState(false);
			if (FB.IsInitialized)
			{
				FB.ActivateApp();
				RaiseOnInitialized();
			}
			else
			{
				UnityEngine.Debug.LogWarning("Failed to Initialize the Facebook SDK");
			}
		}

		private void OnHideUnity(bool isGameShown)
		{
			if (!isGameShown)
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}

		private void AppInviteCallback(IAppInviteResult result)
		{
			SetExecuteState(false);
			UnityEngine.Debug.Log("FACEBOOK::INVITE RAW:" + result.RawResult);
			if (IsCorrectResult(result))
			{
				if (result.Cancelled)
				{
					UnityEngine.Debug.Log("User canceled invite");
				}
				else
				{
					RaiseOnInviteSuccess();
				}
			}
		}

		private void AppRequestCallback(IAppRequestResult result)
		{
			SetExecuteState(false);
			UnityEngine.Debug.Log("FACEBOOK::APP REQUEST:" + result.RawResult);
			if (!IsCorrectResult(result))
			{
				return;
			}
			if (result.Cancelled)
			{
				UnityEngine.Debug.Log("User canceled request/invite");
				return;
			}
			int num = result.To.Count();
			if (num > 0)
			{
				RaiseOnInviteSuccess();
			}
		}

		private void ShareCallback(IShareResult result)
		{
			SetExecuteState(false);
			UnityEngine.Debug.Log("FACEBOOK::SHARE RAW:" + result.RawResult);
			UnityEngine.Debug.Log("FACEBOOK::SHARE POSTID:" + result.PostId);
			if (IsCorrectResult(result))
			{
				if (result.Cancelled)
				{
					UnityEngine.Debug.Log("Sharing canceled");
				}
				else if (!string.IsNullOrEmpty(result.PostId) || result.RawResult.Contains("postId"))
				{
					RaiseOnShared();
				}
			}
		}

		private void FeedCallback(IShareResult result)
		{
			SetExecuteState(false);
			if (IsCorrectResult(result))
			{
				if (result.Cancelled)
				{
					UnityEngine.Debug.Log("Feedshare canceled");
				}
				else
				{
					RaiseOnShared();
				}
			}
		}

		private void ScreenshotUploadCallback(IGraphResult result)
		{
			SetExecuteState(false);
			UnityEngine.Debug.Log("FacebookController::ScreenshotUploadCallback result: " + result.RawResult);
			if (IsCorrectResult(result))
			{
				if (result.Cancelled)
				{
					UnityEngine.Debug.Log("User canceled upload screenshot");
				}
				else
				{
					RaiseOnUploadScreenshot();
				}
			}
		}

		private void AppLinkCallback(IAppLinkResult result)
		{
			SetExecuteState(false);
			if (IsCorrectResult(result))
			{
				UnityEngine.Debug.Log("App link raw:" + result.RawResult);
			}
		}

		private void DefferedAppLinkCallback(IAppLinkResult result)
		{
			SetExecuteState(false);
			if (IsCorrectResult(result))
			{
				UnityEngine.Debug.Log("Deffered app link raw:" + result.RawResult);
			}
		}

		private void LikeAppCallback(IGraphResult result)
		{
			SetExecuteState(false);
			UnityEngine.Debug.Log("FACEBOOK::LIKE app raw:" + result.RawResult);
			if (IsCorrectResult(result) && result.RawResult.Contains("Slugterra: Dark Waters"))
			{
				UnityEngine.Debug.Log("app is liked");
				RaiseOnAppLiked();
			}
		}

		private void RaiseOnInitialized()
		{
			if (OnInitialized__BackingField != null)
			{
				OnInitialized__BackingField();
			}
		}

		private void RaiseOnLoggedInEvent()
		{
			if (OnLoggedIn__BackingField != null)
			{
				OnLoggedIn__BackingField();
			}
		}

		private void RaiseOnInviteSuccess()
		{
			if (OnInviteSuccess__BackingField != null)
			{
				OnInviteSuccess__BackingField();
			}
		}

		private void RaiseOnShared()
		{
			if (OnShared__BackingField != null)
			{
				OnShared__BackingField();
			}
		}

		private void RaiseOnUploadScreenshot()
		{
			if (OnUploadScreenshot__BackingField != null)
			{
				OnUploadScreenshot__BackingField();
			}
		}

		private void RaiseOnAppLiked()
		{
			if (OnAppLiked__BackingField != null)
			{
				OnAppLiked__BackingField();
			}
		}

		private void RaiseOnRewardSuccess(FacebookRewardType rewardType)
		{
			if (OnRewardSuccess__BackingField != null)
			{
				OnRewardSuccess__BackingField(rewardType);
			}
		}

		private void OnInitializedHandler()
		{
			FetchDeepLinks();
			FetchUserLikes();
			FetchFacebookActionsData();
		}

		private void OnAppLikedHandler()
		{
			IsAppLiked = true;
			RaiseOnRewardSuccess(FacebookRewardType.Like);
		}

		private void OnInviteSuccessHandler()
		{
			if (IsFirstInvite())
			{
				SetBoolPrefs("facebookInvited", false);
				RaiseOnRewardSuccess(FacebookRewardType.Invite);
			}
		}

		private void OnLoginHandler()
		{
			if (IsFirstLogin())
			{
				SetBoolPrefs("facebookLogined", false);
				RaiseOnRewardSuccess(FacebookRewardType.Login);
			}
		}

		private void OnSharedHandler()
		{
			if (IsFirstShared())
			{
				SetBoolPrefs("facebookShared", false);
				RaiseOnRewardSuccess(FacebookRewardType.Share);
			}
		}

		private void OnUploadScreenshotHandler()
		{
		}

		private void RecommendGame()
		{
			if (IsExecuting)
			{
				return;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsLogined(RecommendGame) && AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
				{
					UnityEngine.Debug.LogWarning("FacebookController::RecommendGame have \"publish_actions\" permission");
					SetExecuteState(true);
					FB.AppRequest("Come play this great game!", null, null, null, null, null, null, (IAppRequestResult result) =>
					{
						UnityEngine.Debug.Log(result.RawResult);
						SetExecuteState(false);
					});
				}
			}, null);
		}

		private void SendFBGift()
		{
			if (IsExecuting)
			{
				return;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsLogined(SendFBGift) && AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
				{
					UnityEngine.Debug.LogWarning("FacebookController::SendFBGift have \"publish_actions\" permission");
					SetExecuteState(true);
					FB.AppRequest("Here is a free gift!", null, new List<object> { "app_users" }, null, null, null, null, (IAppRequestResult result) =>
					{
						UnityEngine.Debug.Log(result.RawResult);
						SetExecuteState(false);
					});
				}
			}, null);
		}

		private void SendBrag(int score)
		{
			if (IsExecuting)
			{
				return;
			}
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (IsLogined(SendFBGift) && AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
				{
					UnityEngine.Debug.LogWarning("FacebookController::SendBrag have \"publish_actions\" permission");
					SetExecuteState(true);
					FB.AppRequest("I Just got " + score + " points! Can you beat it?", null, null, null, null, "{\"challenge_score\":" + score + "}", "Slugterra: Dark Waters", (IAppRequestResult result) =>
					{
						UnityEngine.Debug.Log(result.RawResult);
						SetExecuteState(false);
					});
				}
			}, null);
		}

		private object CMDRecommendGame(string[] args)
		{
			RecommendGame();
			return "Call RecommendGame";
		}

		private object CMDSendFBGift(string[] args)
		{
			SendFBGift();
			return "Call SendFBGift";
		}

		private object CMDBrag(string[] args)
		{
			SendBrag(300);
			return "Call SendBrag";
		}
	}
}
