using UnityEngine;

public class SMMoveThis : MonoBehaviour
{
	public Vector3 translationSpeed = Vector3.forward;

	public bool local = true;

	private void Start()
	{
	}

	private void Update()
	{
		if (local)
		{
			transform.Translate(translationSpeed * Time.deltaTime);
		}
		else
		{
			transform.Translate(translationSpeed * Time.deltaTime, Space.World);
		}
	}
}
