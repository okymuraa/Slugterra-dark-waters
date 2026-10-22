using System.Text;
using Facebook.MiniJSON;
using Facebook.Unity.Editor.Dialogs;
using System;
using System.Collections.Generic;
using Facebook.Unity.Canvas;
using Facebook.Unity.Editor;
using Facebook.Unity.Mobile;
using Facebook.Unity.Mobile.Android;
using Facebook.Unity.Mobile.IOS;
using UnityEngine;

namespace Facebook.Unity
{
	public sealed class FB : ScriptableObject
	{
		private delegate void OnDLLLoaded();

		public sealed class Canvas
		{
			private static IPayFacebook FacebookPayImpl
			{
				get
				{
					IPayFacebook payFacebook = FacebookImpl as IPayFacebook;
					if (payFacebook == null)
					{
						throw new InvalidOperationException("Attempt to call Facebook pay interface on unsupported platform");
					}
					return payFacebook;
				}
			}

			public static void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, FacebookDelegate<IPayResult> callback = null)
			{
				FacebookPayImpl.Pay(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
			}
		}

		public sealed class Mobile
		{
			public static ShareDialogMode ShareDialogMode
			{
				get
				{
					return MobileFacebookImpl.ShareDialogMode;
				}
				set
				{
					MobileFacebookImpl.ShareDialogMode = value;
				}
			}

			private static IMobileFacebook MobileFacebookImpl
			{
				get
				{
					IMobileFacebook mobileFacebook = FacebookImpl as IMobileFacebook;
					if (mobileFacebook == null)
					{
						throw new InvalidOperationException("Attempt to call Mobile interface on non mobile platform");
					}
					return mobileFacebook;
				}
			}

			public static void AppInvite(Uri appLinkUrl, Uri previewImageUrl = null, FacebookDelegate<IAppInviteResult> callback = null)
			{
				MobileFacebookImpl.AppInvite(appLinkUrl, previewImageUrl, callback);
			}

			public static void FetchDeferredAppLinkData(FacebookDelegate<IAppLinkResult> callback = null)
			{
				if (callback != null)
				{
					MobileFacebookImpl.FetchDeferredAppLink(callback);
				}
			}

			public static void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback = null)
			{
				MobileFacebookImpl.RefreshCurrentAccessToken(callback);
			}
		}

		public sealed class Android
		{
			public static string KeyHash
			{
				get
				{
					AndroidFacebook androidFacebook = FacebookImpl as AndroidFacebook;
					return (androidFacebook == null) ? string.Empty : androidFacebook.KeyHash;
				}
			}
		}

		internal abstract class CompiledFacebookLoader : MonoBehaviour
		{
			protected abstract FacebookGameObject FBGameObject { get; }

			public void Start()
			{
				facebook = FBGameObject.Facebook;
				OnDLLLoadedDelegate();
				LogVersion();
				UnityEngine.Object.Destroy(this);
			}
		}

		private const string DefaultJSSDKLocale = "en_US";

		private static IFacebook facebook;

		private static bool isInitCalled;

		private static string facebookDomain = "facebook.com";

		private static string graphApiVersion = "v2.6";

		public static string AppId { get; private set; }

		public static string GraphApiVersion
		{
			get
			{
				return graphApiVersion;
			}
			set
			{
				graphApiVersion = value;
			}
		}

		public static bool IsLoggedIn
		{
			get
			{
				return facebook != null && FacebookImpl.LoggedIn;
			}
		}

		public static bool IsInitialized
		{
			get
			{
				return facebook != null && facebook.Initialized;
			}
		}

		public static bool LimitAppEventUsage
		{
			get
			{
				return facebook != null && facebook.LimitEventUsage;
			}
			set
			{
				if (facebook != null)
				{
					facebook.LimitEventUsage = value;
				}
			}
		}

		internal static IFacebook FacebookImpl
		{
			get
			{
				if (facebook == null)
				{
					throw new NullReferenceException("Facebook object is not yet loaded.  Did you call FB.Init()?");
				}
				return facebook;
			}
			set
			{
				facebook = value;
			}
		}

		internal static string FacebookDomain
		{
			get
			{
				return facebookDomain;
			}
			set
			{
				facebookDomain = value;
			}
		}

