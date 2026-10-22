using UnityEngine;
using UnityEngine.UI;

public class Multiplayer_Tutor : MonoBehaviour
{
	public Image Shadow;

	public GameObject TutorElements;

	private Color shadow;

	private int _complete;

	private bool _lock;

	public void Start()
	{
		_complete = PlayerPrefs.GetInt("Tutor_MultiplayerComplete");
		if (_complete >= 2)
		{
			Shadow.gameObject.SetActive(false);
			TutorElements.SetActive(false);
			Object.Destroy(this);
		}
		else
		{
			shadow = Color.black;
			shadow.a = 0f;
			Shadow.color = shadow;
			Shadow.gameObject.SetActive(true);
			TutorElements.SetActive(true);
		}
	}

	private void Complete()
	{
		Shadow.gameObject.SetActive(false);
		TutorElements.SetActive(false);
		_complete++;
		PlayerPrefs.SetInt("Tutor_MultiplayerComplete", _complete);
		_lock = true;
	}

	private void Update()
	{
		if (!_lock && Input.GetMouseButtonDown(0))
		{
			Complete();
		}
	}

	private void LateUpdate()
	{
		if (!_lock && shadow.a < 0.628f)
		{
			shadow.a += 0.0314f;
			Shadow.color = shadow;
		}
	}
}
