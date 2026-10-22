using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Ally;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_1_1_1 : SlugterraCutScene
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
	private GameObject BackGround;

	[SerializeField]
	private GameObject Speaker;

	[SerializeField]
	private GameObject SpeakerIcon;

	[SerializeField]
	private GameObject Txt_Cut_0;

	[SerializeField]
	private GameObject Txt_Cut_1;

	[SerializeField]
	private GameObject Txt_Cut_2;

	[SerializeField]
	private GameObject Txt_Cut_3;

	[SerializeField]
	private GameObject Txt_Cut_4;

	[SerializeField]
	private GameObject Txt_Cut_5;

	private GameObject Ely;

	private bool _skipEnable;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		MainCamera.GetComponent<CameraFade>().StartFade(new Color(0f, 0f, 0f, 1f), 1f, 0.1f, 2f);
		float a = 3f;
		StartCoroutine(TextHint_0(a));
		float a2 = 1f;
		StartCoroutine(StartCameraTrack(a2));
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(1) || (Input.GetMouseButtonUp(0) && _skipEnable))
		{
			Fade();
			_skipEnable = false;
			MainCamera.GetComponent<Animation>().enabled = false;
			MainCamera.GetComponent<CameraSmoothFollow>().enabled = true;
			UICamera.SetActive(true);
			Txt_Cut_0.SetActive(false);
			Txt_Cut_1.SetActive(false);
			Txt_Cut_2.SetActive(false);
			Speaker.SetActive(false);
			BackGround.SetActive(false);
			float a = 1f;
			StartCoroutine(Skip(a));
		}
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		Ely = GameObject.FindGameObjectWithTag("Player");
		Ely.GetComponent<EliController>().enabled = false;
		UICamera.SetActive(false);
	}

	private IEnumerator Skip(float a)
	{
		yield return new WaitForSeconds(a);
		_skipEnable = false;
		Ely.GetComponent<EliController>().enabled = true;
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		Self.SetActive(false);
	}

	private IEnumerator TextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		BackGround.SetActive(true);
		Speaker.SetActive(true);
		Txt_Cut_0.SetActive(true);
		float time = 4f;
		StartCoroutine(HideTextHint_0(time));
		_skipEnable = true;
	}

	private IEnumerator HideTextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			Txt_Cut_0.SetActive(false);
			Speaker.SetActive(false);
			float a2 = 1f;
			StartCoroutine(TextHint_1(a2));
		}
	}

	private IEnumerator TextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			Txt_Cut_1.SetActive(true);
			Speaker.SetActive(true);
			float a2 = 4f;
			StartCoroutine(HideTextHint_1(a2));
		}
	}

	private IEnumerator HideTextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			Txt_Cut_1.SetActive(false);
			Speaker.SetActive(false);
			float a2 = 1f;
			StartCoroutine(TextHint_2(a2));
		}
	}

	private IEnumerator TextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			Txt_Cut_2.SetActive(true);
			Speaker.SetActive(true);
			float a2 = 4f;
			StartCoroutine(HideTextHint_2(a2));
		}
	}

	private IEnumerator HideTextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		if (_skipEnable)
		{
			Txt_Cut_2.SetActive(false);
			Speaker.SetActive(false);
			float a2 = 2f;
			StartCoroutine(Fade(a2));
			float a3 = 4f;
			StartCoroutine(CutSceneEnd(a3));
		}
	}

	private void Fade()
	{
		if (_skipEnable)
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
		if (_skipEnable)
		{
			_skipEnable = false;
			MainCamera.GetComponent<Animation>().Stop();
			BackGround.SetActive(false);
			Txt_Cut_2.SetActive(false);
			Speaker.SetActive(false);
			UICamera.SetActive(true);
			MainCamera.GetComponent<CameraSmoothFollow>().enabled = true;
			float a2 = 1f;
			StartCoroutine(Skip(a2));
		}
	}
}
