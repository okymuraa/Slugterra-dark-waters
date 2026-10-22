using UnityEngine;

public class RandomRotationY : MonoBehaviour
{
	public LayerMask Border;

	public float distanseFire = 5f;

	private Transform _transform;

	private RaycastHit hit;

	private Vector3 _ray;

	public float timeWeit;

	private float timer;

	private void Start()
	{
		_transform = transform;
		timer = timeWeit;
	}

	private void FixedUpdate()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			_ray = Quaternion.Euler(_transform.position.x, Random.Range(0f, 360f), _transform.position.z) * Vector3.one;
			_ray.y = 1f;
			if (!Physics.Raycast(_transform.position, _ray - Vector3.up, out hit, distanseFire, Border))
			{
				_transform.up = _ray - Vector3.up;
				timer = timeWeit;
			}
		}
	}
}
