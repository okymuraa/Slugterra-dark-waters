using UnityEngine;

public class SM_destroyThisTimed : MonoBehaviour
{
	public float destroyTime = 5f;

	protected virtual void OnStart()
	{
	}

	private void Start()
	{
		Object.Destroy(gameObject, destroyTime);
		OnStart();
	}
}
