using System.Collections;
using UnityEngine;

public class SM_destroyThisTimedUseLight : SM_destroyThisTimed
{
	public float weitBeforelowerIntensity = 1f;

	public float lowerIntensityTime = 1f;

	private float Lowertimer;

	private float offset;

	private float startTime;

	public Light FxLight;

	private float maxIntensity;

	protected override void OnStart()
	{
		if (FxLight != null)
		{
			StartCoroutine(LightLower(weitBeforelowerIntensity));
		}
	}

	private IEnumerator LightLower(float t)
	{
		yield return new WaitForSeconds(t);
		maxIntensity = FxLight.intensity;
		startTime = Time.time;
		while (FxLight.intensity > 0f)
		{
			offset = (Time.time - startTime) / lowerIntensityTime;
			FxLight.intensity = Mathf.SmoothStep(maxIntensity, 0f, offset);
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
}
