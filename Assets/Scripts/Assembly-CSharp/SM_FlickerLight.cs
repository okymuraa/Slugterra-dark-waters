using UnityEngine;

[RequireComponent(typeof(Light))]
public class SM_FlickerLight : MonoBehaviour
{
	private Light _light;

	public AnimationCurve curve;

	public float manualDuration = 3f;

	private void Start()
	{
		_light = GetComponent<Light>();
	}

	private void Update()
	{
		_light.intensity = curve.Evaluate(Mathf.Repeat(Time.time, manualDuration));
	}
}
