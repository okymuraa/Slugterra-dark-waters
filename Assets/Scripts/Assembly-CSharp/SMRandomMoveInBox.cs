using UnityEngine;

public class SMRandomMoveInBox : MonoBehaviour
{
	public float xspeed = 1f;

	public float yspeed = 1.5f;

	public float zspeed = 1f;

	public float speedDeviation;

	public float xDim = 0.3f;

	public float yDim = 0.3f;

	public float zDim = 0.3f;

	private Transform _transform;

	private void Start()
	{
		_transform = transform;
		_transform.localPosition = Vector3.zero;
		xspeed += (float)Random.Range(-1, 1) * speedDeviation;
		yspeed += (float)Random.Range(-1, 1) * speedDeviation;
		zspeed += (float)Random.Range(-1, 1) * speedDeviation;
	}

	private void Update()
	{
		_transform.Translate(new Vector3(xspeed, yspeed, zspeed) * Time.deltaTime);
		if (_transform.localPosition.x > xDim)
		{
			xspeed = 0f - Mathf.Abs(xspeed);
		}
		if (_transform.localPosition.x < 0f - xDim)
		{
			xspeed = Mathf.Abs(xspeed);
		}
		if (_transform.localPosition.y > yDim)
		{
			yspeed = 0f - Mathf.Abs(yspeed);
		}
		if (_transform.localPosition.y < 0f - yDim)
		{
			yspeed = Mathf.Abs(yspeed);
		}
		if (_transform.localPosition.z > zDim)
		{
			zspeed = 0f - Mathf.Abs(zspeed);
		}
		if (_transform.localPosition.z < 0f - zDim)
		{
			zspeed = Mathf.Abs(zspeed);
		}
	}
}
