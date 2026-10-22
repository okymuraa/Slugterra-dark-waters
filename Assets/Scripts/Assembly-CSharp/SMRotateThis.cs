using UnityEngine;

public class SMRotateThis : MonoBehaviour
{
	public float rotationSpeedX = 90f;

	public float rotationSpeedY;

	public float rotationSpeedZ;

	public bool local = true;

	private Transform _transform;

	private void Awake()
	{
		_transform = transform;
	}

	private void Update()
	{
		if (local)
		{
			_transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
		}
		else
		{
			_transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime, Space.World);
		}
	}
}
