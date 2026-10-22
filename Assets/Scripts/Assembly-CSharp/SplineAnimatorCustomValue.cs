using UnityEngine;

[AddComponentMenu("")]
[RequireComponent(typeof(MeshRenderer))]
public class SplineAnimatorCustomValue : MonoBehaviour
{
	public Spline spline;

	public WrapMode wrapMode = WrapMode.Once;

	public float speed = 1f;

	public float offSet;

	public float passedTime;

	private void Update()
	{
		passedTime += Time.deltaTime * speed;
		float param = WrapValue(passedTime + offSet, 0f, 1f, wrapMode);
		float customValueOnSpline = spline.GetCustomValueOnSpline(param);
		transform.position = spline.GetPositionOnSpline(param);
		transform.rotation = spline.GetOrientationOnSpline(param);
		GetComponent<Renderer>().material.color = Color.red * (1f - customValueOnSpline) + Color.blue * customValueOnSpline;
	}

	private float WrapValue(float v, float start, float end, WrapMode wMode)
	{
		switch (wMode)
		{
		case WrapMode.Once:
		case WrapMode.ClampForever:
			return Mathf.Clamp(v, start, end);
		case WrapMode.Default:
		case WrapMode.Loop:
			return Mathf.Repeat(v, end - start) + start;
		case WrapMode.PingPong:
			return Mathf.PingPong(v, end - start) + start;
		default:
			return v;
		}
	}
}
