using UnityEngine;

public class Trigger_RunAnimAction : MonoBehaviour
{
	public enum TriggerMode
	{
		OnEnter = 0,
		OnExit = 1,
		OnStay = 2
	}

	public bool b_Once;

	public float _StartTimer = 1f;

	public TriggerMode _TriggerOnEventSelector;

	public Animation[] _TargetObjectAnimationComponent;

	public AnimationClip[] _EventAnimation;

	private float _StartTimerLocal;

	private bool b_StartSequence;

	private void Start()
	{
		_StartTimerLocal = _StartTimer;
	}

	private void Update()
	{
		if (b_StartSequence)
		{
			_StartTimerLocal -= Time.deltaTime;
			if (_StartTimerLocal < 0f)
			{
				Action();
				OnceModeCheck();
				_StartTimerLocal = _StartTimer;
				b_StartSequence = false;
			}
		}
	}

	private void OnTriggerEnter(Collider _OtherCollider)
	{
		if (_OtherCollider.tag == "Player" && _TargetObjectAnimationComponent.Length > 0 && _TriggerOnEventSelector == TriggerMode.OnEnter)
		{
			b_StartSequence = true;
		}
	}

	private void OnTriggerExit(Collider _OtherCollider)
	{
		if (_OtherCollider.tag == "Player" && _TargetObjectAnimationComponent.Length > 0 && _TriggerOnEventSelector == TriggerMode.OnExit)
		{
			b_StartSequence = true;
		}
	}

	private void OnTriggerStay(Collider _OtherCollider)
	{
		if (_OtherCollider.tag == "Player" && _TargetObjectAnimationComponent.Length > 0 && _TriggerOnEventSelector == TriggerMode.OnStay)
		{
			b_StartSequence = true;
		}
	}

	private void OnceModeCheck()
	{
		if (b_Once)
		{
			gameObject.SetActive(false);
		}
	}

	private void Action()
	{
		for (int i = 0; i < _TargetObjectAnimationComponent.Length; i++)
		{
			if (_TargetObjectAnimationComponent[i] == null || _EventAnimation[i] == null)
			{
				Debug.Log("-----!!! Some errors on:+" + gameObject.name);
				Debug.Log("-----!!! Null _TargetObjectAnimationComponent on :" + i + " OR null _EventAnimation on:" + i);
			}
			else
			{
				_TargetObjectAnimationComponent[i].clip = _EventAnimation[i];
				_TargetObjectAnimationComponent[i].Play();
			}
		}
	}
}
