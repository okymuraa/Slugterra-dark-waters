using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI.Camera;
using Slugterra.Game.Units.Enemy;
using Slugterra.Scene;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class CutScene_3_1_0 : SlugterraCutScene
{
	[SerializeField]
	private GameController gameController;

	[SerializeField]
	private GameObject startBlackPlane;

	public float _time_StartCameraTrack = 1f;

	public float _time_textHint_0 = 3f;

	public float _time_HideTextHint_0 = 3f;

	public float _time_textHint_1 = 3f;

	public float _time_HideTextHint_1 = 3f;

	public float _time_textHint_2 = 3f;

	public float _time_HideTextHint_2 = 3f;

	public float _time_DelaySkip = 1f;

	[SerializeField]
	private ScrapHeapController Mob_1;

	[SerializeField]
	private ScrapHeapController AllyMob_1;

	[SerializeField]
	private ScrapHeapController Mob_2;

	[SerializeField]
	private ScrapHeapController AllyMob_2;

	[SerializeField]
	private ScrapHeapController Mob_3;

	[SerializeField]
	private ScrapHeapController AllyMob_3;

	[SerializeField]
	private ScrapHeapController Mob_4;

	[SerializeField]
	private ScrapHeapController AllyMob_4;

	[SerializeField]
	private ScrapHeapController Mob_5;

	[SerializeField]
	private ScrapHeapController AllyMob_5;

	[SerializeField]
	private ScrapHeapController Mob_6;

	[SerializeField]
	private ScrapHeapController AllyMob_6;

	private GameObject MainCamera;

	private GameObject UICamera;

	private CameraFade _cameraFade;

	private CameraSmoothFollow _cameraSmoothFollow;

	private Animation _camerAnimation;

	public float _time_enabled_AB2;

	public float _time_disabled_AB2;

	public float _time_enabled_AB1;

	public float _time_disabled_AB1;

	public GameObject desable_AB1;

	public GameObject desable_AB2;

	private void Start()
	{
		RisePlayCutSceneEvent(true);
		MainCamera = GameObject.Find("MainCamera");
		UICamera = GameObject.Find("UICamera");
		_camerAnimation = MainCamera.GetComponent<Animation>();
		_cameraFade = MainCamera.GetComponent<CameraFade>();
		_cameraSmoothFollow = MainCamera.GetComponent<CameraSmoothFollow>();
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 2f, 0.1f, 2f);
		_cutSceneView.SkipButton.targetObject = gameObject;
		gameController.PlayCutScene(true);
		DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		StartCoroutine(TextHint_0(_time_textHint_0));
		StartCoroutine(StartCameraTrack(_time_StartCameraTrack));
		StartCoroutine(Enable_AB2(_time_enabled_AB2));
		StartCoroutine(Enable_AB1(_time_enabled_AB1));
	}

	private IEnumerator StartCameraTrack(float a)
	{
		yield return new WaitForSeconds(a);
		UICamera.SetActive(false);
		_cameraSmoothFollow.enabled = false;
		_camerAnimation.Play("CameraTrack_1");
		startBlackPlane.SetActive(false);
	}

	private IEnumerator DelaySkip(float a)
	{
		yield return new WaitForSeconds(a);
		Skip();
	}

	private void Skip()
	{
		_camerAnimation.Stop();
		_cameraSmoothFollow.enabled = true;
		UICamera.SetActive(true);
		ShowScreens(false);
		DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
		gameController.PlayCutScene(false);
		RisePlayCutSceneEvent(false);
		SwitchOffMobs_AB2();
		SwitchOffMobs_AB1();
		gameObject.SetActive(false);
	}

	private IEnumerator TextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		ShowScreens(true);
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_1_cutscene_1_text_01"];
		StartCoroutine(HideTextHint_0(_time_HideTextHint_0));
		_isSkipEnable = true;
		_cutSceneView.SkipButton.gameObject.SetActive(true);
	}

	private IEnumerator HideTextHint_0(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			StartCoroutine(TextHint_1(_time_textHint_1));
			SwitchOffMobs_AB2();
		}
	}

	private IEnumerator TextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_1_cutscene_1_text_02"];
			StartCoroutine(HideTextHint_1(_time_HideTextHint_1));
		}
	}

	private IEnumerator HideTextHint_1(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			StartCoroutine(TextHint_2(_time_textHint_2));
		}
	}

	private IEnumerator TextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance["3_1_cutscene_1_text_03"];
			StartCoroutine(HideTextHint_2(_time_HideTextHint_2));
		}
	}

	private IEnumerator HideTextHint_2(float a)
	{
		yield return new WaitForSeconds(a);
		if (_isSkipEnable)
		{
			ShowScreens(false);
			Fade();
		}
	}

	private void Fade()
	{
		_isSkipEnable = false;
		_cameraFade.StartFade(new Color(0f, 0f, 0f, 1f), 1f, 1f, 1f);
		_cutSceneView.SkipButton.gameObject.SetActive(false);
		_cutSceneView.SkipButton.targetObject = null;
		StartCoroutine(DelaySkip(_time_DelaySkip));
	}

	private IEnumerator Enable_AB2(float a)
	{
		yield return new WaitForSeconds(a);
		SettAttackAB2();
		StartCoroutine(Disable_AB2(_time_disabled_AB2));
	}

	private IEnumerator Disable_AB2(float a)
	{
		yield return new WaitForSeconds(a);
		SwitchOffMobs_AB2();
	}

	private IEnumerator Enable_AB1(float a)
	{
		yield return new WaitForSeconds(a);
		desable_AB1.SetActive(true);
		SettAttackAB1();
		StartCoroutine(Disable_AB1(_time_disabled_AB1));
	}

	private IEnumerator Disable_AB1(float a)
	{
		yield return new WaitForSeconds(a);
		SwitchOffMobs_AB1();
	}

	private void SettAttackAB1()
	{
		if (Mob_1 != null)
		{
			Mob_1.Model.MaxHealth = 400f;
			Mob_1.Model.Health = 400f;
		}
		if (AllyMob_1 != null)
		{
			AllyMob_1.Model.MaxHealth = 400f;
			AllyMob_1.Model.Health = 400f;
		}
		if (Mob_2 != null)
		{
			Mob_2.Model.MaxHealth = 400f;
			Mob_2.Model.Health = 400f;
		}
		if (AllyMob_2 != null)
		{
			AllyMob_2.Model.MaxHealth = 400f;
			AllyMob_2.Model.Health = 400f;
		}
		if (Mob_1 != null && AllyMob_1 != null)
		{
			Mob_1.SetAttackTarget(AllyMob_1.Transform);
			AllyMob_1.SetAttackTarget(Mob_1.Transform);
		}
		if (Mob_2 != null && AllyMob_2 != null)
		{
			Mob_2.SetAttackTarget(AllyMob_2.Transform);
			AllyMob_2.SetAttackTarget(Mob_2.Transform);
		}
	}

	private void SettAttackAB2()
	{
		if (Mob_3 != null)
		{
			Mob_3.Model.MaxHealth = 400f;
			Mob_3.Model.Health = 400f;
		}
		if (AllyMob_3 != null)
		{
			AllyMob_3.Model.MaxHealth = 400f;
			AllyMob_3.Model.Health = 400f;
		}
		if (Mob_4 != null)
		{
			Mob_4.Model.MaxHealth = 400f;
			Mob_4.Model.Health = 400f;
		}
		if (AllyMob_4 != null)
		{
			AllyMob_4.Model.MaxHealth = 400f;
			AllyMob_4.Model.Health = 400f;
		}
		if (Mob_3 != null && AllyMob_3 != null)
		{
			Mob_3.SetAttackTarget(AllyMob_3.Transform);
			AllyMob_3.SetAttackTarget(Mob_3.Transform);
		}
		if (Mob_4 != null && AllyMob_4 != null)
		{
			Mob_4.SetAttackTarget(AllyMob_4.Transform);
			AllyMob_4.SetAttackTarget(Mob_4.Transform);
		}
	}

	private void SettAttackAB3()
	{
		if (Mob_5 != null && AllyMob_5 != null)
		{
			Mob_5.SetAttackTarget(AllyMob_5.Transform);
			AllyMob_5.SetAttackTarget(Mob_5.Transform);
		}
		if (Mob_6 != null && AllyMob_6 != null)
		{
			Mob_6.SetAttackTarget(AllyMob_6.Transform);
			AllyMob_6.SetAttackTarget(Mob_6.Transform);
		}
	}

	private void SwitchOffMobs_AB2()
	{
		desable_AB2.SetActive(false);
	}

	private void SwitchOffMobs_AB1()
	{
		desable_AB1.SetActive(false);
	}
}