		private static OnDLLLoaded OnDLLLoadedDelegate { get; set; }

		public static void Init(InitDelegate onInitComplete = null, HideUnityDelegate onHideUnity = null, string authResponse = null)
		{
			Init(FacebookSettings.AppId, FacebookSettings.Cookie, FacebookSettings.Logging, FacebookSettings.Status, FacebookSettings.Xfbml, FacebookSettings.FrictionlessRequests, authResponse, "en_US", onHideUnity, onInitComplete);
		}

		public static void Init(string appId, bool cookie = true, bool logging = true, bool status = true, bool xfbml = false, bool frictionlessRequests = true, string authResponse = null, string javascriptSDKLocale = "en_US", HideUnityDelegate onHideUnity = null, InitDelegate onInitComplete = null)
		{
			if (string.IsNullOrEmpty(appId))
			{
				throw new ArgumentException("appId cannot be null or empty!");
			}
			AppId = appId;
			if (!isInitCalled)
			{
				isInitCalled = true;
				if (Constants.IsEditor)
				{
					OnDLLLoadedDelegate = () =>
					{
						((EditorFacebook)facebook).Init(onHideUnity, onInitComplete);
					};
					ComponentFactory.GetComponent<EditorFacebookLoader>();
					return;
				}
				switch (Constants.CurrentPlatform)
				{
				case FacebookUnityPlatform.WebGL:
				case FacebookUnityPlatform.WebPlayer:
					OnDLLLoadedDelegate = () =>
					{
						((CanvasFacebook)facebook).Init(appId, cookie, logging, status, xfbml, FacebookSettings.ChannelUrl, authResponse, frictionlessRequests, javascriptSDKLocale, Constants.DebugMode, onHideUnity, onInitComplete);
					};
					ComponentFactory.GetComponent<CanvasFacebookLoader>();
					break;
				case FacebookUnityPlatform.IOS:
					OnDLLLoadedDelegate = () =>
					{
						((IOSFacebook)facebook).Init(appId, frictionlessRequests, FacebookSettings.IosURLSuffix, onHideUnity, onInitComplete);
					};
					ComponentFactory.GetComponent<IOSFacebookLoader>();
					break;
				case FacebookUnityPlatform.Android:
					OnDLLLoadedDelegate = () =>
					{
						((AndroidFacebook)facebook).Init(appId, onHideUnity, onInitComplete);
					};
					ComponentFactory.GetComponent<AndroidFacebookLoader>();
					break;
				default:
					throw new NotImplementedException("Facebook API does not yet support this platform");
				}
			}
			else
			{
				FacebookLogger.Warn("FB.Init() has already been called.  You only need to call this once and only once.");
			}
		}

		public static void LogInWithPublishPermissions(IEnumerable<string> permissions = null, FacebookDelegate<ILoginResult> callback = null)
		{
			FacebookImpl.LogInWithPublishPermissions(permissions, callback);
		}

		public static void LogInWithReadPermissions(IEnumerable<string> permissions = null, FacebookDelegate<ILoginResult> callback = null)
		{
			FacebookImpl.LogInWithReadPermissions(permissions, callback);
		}

		public static void LogOut()
		{
			FacebookImpl.LogOut();
		}

