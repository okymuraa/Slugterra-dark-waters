using UnityEngine;

public class Trigger_CollisionEvent : MonoBehaviour
{
	public int v_CutSceneID;

	public bool b_Once;

	public CutSceneEventManager _EventManager;

	private void Start()
	{
		if (_EventManager == null)
		{
			_EventManager = Object.FindObjectOfType<CutSceneEventManager>();
		}
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider _ColliderOther)
	{
		OnceModeCheck();
	}

	private void OnTriggerExit(Collider _ColliderOther)
	{
		OnceModeCheck();
	}

	private void OnTriggerStay(Collider _ColliderOther)
	{
		OnceModeCheck();
	}

	private void OnceModeCheck()
	{
		if (b_Once)
		{
			gameObject.SetActive(false);
		}
	}
}
