using UnityEngine;

[AddComponentMenu("SuperSplines/Animation/Regular Animator")]
public class SplineAnimator : MonoBehaviour
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
		transform.position = spline.GetPositionOnSpline(param);
		transform.rotation = spline.GetOrientationOnSpline(param);
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
