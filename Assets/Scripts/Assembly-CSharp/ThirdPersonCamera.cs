using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	public float distanceAway;

	public float distanceUp;

	public float smooth;

	private GameObject hovercraft;

	private Vector3 targetPosition;

	private Transform follow;

	private void Start()
	{
		follow = GameObject.FindWithTag("Player").transform;
	}

	private void LateUpdate()
	{
		targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
		transform.LookAt(follow);
	}
}
