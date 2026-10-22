using UnityEngine;

public class bounce : MonoBehaviour
{
	public LayerMask Border;

	public float speed = 6f;

	public byte MaxBounceSize = 25;

	public GameObject FX;

	private Transform _transform;

	private RaycastHit hit;

	private Vector3 startPos;

	private Vector3 newPoint;

	private Vector3 _ray;

	private void Start()
	{
		_transform = transform;
		startPos = _transform.position + Vector3.up;
		newPoint = startPos;
		_ray = Quaternion.Euler(startPos.x, Random.Range(0f, 360f), startPos.z) * Vector3.one;
	}

	private void LateUpdate()
	{
		if (Vector3.Distance(_transform.position, newPoint) > 1.1f)
		{
			_transform.position = Vector3.MoveTowards(_transform.position, newPoint, Time.smoothDeltaTime * speed);
			return;
		}
		NextPoint();
		MaxBounceSize--;
	}

	private void NextPoint()
	{
		if (MaxBounceSize <= 0)
		{
			if ((bool)FX)
			{
				Object.Instantiate(FX, _transform.position, Quaternion.identity);
			}
			_transform.gameObject.SetActive(false);
		}
		if (Physics.Raycast(_transform.position, _ray, out hit, 100f, Border))
		{
			_transform.LookAt(hit.point);
			newPoint = hit.point;
			_ray = Vector3.Reflect(_ray, hit.normal);
			_ray.y = 0f;
		}
		else
		{
			if ((bool)FX)
			{
				Object.Instantiate(FX, _transform.position, Quaternion.identity);
			}
			_transform.gameObject.SetActive(false);
		}
	}
}