		public static void AppRequest(string message, OGActionType actionType, string objectId, IEnumerable<string> to, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, actionType, objectId, to, null, null, null, data, title, callback);
		}

		public static void AppRequest(string message, OGActionType actionType, string objectId, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, actionType, objectId, null, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public static void AppRequest(string message, IEnumerable<string> to = null, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public static void ShareLink(Uri contentURL = null, string contentTitle = "", string contentDescription = "", Uri photoURL = null, FacebookDelegate<IShareResult> callback = null)
		{
			FacebookImpl.ShareLink(contentURL, contentTitle, contentDescription, photoURL, callback);
		}

		public static void FeedShare(string toId = "", Uri link = null, string linkName = "", string linkCaption = "", string linkDescription = "", Uri picture = null, string mediaSource = "", FacebookDelegate<IShareResult> callback = null)
		{
			FacebookImpl.FeedShare(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, callback);
		}

		public static void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback = null, IDictionary<string, string> formData = null)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query", "The query param cannot be null or empty");
			}
			FacebookImpl.API(query, method, formData, callback);
		}

		public static void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback, WWWForm formData)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query", "The query param cannot be null or empty");
			}
			FacebookImpl.API(query, method, formData, callback);
		}

		public static void ActivateApp()
		{
			FacebookImpl.ActivateApp(AppId);
		}

		public static void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			if (callback != null)
			{
				FacebookImpl.GetAppLink(callback);
			}
		}

		public static void GameGroupCreate(string name, string description, string privacy = "CLOSED", FacebookDelegate<IGroupCreateResult> callback = null)
		{
			FacebookImpl.GameGroupCreate(name, description, privacy, callback);
		}

		public static void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback = null)
		{
			FacebookImpl.GameGroupJoin(id, callback);
		}

		public static void LogAppEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
		{
			FacebookImpl.AppEventsLogEvent(logEvent, valueToSum, parameters);
		}

		public static void LogPurchase(float logPurchase, string currency = null, Dictionary<string, object> parameters = null)
		{
			if (string.IsNullOrEmpty(currency))
			{
				currency = "USD";
			}
			FacebookImpl.AppEventsLogPurchase(logPurchase, currency, parameters);
		}

		private static void LogVersion()
		{
			if (facebook != null)
			{
				FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0} with {1}", FacebookSdkVersion.Build, FacebookImpl.SDKUserAgent));
			}
			else
			{
				FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0}", FacebookSdkVersion.Build));
			}
		}
	}
}


// Merged from Facebook/Unity/Editor so Unity folder convention does not move runtime classes to Assembly-CSharp-Editor


namespace Facebook.Unity.Editor.Dialogs
{
	internal class EmptyMockDialog : EditorFacebookMockDialog
	{
		public string EmptyDialogTitle { get; set; }

		protected override string DialogTitle
		{
			get
			{
				return EmptyDialogTitle;
			}
		}

		protected override void DoGui()
		{
		}

		protected override void SendSuccessResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did_complete"] = true;
			if (!string.IsNullOrEmpty(CallbackID))
			{
				dictionary["callback_id"] = CallbackID;
			}
			if (Callback != null)
			{
				Callback(new ResultContainer(dictionary));
			}
		}
	}
}

namespace Facebook.Unity.Editor.Dialogs
{
	internal class MockLoginDialog : EditorFacebookMockDialog
	{
		private string accessToken = string.Empty;

		protected override string DialogTitle
		{
			get
			{
				return "Mock Login Dialog";
			}
		}

		protected override void DoGui()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("User Access Token:");
			accessToken = GUILayout.TextField(accessToken, GUI.skin.textArea, GUILayout.MinWidth(400f));
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			if (GUILayout.Button("Find Access Token"))
			{
				Application.OpenURL(string.Format("https://developers.facebook.com/tools/accesstoken/?app_id={0}", FB.AppId));
			}
			GUILayout.Space(20f);
		}

		protected override void SendSuccessResult()
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				SendErrorResult("Empty Access token string");
				return;
			}
			FB.API("/me?fields=id&access_token=" + accessToken, HttpMethod.GET, (IGraphResult graphResult) =>
			{
				if (!string.IsNullOrEmpty(graphResult.Error))
				{
					SendErrorResult("Graph API error: " + graphResult.Error);
				}
				else
				{
					string facebookID = graphResult.ResultDictionary["id"] as string;
					FB.API("/me/permissions?access_token=" + accessToken, HttpMethod.GET, (IGraphResult permResult) =>
					{
						if (!string.IsNullOrEmpty(permResult.Error))
						{
							SendErrorResult("Graph API error: " + permResult.Error);
						}
						else
						{
							List<string> list = new List<string>();
							List<string> list2 = new List<string>();
							List<object> list3 = permResult.ResultDictionary["data"] as List<object>;
							foreach (Dictionary<string, object> item in list3)
							{
								if (item["status"] as string == "granted")
								{
									list.Add(item["permission"] as string);
								}
								else
								{
									list2.Add(item["permission"] as string);
								}
							}
							AccessToken accessToken = new AccessToken(this.accessToken, facebookID, DateTime.UtcNow.AddDays(60.0), list, DateTime.UtcNow);
							IDictionary<string, object> dictionary2 = (IDictionary<string, object>)Json.Deserialize(accessToken.ToJson());
							dictionary2.Add("granted_permissions", list);
							dictionary2.Add("declined_permissions", list2);
							if (!string.IsNullOrEmpty(CallbackID))
							{
								dictionary2["callback_id"] = CallbackID;
							}
							if (Callback != null)
							{
								Callback(new ResultContainer(dictionary2));
							}
						}
					});
				}
			});
		}
	}
}

