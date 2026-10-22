using HedgehogTeam.EasyTouch;
using UnityEngine;

public class LongTapMe : MonoBehaviour
{
	private TextMesh textMesh;

	private Color startColor;

	private void OnEnable()
	{
		EasyTouch.On_LongTapStart += On_LongTapStart;
		EasyTouch.On_LongTap += On_LongTap;
		EasyTouch.On_LongTapEnd += On_LongTapEnd;
	}

	private void OnDisable()
	{
		UnsubscribeEvent();
	}

	private void OnDestroy()
	{
		UnsubscribeEvent();
	}

	private void UnsubscribeEvent()
	{
		EasyTouch.On_LongTapStart -= On_LongTapStart;
		EasyTouch.On_LongTap -= On_LongTap;
		EasyTouch.On_LongTapEnd -= On_LongTapEnd;
	}

	private void Start()
	{
		textMesh = GetComponentInChildren<TextMesh>();
		startColor = gameObject.GetComponent<Renderer>().material.color;
	}

	private void On_LongTapStart(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject)
		{
			RandomColor();
		}
	}

	private void On_LongTap(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject)
		{
			textMesh.text = gesture.actionTime.ToString("f2");
		}
	}

	private void On_LongTapEnd(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject)
		{
			gameObject.GetComponent<Renderer>().material.color = startColor;
			textMesh.text = "Long tap me";
		}
	}

	private void RandomColor()
	{
		gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}
}
