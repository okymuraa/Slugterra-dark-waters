using System.Collections;
using UnityEngine;

public class FS_RandomWander : MonoBehaviour
{
	public float speed = 0.1f;

	public float directionChangeInterval = 1f;

	public float maxHeadingChange = 30f;

	public float dist;

	private float heading;

	private Vector3 targetRotation;

	private void Awake()
	{
		heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0f, heading, 0f);
		StartCoroutine(NewHeading());
	}

	private void FixedUpdate()
	{
		transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
		Vector3 vector = transform.TransformDirection(Vector3.forward);
		transform.Translate(vector * speed);
		if (transform.position.y < 1f)
		{
			NewHeadingRoutine();
		}
		dist = transform.position.magnitude;
		if (transform.position.magnitude > 70f)
		{
			Object.Destroy(gameObject);
		}
		Vector3 position = transform.position;
		position.y = 10f;
		transform.position = position;
	}

	private IEnumerator NewHeading()
	{
		while (true)
		{
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	private void NewHeadingRoutine()
	{
		float min = Mathf.Clamp(heading - maxHeadingChange, 0f, 360f);
		float max = Mathf.Clamp(heading + maxHeadingChange, 0f, 360f);
		heading = Random.Range(min, max);
		targetRotation = new Vector3(0f, heading, 0f);
	}
}
