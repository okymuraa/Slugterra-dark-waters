using Slugterra.Game.Environment;
using UnityEngine;

public class HeightDestroy : MonoBehaviour
{
	public LayerMask layer;

	private RaycastHit hit;

	private float _height;

	public float speed = 6f;

	private ArenaInteractiveEnvironmant target;

	private Vector3 targetHeight;

	private void Start()
	{
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10f, layer))
		{
			_height = hit.point.y;
		}
		else
		{
			_height = transform.position.y;
		}
		targetHeight = transform.position;
		targetHeight.y = _height;
		if (Vector3.SqrMagnitude(targetHeight - transform.position) < 2f)
		{
			targetHeight.y = transform.position.y - 6.5f;
		}
		target = transform.GetComponent<ArenaInteractiveEnvironmant>();
	}

	private void LateUpdate()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetHeight, Time.smoothDeltaTime * speed);
		if (Vector3.SqrMagnitude(transform.position - targetHeight) < 0.01f)
		{
			target.OnDamage(10f);
		}
	}
}
