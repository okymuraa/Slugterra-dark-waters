using AppsMinistry.Core;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.Scene
{
	[SceneName("MainScene", "", false, false)]
	[BundleScene("base", true)]
	public class MainSceneController : SlugterraSceneController
	{
		protected override void OnLoaded()
		{
			base.OnLoaded();
			string text = 2 + "." + 0 + "." + 2275;
			Debug.Log("MainSceneController::OnLoaded Version = " + text);
			PlayerPrefs.SetInt(text, 1);
			PlayerPrefs.SetInt("LoadBundlesEnable", 1);
		}

		protected override void OnBack()
		{
			PopupConfirmView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			if (registeredPopup != null)
			{
				registeredPopup.Show("quit_title", () =>
				{
					Application.Quit();
				});
			}
		}
	}
}
