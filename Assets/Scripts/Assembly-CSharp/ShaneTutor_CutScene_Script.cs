using AppsMinistry.UI.Camera;
using UnityEngine;

public class ShaneTutor_CutScene_Script : MonoBehaviour
{
	public GameObject MainCamera;

	public GameObject Cameras_UI;

	public GameObject Cutscene_Camera;

	public GameObject House_Round_Door_Door_L;

	public GameObject House_Round_Door_Door_R;

	private void Start()
	{
		MainCamera.SetActive(false);
		Cameras_UI.SetActive(false);
		Cutscene_Camera.SetActive(true);
		Cutscene_Camera.GetComponent<CameraFade>().StartFade(Color.black, 0.1f, 1f, 1f);
	}

	private void DoorsOpen()
	{
		House_Round_Door_Door_L.GetComponent<Animation>().Play();
		House_Round_Door_Door_R.GetComponent<Animation>().Play();
	}

	private void Update()
	{
	}
}
