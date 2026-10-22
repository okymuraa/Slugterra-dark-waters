using UnityEngine;

[AddComponentMenu("SuperSplines/Animation/Gravity Animator")]
public class SplineGravitySimulator : MonoBehaviour
{
	public Spline spline;

	public float gravityConstant = 9.81f;

	public int iterations = 5;

	private void Start()
	{
		GetComponent<Rigidbody>().useGravity = false;
	}

	private void FixedUpdate()
	{
		if (!(GetComponent<Rigidbody>() == null) && !(spline == null))
		{
			Vector3 positionOnSpline = spline.GetPositionOnSpline(spline.GetClosestPointParam(GetComponent<Rigidbody>().position, iterations));
			Vector3 vector = positionOnSpline - GetComponent<Rigidbody>().position;
			Vector3 force = vector * Mathf.Pow(vector.magnitude, -3f) * gravityConstant * GetComponent<Rigidbody>().mass;
			GetComponent<Rigidbody>().AddForce(force);
		}
	}
}
