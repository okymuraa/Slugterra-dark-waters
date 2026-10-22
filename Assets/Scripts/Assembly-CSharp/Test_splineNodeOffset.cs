using UnityEngine;

public class Test_splineNodeOffset : MonoBehaviour
{
	public Transform[] point;

	private int i;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			for (i = 0; i < point.Length; i++)
			{
				point[i].localPosition += Vector3.forward * Time.deltaTime * 5f;
			}
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			for (i = 0; i < point.Length; i++)
			{
				point[i].localPosition -= Vector3.forward * Time.deltaTime * 5f;
			}
		}
	}
}
