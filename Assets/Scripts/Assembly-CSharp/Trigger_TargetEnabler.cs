using UnityEngine;

public class Trigger_TargetEnabler : MonoBehaviour
{
	public enum TriggerAction
	{
		EnableTarget = 0,
		DisableTarget = 1
	}

	[SerializeField]
	private GameObject[] _TargetObject;

	public bool b_OnEnter;

	public bool b_OnEnterOnce;

	public TriggerAction _TriggerEnterSelector;

	public bool b_OnExit;

	public bool b_OnExitOnce;

	public TriggerAction _TriggerExitSelector;

	private void OnTriggerEnter(Collider _Collider)
	{
		if (_Collider.tag == "Player" && _TargetObject.Length > 0 && b_OnEnter)
		{
			if (_TriggerEnterSelector == TriggerAction.EnableTarget)
			{
				_SwitchTargetState(true);
			}
			else
			{
				_SwitchTargetState(false);
			}
		}
		if (b_OnEnterOnce)
		{
			b_OnEnter = false;
		}
	}

	private void OnTriggerExit(Collider _Collider)
	{
		if (_Collider.tag == "Player" && _TargetObject.Length > 0 && b_OnExit)
		{
			if (_TriggerExitSelector == TriggerAction.EnableTarget)
			{
				_SwitchTargetState(true);
			}
			else
			{
				_SwitchTargetState(false);
			}
		}
		if (b_OnExitOnce)
		{
			b_OnExit = false;
		}
	}

	private void _SwitchTargetState(bool b_InState)
	{
		GameObject[] targetObject = _TargetObject;
		foreach (GameObject gameObject in targetObject)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(b_InState);
			}
		}
	}
}
