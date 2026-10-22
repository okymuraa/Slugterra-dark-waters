using UnityEngine;

public class SMTransRimShaderIrisator : MonoBehaviour
{
	public float topStr = 2f;

	public float botStr = 1f;

	public float minSpeed = 1f;

	public float maxSpeed = 1f;

	private float speed;

	private float timeGoes;

	private bool timeGoesUp = true;

	private void RandomizeSpeed()
	{
		speed = Random.Range(minSpeed, maxSpeed);
	}

	private void Start()
	{
		timeGoes = botStr;
		speed = Random.Range(minSpeed, maxSpeed);
	}

	private void Update()
	{
		if (timeGoes > topStr)
		{
			timeGoesUp = false;
			RandomizeSpeed();
		}
		if (timeGoes < botStr)
		{
			timeGoesUp = true;
			RandomizeSpeed();
		}
		if (timeGoesUp)
		{
			timeGoes += Time.deltaTime * speed;
		}
		if (!timeGoesUp)
		{
			timeGoes -= Time.deltaTime * speed;
		}
		float value = timeGoes;
		GetComponent<Renderer>().material.SetFloat("_AllPower", value);
	}
}
