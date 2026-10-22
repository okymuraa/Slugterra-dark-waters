using HedgehogTeam.EasyTouch;
using UnityEngine;

public class TouchMe : MonoBehaviour
{
	private TextMesh textMesh;

	private Color startColor;

	private void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
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
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}

	private void Start()
	{
		textMesh = GetComponentInChildren<TextMesh>();
		startColor = gameObject.GetComponent<Renderer>().material.color;
	}

	private void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject)
		{
			RandomColor();
		}
	}

	private void On_TouchDown(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject)
		{
			textMesh.text = "Down since :" + gesture.actionTime.ToString("f2");
		}
	}

	private void On_TouchUp(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject)
		{
			gameObject.GetComponent<Renderer>().material.color = startColor;
			textMesh.text = "Touch me";
		}
	}

	private void RandomColor()
	{
		gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}
}
