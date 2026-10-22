using UnityEngine;

namespace ColorTvPlugin
{
	public static class ColorTv
	{
		public enum Gender
		{
			MALE = 0,
			FEMALE = 1
		}

		private const string GAME_OBJECT_NAME = "ColorTvGameObject";

		private static ColorTvGameObjectBehaviour ColorTvGameObject;

		private static bool DebugMode;

		public static void Init(string appId)
		{
			if (!isAndroidTv())
			{
				Debug.LogWarning("The device isn't a connected TV, ColorTV SDK won't run");
				return;
			}
			if (GameObject.Find("ColorTvGameObject") != null)
			{
				Debug.LogWarning("ColorTv gameobject already in scene!");
				return;
			}
			GameObject gameObject = new GameObject("ColorTvGameObject");
			ColorTvGameObject = gameObject.AddComponent<ColorTvGameObjectBehaviour>();
			Object.DontDestroyOnLoad(ColorTvGameObject);
			ColorTvGameObject.InitPlugin();
			if (DebugMode)
			{
				SetDebugMode(DebugMode);
			}
			ColorTvGameObject.InitSDK(appId);
		}

		public static void ShowAd(string placementId)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.ShowAd(placementId);
			}
		}

		public static void LoadAd(string placementId)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.LoadAd(placementId);
			}
		}

		public static void SetDebugMode(bool enabled)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.SetDebugMode(enabled);
			}
			else
			{
				DebugMode = enabled;
			}
		}

		public static void SetRecordAudioEnabled(bool enabled)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.SetRecordAudioEnabled(enabled);
			}
		}

		public static void SetUserId(string userId)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.SetUserId(userId);
			}
		}

		public static void SetUserAge(int age)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.SetUserAge(age);
			}
		}

		public static void SetUserGender(Gender gender)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.SetUserGender(gender);
			}
		}

		public static void SetUserKeywords(string keywords)
		{
			if (ColorTvGameObject != null)
			{
				ColorTvGameObject.SetUserKeywords(keywords);
			}
		}

		private static bool isAndroidTv()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.content.Context");
			AndroidJavaObject androidJavaObject2 = androidJavaClass2.GetStatic<AndroidJavaObject>("UI_MODE_SERVICE");
			AndroidJavaObject androidJavaObject3 = androidJavaObject.Call<AndroidJavaObject>("getSystemService", new object[1] { androidJavaObject2 });
			int num = androidJavaObject3.Call<int>("getCurrentModeType", new object[0]);
			AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("android.content.res.Configuration");
			int num2 = androidJavaClass3.GetStatic<int>("UI_MODE_TYPE_TELEVISION");
			return num2 == num;
		}
	}
}
