using AppsMinistry.Scene;
using UnityEngine;

namespace Slugterra.Scene.Game
{
	[SceneName("FlaringoParticleTest", "", false, false)]
	public class SlugFlightSceneController : SlugterraSceneController
	{
		private void OnGUI()
		{
			if (GUI.Button(new Rect(5f, 5f, Screen.width / 5, Screen.height / 10), "Main menu"))
			{
				SceneLoader.LoadScene<MainSceneController>();
			}
		}
	}
}
