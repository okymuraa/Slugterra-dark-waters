using UnityEngine;

public class WagonLookAt : MonoBehaviour
{
	public Transform target;

	private void Update()
	{
		transform.LookAt(target);
	}
}
