using UnityEngine;

public class test_crash : MonoBehaviour
{
	public Transform pointScan;

	private RaycastHit hit;

	public LayerMask Layer;

	private Collider[] colliders;

	public float detectionRadius = 0.5f;

	private int j;

	public int count;

	private float timer;

	private void Start()
	{
	}

	private void Update()
	{
		transform.eulerAngles += Vector3.up * Time.deltaTime * 100f;
		colliders = Physics.OverlapSphere(pointScan.position, detectionRadius);
		count = colliders.Length;
		for (j = 0; j < colliders.Length; j++)
		{
		}
	}

	private void FixedUpdate()
	{
		if (timer < 0f)
		{
			pointScan.localPosition = Vector3.left * Random.Range(-20, 20);
			timer = Random.Range(5, 10);
		}
		timer -= Time.deltaTime;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(pointScan.position, detectionRadius);
	}
}
