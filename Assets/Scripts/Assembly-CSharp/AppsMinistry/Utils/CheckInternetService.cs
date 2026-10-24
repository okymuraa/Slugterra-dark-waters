using System;
using System.Collections;
using AppsMinistry.Core;
using TinyTeam.Debuger;
using UnityEngine;

namespace AppsMinistry.Utils
{
	public class CheckInternetService : DonDestroyMonoSingleton<CheckInternetService>
	{
		public Action onInternetConnectionAppear;

		public Action onInternetConnectionLost;

		[SerializeField]
		private float _checkInterval = 2f;

		[SerializeField]
		private bool _isInternetAvailable = true;

		private bool showconnectionStatus;

		private float timeout = 5f;

		private bool _pingInWork;

		private bool _forceCheck;

		private int urlIndex;

		private string[] networksUrl = new string[4] { "https://ya.ru", "https://www.bing.com/", "https://yahoo.com", "https://google.com" };

		public bool IsInternetAvailable
		{
			get
			{
				return _isInternetAvailable;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			TinyTeam.Debuger.Console.RegisterCommand("showInternetStatus", CMDShowInternetStatus, "Show Internet Status");
		}

		private void Start()
		{
			urlIndex = UnityEngine.Random.Range(0, networksUrl.Length - 1);
			AppsMinistrySdkBinding.Instance.AMStartNetworkStateNotification();
			StartCoroutine(_Update());
			_forceCheck = true;
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			TinyTeam.Debuger.Console.UnRegisterCommand("showInternetStatus");
		}

		public void CheckInternet(Action OnSuccess, Action<string> OnFail, bool isForceCheck = false)
		{
			_forceCheck = isForceCheck;
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				_isInternetAvailable = false;
				_forceCheck = true;
			}
			timeout = 5f;
			if (_forceCheck)
			{
				timeout = 2f;
			}
			if (_isInternetAvailable)
			{
				if (OnSuccess != null)
				{
					OnSuccess();
				}
			}
			else if (OnFail != null)
			{
				OnFail("No Internet Connection");
			}
		}

		private IEnumerator _Update()
		{
			float startTime = Time.realtimeSinceStartup;
			float deltaTime = 0f;
			while (true)
			{
				deltaTime = Time.realtimeSinceStartup - startTime;
				if ((deltaTime > _checkInterval || _forceCheck) && !_pingInWork)
				{
					startTime = Time.realtimeSinceStartup;
					_forceCheck = false;
					StartCoroutine(CheckAndroidConnection());
				}
				yield return null;
			}
		}

		private IEnumerator CheckAndroidConnection()
		{
			if (Application.isEditor || Application.platform != RuntimePlatform.Android)
			{
				_isInternetAvailable = Application.internetReachability != NetworkReachability.NotReachable;
				_pingInWork = false;
				if (_isInternetAvailable)
				{
					OnInternetConnectionAppear();
				}
				else
				{
					OnInternetConnectionLost();
				}
				yield break;
			}
			float startTime = Time.realtimeSinceStartup;
			_pingInWork = true;
			urlIndex = Mathf.Clamp(urlIndex, 0, networksUrl.Length - 1);
			string testUrl = networksUrl[urlIndex];
			WWW www = new WWW(testUrl);
			while (true)
			{
				if (www.isDone && www.error == null)
				{
					_isInternetAvailable = true;
					_pingInWork = false;
					OnInternetConnectionAppear();
					yield break;
				}
				if (Time.realtimeSinceStartup - startTime > timeout || www.error != null)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			_isInternetAvailable = false;
			_pingInWork = false;
			OnInternetConnectionLost();
			urlIndex++;
			urlIndex %= networksUrl.Length;
		}

		private IEnumerator CheckiOSConnection()
		{
			float startTime = Time.realtimeSinceStartup;
			Ping ping = new Ping("8.8.8.8");
			_pingInWork = true;
			while (true)
			{
				if (ping.isDone)
				{
					_isInternetAvailable = true;
					_pingInWork = false;
					OnInternetConnectionAppear();
					yield break;
				}
				if (Time.realtimeSinceStartup - startTime > timeout)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			_isInternetAvailable = false;
			_pingInWork = false;
			OnInternetConnectionLost();
		}

		private void OnInternetConnectionAppear()
		{
			if (showconnectionStatus)
			{
				Debug.LogWarning("CheckInternetService::OnInternetConnectionAppear Internet connection appear");
			}
			if (onInternetConnectionAppear != null)
			{
				onInternetConnectionAppear();
			}
		}

		private void OnInternetConnectionLost()
		{
			if (showconnectionStatus)
			{
				Debug.LogWarning("CheckInternetService::OnInternetConnectionAppear Internet connection was lost");
			}
			if (onInternetConnectionLost != null)
			{
				onInternetConnectionLost();
			}
		}

		private object CMDShowInternetStatus(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "For enable output status of Connection enter: \"showInternetStatus 1\" \nFor disable output status of Connection enter: \"showInternetStatus 0\" ";
			}
			string text = args[1];
			if (text == "1")
			{
				showconnectionStatus = true;
				return "showconnectionStatus set to true";
			}
			if (text == "0")
			{
				showconnectionStatus = false;
				return "showconnectionStatus set to false";
			}
			return "ERROR!!!\nUsage: For enable output status of Connection enter: \"showInternetStatus 1\" \nFor disable output status of Connection enter: \"showInternetStatus 0\" ";
		}
	}
}
