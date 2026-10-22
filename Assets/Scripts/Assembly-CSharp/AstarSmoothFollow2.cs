using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_astar_smooth_follow2.php")]
public class AstarSmoothFollow2 : MonoBehaviour
{
	public Transform target;

	public float distance = 3f;

	public float height = 3f;

	public float damping = 5f;

	public bool smoothRotation = true;

	public bool followBehind = true;

	public float rotationDamping = 10f;

	public bool staticOffset;

	private void LateUpdate()
	{
		Vector3 b;
		if (staticOffset)
		{
			b = target.position + new Vector3(0f, height, distance);
		}
		else
		{
			b = ((!followBehind) ? target.TransformPoint(0f, height, distance) : target.TransformPoint(0f, height, 0f - distance));
		}
		transform.position = Vector3.Lerp(transform.position, b, Time.deltaTime * damping);
		if (smoothRotation)
		{
			Quaternion b2 = Quaternion.LookRotation(target.position - transform.position, target.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, b2, Time.deltaTime * rotationDamping);
		}
		else
		{
			transform.LookAt(target, target.up);
		}
	}
}
