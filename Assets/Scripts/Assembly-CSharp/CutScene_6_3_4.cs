using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_6_3_4 : SlugterraCutScene
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
	private GameObject Twist;

	[SerializeField]
	private Transform ElyGoTo_AB_3;

	[SerializeField]
	private Transform AllyGoTo_AB_3;

	[SerializeField]
	private Transform TwistGoTo_AB_3_1;

	[SerializeField]
	private Transform TwistGoTo_AB_3_2;

	private GameObject Ely;

	private AssistantController Ally;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera.GetComponent<CameraFade>().StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		float a = 3f;
		StartCoroutine(TextHint_11(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
		Twist.SetActive(true);
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(1) || (Input.GetMouseButtonUp(0) && _isSkipEnable))
		{
			Fade();
			_isSkipEnable = false;
			ShowScreens(false);
			MainCamera.GetComponent<Animation>().Stop();
			MainCamera.GetComponent<CameraSmoothFollow>().enabled = true;
			TwistGoToOnEnd();
			float a = 4f;
			StartCoroutine(OnGameWin(a));
		}
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		Ely = GameObject.FindGameObjectWithTag("Player");
		Ally = Object.FindObjectOfType<AssistantController>();
		Ely.GetComponent<EliController>().enabled = false;
		UICamera.SetActive(false);
		MainCamera.GetComponent<CameraSmoothFollow>().enabled = false;
		MainCamera.GetComponent<Animation>().enabled = true;
		MainCamera.GetComponent<Animation>().Play("CameraTrack_4");
		GoToOnStart();
	}

	private IEnumerator Skip(float a)
	{
		yield return new WaitForSeconds(a);
		_isSkipEnable = false;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		Self.SetActive(false);
	}

	private IEnumerator TextHint_11(float a)
	{
		yield return new WaitForSeconds(a);
		_cutSceneView.SpeakerIcon = "icon_eli";
		ShowScreens(true);
		_cutSceneView.DialogueText = "txt cut";
		float time = 4f;
		StartCoroutine(HideTextHint_11(time));
		_isSkipEnable = true;
	}

	private IEnumerator HideTextHint_11(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_12(a2));
		}
	}

	private IEnumerator TextHint_12(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_twist";
			_cutSceneView.DialogueText = "txt cut";
			float a2 = 4f;
			StartCoroutine(HideTextHint_12(a2));
		}
	}

	private IEnumerator HideTextHint_12(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			TwistGoToOnEnd();
			float a2 = 1f;
			StartCoroutine(TextHint_13(a2));
		}
	}

	private IEnumerator TextHint_13(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.SpeakerIcon = "icon_eli";
			_cutSceneView.DialogueText = "txt cut";
			float a2 = 4f;
			StartCoroutine(HideTextHint_13(a2));
		}
	}

	private IEnumerator HideTextHint_13(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 2f;
			StartCoroutine(Fade(a2));
			float a3 = 4f;
			StartCoroutine(CutSceneEnd(a3));
		}
	}

	private void Fade()
	{
		if (_isSkipEnable)
		{
			MainCamera.GetComponent<CameraFade>().StartFade(new Color(0f, 0f, 0f, 1f), 2f, 1f, 1f);
		}
	}

	private IEnumerator Fade(float a)
	{
		yield return new WaitForSeconds(a);
		Fade();
	}

	private IEnumerator CutSceneEnd(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_isSkipEnable = false;
			MainCamera.GetComponent<Animation>().Stop();
			ShowScreens(false);
			float a2 = 1f;
			StartCoroutine(OnGameWin(a2));
		}
	}

	private void TwistGoToOnEnd()
	{
		Twist.GetComponent<TwistController>().MoveTarget.position = TwistGoTo_AB_3_2.position;
		Twist.GetComponent<TwistController>().OnMove(10f);
	}

	private IEnumerator OnGameWin(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(true);
		gameController.OnFinishGame();
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		Self.SetActive(false);
	}

	private void GoToOnStart()
	{
		Twist.GetComponent<TwistController>().MoveTarget.position = TwistGoTo_AB_3_1.position;
		Twist.transform.position = TwistGoTo_AB_3_1.position;
		Ely.GetComponent<EliController>().MoveTarget.position = ElyGoTo_AB_3.position;
		Ely.transform.position = ElyGoTo_AB_3.position;
		if (Ally != null)
		{
			Ally.GetComponent<AssistantController>().MoveTarget.position = AllyGoTo_AB_3.position;
			Ally.transform.position = AllyGoTo_AB_3.position;
		}
		Rotate();
	}

	private void Rotate()
	{
		Ely.transform.eulerAngles = ElyGoTo_AB_3.eulerAngles;
		Twist.transform.eulerAngles = TwistGoTo_AB_3_1.eulerAngles;
	}
}
