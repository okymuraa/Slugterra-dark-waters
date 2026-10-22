using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Scene;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.Scene
{
	[SceneName("SplashScene", "", false, false)]
	public class SplashSceneController : SlugterraSceneController
	{
		protected override void OnLoad()
		{
			if (Camera.main.pixelWidth > 1280)
			{
				tk2dSystem.CurrentPlatform = "2x";
			}
			else
			{
				tk2dSystem.CurrentPlatform = "1x";
			}
			PhotonNetwork.PhotonServerSettings.AppID = "b88e965e-596d-4f99-b4de-da95caffd865";
			MonoSingleton<AppController>.Instance.Load();
			IncrementLaunchCount();
			StartCoroutine(Wait());
		}

		private void IncrementLaunchCount()
		{
			if (PlayerPrefsSystem.HasKey("LaunchCount"))
			{
				int num = PlayerPrefs.GetInt("LaunchCount");
				PlayerPrefs.SetInt("LaunchCount", ++num);
			}
			else
			{
				PlayerPrefs.SetInt("LaunchCount", 1);
			}
		}

		private IEnumerator Wait()
		{
			yield return new WaitForSeconds(0.4f);
			Type nextScene = typeof(ShaneTutor_0_1);
			if (PlayerPrefsSystem.HasKey(typeof(ShaneTutor_0_1).Name))
			{
				nextScene = typeof(MainSceneController);
			}
			string[] bundlesList = new string[3]
			{
				"atlases" + tk2dSystem.CurrentPlatform,
				"base",
				"tutorial"
			};
			DonDestroyMonoSingleton<DownloadManager>.Instance.LoadBundlesWithDownloadScene(new string[0], nextScene);
		}
	}
}
