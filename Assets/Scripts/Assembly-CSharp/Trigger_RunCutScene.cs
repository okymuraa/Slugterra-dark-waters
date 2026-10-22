using UnityEngine;

public class Trigger_RunCutScene : MonoBehaviour
{
	[SerializeField]
	public bool b_Once;

	public float _TimeSwitch = 2f;

	public string[] _CutSceneTextString;

	public GameObject _CameraAnimationObject;

	public GameObject _MainCamera;

	public GameObject _CutSceneDialog;

	public GameObject _CutScTextObject;

	public GameObject _CutSceneCamera;

	public GameObject _HUDCamera;

	private Camera _CamMainCamera;

	private Camera _CamHUDCamera;

	private Camera _CamCutSceneCamera;

	private TextMesh _CutScText;

	private float _TimerLocal = 5f;

	private bool b_StartSequence;

	private int _Textindex;

	private void Start()
	{
		if (_CameraAnimationObject != null)
		{
			_CameraAnimationObject.SetActive(false);
		}
		_TimerLocal = _TimeSwitch;
		_CamMainCamera = _MainCamera.GetComponent<Camera>();
		_CamHUDCamera = _HUDCamera.GetComponent<Camera>();
		_CamCutSceneCamera = _CutSceneCamera.GetComponent<Camera>();
		_CutSceneDialog.SetActive(false);
		_CutScTextObject.SetActive(false);
		_CutScText = _CutScTextObject.GetComponent<TextMesh>();
		_CutScText.text = _CutSceneTextString[_Textindex];
	}

	private void Update()
	{
		if (b_StartSequence)
		{
			_TimerLocal -= Time.deltaTime;
			if (_TimerLocal < 0f)
			{
				SwitchText();
			}
		}
	}

	private void OnTriggerEnter(Collider _Collider)
	{
		if (_Collider.tag == "Player")
		{
			CutSceneMode(true);
			StartCameraSequence();
		}
	}

	private void StartCameraSequence()
	{
		if (_CameraAnimationObject != null)
		{
			_CameraAnimationObject.SetActive(true);
		}
	}

	private void SwitchText()
	{
		if (_CutSceneTextString.Length > 0 && _Textindex < _CutSceneTextString.Length)
		{
			_TimerLocal = _TimeSwitch;
			_CutScText.text = _CutSceneTextString[_Textindex];
			_Textindex++;
			return;
		}
		CutSceneMode(false);
		if (b_Once)
		{
			gameObject.SetActive(false);
		}
	}

	private void CutSceneMode(bool _inBool)
	{
		if (_inBool)
		{
			b_StartSequence = _inBool;
			_CamHUDCamera.enabled = false;
			_CamCutSceneCamera.enabled = true;
			_CutSceneDialog.SetActive(true);
			_CutScTextObject.SetActive(true);
		}
		else
		{
			b_StartSequence = _inBool;
			_CamCutSceneCamera.enabled = false;
			_CamHUDCamera.enabled = true;
			_CamMainCamera.enabled = true;
			_CutSceneDialog.SetActive(false);
			_CutScTextObject.SetActive(false);
		}
	}
}
