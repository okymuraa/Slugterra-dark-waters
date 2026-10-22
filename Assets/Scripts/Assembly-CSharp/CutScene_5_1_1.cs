using System.Collections;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_5_1_1 : SlugterraCutScene
{
	[SerializeField]
	private GameObject Self;

	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject MainCamera;

	[SerializeField]
	private GameObject UICamera;

	[SerializeField]
	private GameObject CutBack_UI;

	[SerializeField]
	private GameObject EliAvatar_UI;

	[SerializeField]
	private GameObject Eli;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera.GetComponent<CameraFade>().StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		MainCamera.GetComponent<Animation>().enabled = true;
		MainCamera.GetComponent<Animation>().Stop();
		float a = 3f;
		StartCoroutine(TextHint_3(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(1) || (Input.GetMouseButtonUp(0) && _isSkipEnable))
		{
			Fade();
			_isSkipEnable = false;
			MainCamera.GetComponent<Animation>().Stop();
			MainCamera.GetComponent<CameraSmoothFollow>().enabled = true;
			float a = 3f;
			StartCoroutine(OnGameWin(a));
		}
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		Eli.GetComponent<EliController>().enabled = false;
		UICamera.SetActive(false);
		MainCamera.GetComponent<CameraSmoothFollow>().enabled = false;
		MainCamera.GetComponent<Animation>().Play("CameraTrack_2");
	}

	private IEnumerator Fade(float a)
	{
		yield return new WaitForSeconds(a);
		Fade();
	}

	private void Fade()
	{
		if (_isSkipEnable)
		{
			MainCamera.GetComponent<CameraFade>().StartFade(new Color(0f, 0f, 0f, 1f), 2f, 1f, 1f);
		}
	}

	private IEnumerator TextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.DialogueText = "txt cut 3";
		float time = 3f;
		StartCoroutine(HideTextHint_3(time));
		_isSkipEnable = true;
	}

	private IEnumerator HideTextHint_3(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_4(a2));
		}
	}

	private IEnumerator TextHint_4(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = "txt cut 4";
			float a2 = 3f;
			StartCoroutine(HideTextHint_4(a2));
		}
	}

	private IEnumerator HideTextHint_4(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_5(a2));
		}
	}

	private IEnumerator TextHint_5(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = "txt cut 5";
			float a2 = 6f;
			StartCoroutine(HideTextHint_5(a2));
			float a3 = 4f;
			StartCoroutine(Fade(a3));
		}
	}

	private IEnumerator HideTextHint_5(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_isSkipEnable = false;
			MainCamera.GetComponent<Animation>().Stop();
			MainCamera.GetComponent<CameraSmoothFollow>().enabled = true;
			float a2 = 1f;
			StartCoroutine(OnGameWin(a2));
		}
	}

	private IEnumerator OnGameWin(float a)
	{
		yield return new WaitForSeconds(a);
		gameController.OnFinishGame();
		RisePlayCutSceneEvent(false);
		Self.SetActive(false);
	}
}