namespace Facebook.Unity.Editor.Dialogs
{
	internal class MockShareDialog : EditorFacebookMockDialog
	{
		public string SubTitle { private get; set; }

		protected override string DialogTitle
		{
			get
			{
				return "Mock " + SubTitle + " Dialog";
			}
		}

		protected override void DoGui()
		{
		}

		protected override void SendSuccessResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (FB.IsLoggedIn)
			{
				dictionary["postId"] = GenerateFakePostID();
			}
			else
			{
				dictionary["did_complete"] = true;
			}
			if (!string.IsNullOrEmpty(CallbackID))
			{
				dictionary["callback_id"] = CallbackID;
			}
			if (Callback != null)
			{
				Callback(new ResultContainer(dictionary));
			}
		}

		protected override void SendCancelResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["cancelled"] = "true";
			if (!string.IsNullOrEmpty(CallbackID))
			{
				dictionary["callback_id"] = CallbackID;
			}
			Callback(new ResultContainer(dictionary));
		}

		private string GenerateFakePostID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(AccessToken.CurrentAccessToken.UserId);
			stringBuilder.Append('_');
			for (int i = 0; i < 17; i++)
			{
				stringBuilder.Append(UnityEngine.Random.Range(0, 10));
			}
			return stringBuilder.ToString();
		}
	}
}

namespace Facebook.Unity.Editor
{
	internal class EditorFacebook : FacebookBase, IMobileFacebookImplementation, ICanvasFacebookImplementation, IMobileFacebook, IMobileFacebookResultHandler, IFacebook, IFacebookResultHandler, ICanvasFacebook, ICanvasFacebookResultHandler, IPayFacebook
	{
		private const string WarningMessage = "You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.";

		private const string AccessTokenKey = "com.facebook.unity.editor.accesstoken";

		private IEditorWrapper editorWrapper;

		public override bool LimitEventUsage { get; set; }

		public ShareDialogMode ShareDialogMode { get; set; }

		public override string SDKName
		{
			get
			{
				return "FBUnityEditorSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return FacebookSdkVersion.Build;
			}
		}

		private static IFacebookCallbackHandler EditorGameObject
		{
			get
			{
				return ComponentFactory.GetComponent<EditorFacebookGameObject>();
			}
		}

		public EditorFacebook(IEditorWrapper wrapper, CallbackManager callbackManager)
			: base(callbackManager)
		{
			editorWrapper = wrapper;
		}

		public EditorFacebook()
			: this(new EditorWrapper(EditorGameObject), new CallbackManager())
		{
		}

