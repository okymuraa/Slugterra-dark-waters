using UnityEngine;

public class SMTransRimShaderInOut : MonoBehaviour
{
	public float str = 1f;

	public float fadeIn = 1f;

	public float stay = 1f;

	public float fadeOut = 1f;

	private float timeGoes;

	private float currStr;

	private void Start()
	{
		GetComponent<Renderer>().material.SetFloat("_AllPower", currStr);
	}

	private void Update()
	{
		timeGoes += Time.deltaTime;
		if (timeGoes < fadeIn)
		{
			currStr = timeGoes * str * (1f / fadeIn);
		}
		if (timeGoes > fadeIn && timeGoes < fadeIn + stay)
		{
			currStr = str;
		}
		if (timeGoes > fadeIn + stay)
		{
			currStr = str - (timeGoes - (fadeIn + stay)) * (1f / fadeOut);
		}
		GetComponent<Renderer>().material.SetFloat("_AllPower", currStr);
	}
}
