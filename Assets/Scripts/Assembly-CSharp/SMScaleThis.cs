using UnityEngine;

public class SMScaleThis : MonoBehaviour
{
	public float scaleTime = 1f;

	public float minScale = 0.1f;

	public float maxScale = 1f;

	private float _velocity;

	private void Start()
	{
		transform.localScale = new Vector3(minScale, minScale, minScale);
	}

	private void Update()
	{
		float num = Mathf.SmoothDamp(transform.localScale.x, maxScale, ref _velocity, scaleTime);
		transform.localScale = new Vector3(num, num, num);
	}
}
