using UnityEngine;

public class SMTransRimShaderFader : MonoBehaviour
{
	public float startStr = 2f;

	public float speed = 3f;

	private float timeGoes;

	private float currStr;

	private void Update()
	{
		timeGoes += Time.deltaTime * speed * startStr;
		currStr = startStr - timeGoes;
		GetComponent<Renderer>().material.SetFloat("_AllPower", currStr);
	}
}
