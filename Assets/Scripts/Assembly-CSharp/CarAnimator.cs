using UnityEngine;

[AddComponentMenu("")]
public class CarAnimator : MonoBehaviour
{
	public Spline spline;

	public WrapMode wrapMode = WrapMode.Once;

	public float speed = 1f;

	public float passedTime;

	public float rotationOffset;

	private void Update()
	{
		passedTime += Time.deltaTime * speed;
		float param = WrapValue(passedTime, 0f, 1f, wrapMode);
		transform.rotation = spline.GetOrientationOnSpline(WrapValue(passedTime + rotationOffset, 0f, 1f, wrapMode));
		transform.position = spline.GetPositionOnSpline(param) - transform.right * spline.GetCustomValueOnSpline(param) * 0.5f;
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
