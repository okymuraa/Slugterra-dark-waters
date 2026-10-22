using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_6_3_3 : SlugterraCutScene
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
	private GameObject Twist1;

	[SerializeField]
	private GameObject Twist2;

	[SerializeField]
	private GameObject Twist3;

	[SerializeField]
	private GameObject OnKillEnemyObjective;

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
		StartCoroutine(TextHint_7(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
		TwistGoToOnStart();
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(1) || (Input.GetMouseButtonUp(0) && _isSkipEnable))
		{
			Fade();
			_isSkipEnable = false;
			MainCamera.GetComponent<Animation>().enabled = false;
			MainCamera.GetComponent<CameraSmoothFollow>().enabled = true;
			UICamera.SetActive(true);
			ShowScreens(false);
			TwistGoToOnEnd();
			float a = 3f;
			StartCoroutine(Skip(a));
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
		MainCamera.GetComponent<Animation>().Play("CameraTrack_3");
		ElyGoToOnStart();
	}

	private IEnumerator Skip(float a)
	{
		yield return new WaitForSeconds(a);
		_isSkipEnable = false;
		Ely.GetComponent<EliController>().enabled = true;
		TwistCopiesTurnOn();
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
	}

	private IEnumerator TextHint_7(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = "txt cut 7";
		float time = 4f;
		StartCoroutine(HideTextHint_7(time));
		_isSkipEnable = true;
	}

	private IEnumerator HideTextHint_7(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_8(a2));
		}
	}

	private IEnumerator TextHint_8(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = "txt cut 8";
			float a2 = 4f;
			StartCoroutine(HideTextHint_8(a2));
		}
	}

	private IEnumerator HideTextHint_8(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_9(a2));
		}
	}

	private IEnumerator TextHint_9(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = "txt cut 9";
			float a2 = 4f;
			StartCoroutine(HideTextHint_9(a2));
		}
	}

	private IEnumerator HideTextHint_9(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			float a2 = 1f;
			StartCoroutine(TextHint_10(a2));
		}
	}

	private IEnumerator TextHint_10(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = "txt cut 10";
			float a2 = 4f;
			StartCoroutine(Fade(a2));
			float a3 = 6f;
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
			TwistGoToOnEnd();
			MainCamera.GetComponent<Animation>().Stop();
			ShowScreens(false);
			UICamera.SetActive(true);
			MainCamera.GetComponent<CameraSmoothFollow>().enabled = true;
			float a2 = 1f;
			StartCoroutine(Skip(a2));
		}
	}

	private void ElyGoToOnStart()
	{
		Ely.GetComponent<EliController>().MoveTarget.position = ElyGoTo_AB_3.transform.position;
		Ely.GetComponent<EliController>().OnMove(7f);
		if (Ally != null)
		{
			Ally.GetComponent<AssistantController>().MoveTarget.position = AllyGoTo_AB_3.transform.position;
			Ally.GetComponent<AssistantController>().OnMove(7f);
		}
		float a = 2.5f;
		StartCoroutine(RotateEly(a));
		float a2 = 1.5f;
		StartCoroutine(RotateTwist(a2));
	}

	private IEnumerator RotateEly(float a)
	{
		yield return new WaitForSeconds(a);
		Ely.transform.eulerAngles = ElyGoTo_AB_3.eulerAngles;
	}

	private IEnumerator RotateTwist(float a)
	{
		yield return new WaitForSeconds(a);
		Twist.transform.eulerAngles = TwistGoTo_AB_3_1.eulerAngles;
	}

	private void TwistGoToOnStart()
	{
		Twist.GetComponent<TwistController>().MoveTarget.position = TwistGoTo_AB_3_1.position;
		Twist.GetComponent<TwistController>().OnMove(10f);
	}

	private void TwistGoToOnEnd()
	{
		Twist.GetComponent<TwistController>().MoveTarget.position = TwistGoTo_AB_3_2.position;
		Twist.GetComponent<TwistController>().OnMove(10f);
		float a = 5f;
		StartCoroutine(End(a));
	}

	private IEnumerator End(float a)
	{
		yield return new WaitForSeconds(a);
		Object.Destroy(Twist);
		Self.SetActive(false);
	}

	private void TwistCopiesTurnOn()
	{
		Twist1.SetActive(true);
		Twist2.SetActive(true);
		Twist3.SetActive(true);
		OnKillEnemyObjective.GetComponent<KillEnemyObjective>().Active = true;
	}
}
