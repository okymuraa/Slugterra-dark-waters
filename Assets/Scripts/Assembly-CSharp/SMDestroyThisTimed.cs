using UnityEngine;

public class SMDestroyThisTimed : MonoBehaviour
{
	public float destroyTime = 5f;

	private void Start()
	{
		Object.Destroy(gameObject, destroyTime);
	}
}
