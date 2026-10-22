using UnityEngine;

public class SMRandomRotation : MonoBehaviour
{
	private float rotationMaxX;

	private float rotationMaxY = 360f;

	private float rotationMaxZ;

	private void Start()
	{
		float xAngle = Random.Range(0f - rotationMaxX, rotationMaxX);
		float yAngle = Random.Range(0f - rotationMaxY, rotationMaxY);
		float zAngle = Random.Range(0f - rotationMaxZ, rotationMaxZ);
		transform.Rotate(xAngle, yAngle, zAngle);
	}
}
