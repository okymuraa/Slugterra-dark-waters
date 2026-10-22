using System;
using System.Collections;
using AppsMinistry.UI;
using Holoville.HOTween;
using UnityEngine;

public class CreditsPanelBehaviour : PopupView
{
	[SerializeField]
	private GameObject _scrollObject;

	[SerializeField]
	private float _minHeight = -7.5f;

	[SerializeField]
	private float _maxHeight;

	[SerializeField]
	private float _timeScrolling = 1f;

	public Action OnCloseEvent;

	private float currentHeight;

	protected override void OnEnable()
	{
		base.OnEnable();
		currentHeight = _minHeight;
		StartCoroutine(Scroll());
	}

	private IEnumerator Scroll()
	{
		float currentTime = 0f;
		while (currentTime < _timeScrolling)
		{
			currentTime += Time.deltaTime;
			currentHeight = Mathf.Lerp(_minHeight, _maxHeight, currentTime / _timeScrolling);
			Vector3 pos = _scrollObject.transform.position;
			pos.y = currentHeight;
			_scrollObject.transform.position = pos;
			yield return null;
		}
		ScrollEnd();
	}

	public void TweenAlpha(tk2dSprite target, float time, Color to)
	{
		TweenParms tweenParms = new TweenParms();
		tweenParms.Prop("color", to);
		HOTween.To(target, time, tweenParms);
	}

	private void ScrollEnd()
	{
		currentHeight = _minHeight;
		StartCoroutine(Scroll());
	}

	public override void Close()
	{
		if (OnCloseEvent != null)
		{
			OnCloseEvent();
		}
	}
}