		public override void Init(HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			FacebookLogger.Warn("You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.");
			base.Init(hideUnityDelegate, onInitComplete);
			editorWrapper.Init();
		}

		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			LogInWithPublishPermissions(permissions, callback);
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			editorWrapper.ShowLoginMockDialog(OnLoginComplete, CallbackManager.AddFacebookDelegate(callback), permissions.ToCommaSeparateList());
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			editorWrapper.ShowAppRequestMockDialog(OnAppRequestsComplete, CallbackManager.AddFacebookDelegate(callback));
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			editorWrapper.ShowMockShareDialog(OnShareLinkComplete, "ShareLink", CallbackManager.AddFacebookDelegate(callback));
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			editorWrapper.ShowMockShareDialog(OnShareLinkComplete, "FeedShare", CallbackManager.AddFacebookDelegate(callback));
		}

		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			editorWrapper.ShowGameGroupCreateMockDialog(OnGroupCreateComplete, CallbackManager.AddFacebookDelegate(callback));
		}

		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			editorWrapper.ShowGameGroupJoinMockDialog(OnGroupJoinComplete, CallbackManager.AddFacebookDelegate(callback));
		}

		public override void ActivateApp(string appId)
		{
			FacebookLogger.Info("This only needs to be called for iOS or Android.");
		}

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["callback_id"] = CallbackManager.AddFacebookDelegate(callback);
			OnGetAppLinkComplete(new ResultContainer(dictionary));
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			editorWrapper.ShowAppInviteMockDialog(OnAppInviteComplete, CallbackManager.AddFacebookDelegate(callback));
		}

		public void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["ref"] = "mock ref";
			dictionary["extras"] = new Dictionary<string, object> { { "mock extra key", "mock extra value" } };
			dictionary["target_url"] = "mocktargeturl://mocktarget.url";
			dictionary["callback_id"] = CallbackManager.AddFacebookDelegate(callback);
			OnFetchDeferredAppLinkComplete(new ResultContainer(dictionary));
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			editorWrapper.ShowPayMockDialog(OnPayComplete, CallbackManager.AddFacebookDelegate(callback));
		}

		public void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			if (callback != null)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("callback_id", CallbackManager.AddFacebookDelegate(callback));
				Dictionary<string, object> dictionary2 = dictionary;
				if (AccessToken.CurrentAccessToken == null)
				{
					dictionary2["error"] = "No current access token";
				}
				else
				{
					IDictionary<string, object> source = (IDictionary<string, object>)Json.Deserialize(AccessToken.CurrentAccessToken.ToJson());
					dictionary2.AddAllKVPFrom(source);
				}
				OnRefreshCurrentAccessTokenComplete(new ResultContainer(dictionary2));
			}
		}

		public override void OnAppRequestsComplete(ResultContainer resultContainer)
		{
			AppRequestResult result = new AppRequestResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGetAppLinkComplete(ResultContainer resultContainer)
		{
			AppLinkResult result = new AppLinkResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupCreateComplete(ResultContainer resultContainer)
		{
			GroupCreateResult result = new GroupCreateResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupJoinComplete(ResultContainer resultContainer)
		{
			GroupJoinResult result = new GroupJoinResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public override void OnLoginComplete(ResultContainer resultContainer)
		{
			LoginResult result = new LoginResult(resultContainer);
			OnAuthResponse(result);
		}

		public override void OnShareLinkComplete(ResultContainer resultContainer)
		{
			ShareResult result = new ShareResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public void OnAppInviteComplete(ResultContainer resultContainer)
		{
			AppInviteResult result = new AppInviteResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public void OnFetchDeferredAppLinkComplete(ResultContainer resultContainer)
		{
			AppLinkResult result = new AppLinkResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public void OnPayComplete(ResultContainer resultContainer)
		{
			PayResult result = new PayResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public void OnRefreshCurrentAccessTokenComplete(ResultContainer resultContainer)
		{
			AccessTokenRefreshResult result = new AccessTokenRefreshResult(resultContainer);
			CallbackManager.OnFacebookResponse(result);
		}

		public void OnFacebookAuthResponseChange(ResultContainer resultContainer)
		{
			throw new NotSupportedException();
		}

		public void OnUrlResponse(string message)
		{
			throw new NotSupportedException();
		}
	}
}
namespace Facebook.Unity.Editor
{
	internal class EditorFacebookGameObject : FacebookGameObject
	{
	}
}
namespace Facebook.Unity.Editor
{
	internal class EditorFacebookLoader : FB.CompiledFacebookLoader
	{
		protected override FacebookGameObject FBGameObject
		{
			get
			{
				EditorFacebookGameObject component = ComponentFactory.GetComponent<EditorFacebookGameObject>();
				component.Facebook = new EditorFacebook();
				return component;
			}
		}
	}
}

namespace Facebook.Unity.Editor
{
	internal abstract class EditorFacebookMockDialog : MonoBehaviour
	{
		private Rect modalRect;

		private GUIStyle modalStyle;

		public Utilities.Callback<ResultContainer> Callback { protected get; set; }

		public string CallbackID { protected get; set; }

		protected abstract string DialogTitle { get; }

		public void Start()
		{
			modalRect = new Rect(10f, 10f, Screen.width - 20, Screen.height - 20);
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1f));
			texture2D.Apply();
			modalStyle = new GUIStyle();
			modalStyle.normal.background = texture2D;
		}

		public void OnGUI()
		{
			GUI.ModalWindow(GetHashCode(), modalRect, OnGUIDialog, DialogTitle, modalStyle);
		}

		protected abstract void DoGui();

		protected abstract void SendSuccessResult();

		protected virtual void SendCancelResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["cancelled"] = true;
			if (!string.IsNullOrEmpty(CallbackID))
			{
				dictionary["callback_id"] = CallbackID;
			}
			Callback(new ResultContainer(dictionary.ToJson()));
		}

		protected virtual void SendErrorResult(string errorMessage)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["error"] = errorMessage;
			if (!string.IsNullOrEmpty(CallbackID))
			{
				dictionary["callback_id"] = CallbackID;
			}
			Callback(new ResultContainer(dictionary.ToJson()));
		}

		private void OnGUIDialog(int windowId)
		{
			GUILayout.Space(10f);
			GUILayout.BeginVertical();
			GUILayout.Label("Warning! Mock dialog responses will NOT match production dialogs");
			GUILayout.Label("Test your app on one of the supported platforms");
			DoGui();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUIContent content = new GUIContent("Send Success");
			Rect rect = GUILayoutUtility.GetRect(content, GUI.skin.button);
			if (GUI.Button(rect, content))
			{
				SendSuccessResult();
				UnityEngine.Object.Destroy(this);
			}
			GUIContent content2 = new GUIContent("Send Cancel");
			Rect rect2 = GUILayoutUtility.GetRect(content2, GUI.skin.button);
			if (GUI.Button(rect2, content2, GUI.skin.button))
			{
				SendCancelResult();
				UnityEngine.Object.Destroy(this);
			}
			GUIContent content3 = new GUIContent("Send Error");
			Rect rect3 = GUILayoutUtility.GetRect(content2, GUI.skin.button);
			if (GUI.Button(rect3, content3, GUI.skin.button))
			{
				SendErrorResult("Error: Error button pressed");
				UnityEngine.Object.Destroy(this);
			}
			GUILayout.EndHorizontal();
		}
	}
}

namespace Facebook.Unity.Editor
{
	internal class EditorWrapper : IEditorWrapper
	{
		private IFacebookCallbackHandler callbackHandler;

		public EditorWrapper(IFacebookCallbackHandler callbackHandler)
		{
			this.callbackHandler = callbackHandler;
		}

		public void Init()
		{
			callbackHandler.OnInitComplete(string.Empty);
		}

		public void ShowLoginMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId, string permsisions)
		{
			MockLoginDialog component = ComponentFactory.GetComponent<MockLoginDialog>();
			component.Callback = callback;
			component.CallbackID = callbackId;
		}

		public void ShowAppRequestMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock App Request");
		}

		public void ShowGameGroupCreateMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Create");
		}

		public void ShowGameGroupJoinMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Game Group Join");
		}

		public void ShowAppInviteMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock App Invite");
		}

		public void ShowPayMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId)
		{
			ShowEmptyMockDialog(callback, callbackId, "Mock Pay");
		}

		public void ShowMockShareDialog(Utilities.Callback<ResultContainer> callback, string subTitle, string callbackId)
		{
			MockShareDialog component = ComponentFactory.GetComponent<MockShareDialog>();
			component.SubTitle = subTitle;
			component.Callback = callback;
			component.CallbackID = callbackId;
		}

		private void ShowEmptyMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId, string title)
		{
			EmptyMockDialog component = ComponentFactory.GetComponent<EmptyMockDialog>();
			component.Callback = callback;
			component.CallbackID = callbackId;
			component.EmptyDialogTitle = title;
		}
	}
}
namespace Facebook.Unity.Editor
{
	internal interface IEditorWrapper
	{
		void Init();

		void ShowLoginMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId, string permissions);

		void ShowAppRequestMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId);

		void ShowGameGroupCreateMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId);

		void ShowGameGroupJoinMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId);

		void ShowAppInviteMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId);

		void ShowPayMockDialog(Utilities.Callback<ResultContainer> callback, string callbackId);

		void ShowMockShareDialog(Utilities.Callback<ResultContainer> callback, string subTitle, string callbackId);
	}
}
