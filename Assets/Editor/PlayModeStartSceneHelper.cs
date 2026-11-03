#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class PlayModeStartSceneHelper
{
	private const string MENU_NAME = "Slugterra/Always Start From Splash (Loading Screen)";
	private const string PREF_KEY = "Slugterra_PlayFromSplashScene";

	static PlayModeStartSceneHelper()
	{
		EditorApplication.delayCall += UpdatePlayModeStartScene;
	}

	[MenuItem(MENU_NAME, false, 100)]
	private static void TogglePlayFromSplash()
	{
		bool currentState = EditorPrefs.GetBool(PREF_KEY, true);
		EditorPrefs.SetBool(PREF_KEY, !currentState);
		UpdatePlayModeStartScene();
	}

	[MenuItem(MENU_NAME, true)]
	private static bool TogglePlayFromSplashValidate()
	{
		Menu.SetChecked(MENU_NAME, EditorPrefs.GetBool(PREF_KEY, true));
		return true;
	}

	private static void UpdatePlayModeStartScene()
	{
		bool enabled = EditorPrefs.GetBool(PREF_KEY, true);
		if (enabled)
		{
			string splashScenePath = "Assets/Scenes/Menu/SplashScene.unity";
			SceneAsset splashScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(splashScenePath);
			if (splashScene != null)
			{
				EditorSceneManager.playModeStartScene = splashScene;
				return;
			}
		}
		EditorSceneManager.playModeStartScene = null;
	}
}
#endif
