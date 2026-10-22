using UnityEngine;

[AddComponentMenu("")]
public class VectorArrayInterpolation : MonoBehaviour
{
	public Vector3[] vectorData;

	public float lineResolution;

	public Transform animatedObject;

	private float parameter;

	private void Update()
	{
		parameter = Mathf.PingPong(Time.realtimeSinceStartup, 1f);
		int num = Mathf.FloorToInt((float)(vectorData.Length - 1) * parameter);
		float num2 = 1f / (float)(vectorData.Length - 1);
		float num3 = (parameter - (float)num * num2) / num2;
		SplineInterpolator splineInterpolator = new HermiteInterpolator();
		Vector3 position = splineInterpolator.InterpolateVector(num3, num, false, vectorData, 0);
		animatedObject.transform.position = position;
	}

	private void OnDrawGizmos()
	{
		SplineInterpolator splineInterpolator = new HermiteInterpolator();
		float num = 1f / lineResolution;
		for (int i = 0; i < vectorData.Length; i++)
		{
			for (float num2 = 0f; num2 <= 1f - num; num2 += num)
			{
				Vector3 vector = splineInterpolator.InterpolateVector(num2, i, false, vectorData, 0);
				Vector3 to = splineInterpolator.InterpolateVector(num2 + num, i, false, vectorData, 0);
				Gizmos.DrawLine(vector, to);
			}
		}
	}
}
