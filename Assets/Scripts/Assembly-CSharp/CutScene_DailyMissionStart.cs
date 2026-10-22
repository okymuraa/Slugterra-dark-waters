using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_DailyMissionStart : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraSmoothFollow.enabled = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		StartCoroutine(CameraFolow());
		StartCoroutine(_Start());
	}

	private IEnumerator CameraFolow()
	{
		yield return new WaitForSeconds(0.1f);
		_cameraSmoothFollow.enabled = true;
	}

	private IEnumerator _Start()
	{
		yield return new WaitForSeconds(2f);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		gameObject.SetActive(false);
	}
}
