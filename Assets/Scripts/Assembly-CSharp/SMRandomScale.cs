using UnityEngine;

public class SMRandomScale : MonoBehaviour
{
	public float minScale = 1f;

	public float maxScale = 2f;

	private void Start()
	{
		float num = Random.Range(minScale, maxScale);
		transform.localScale = new Vector3(num, num, num);
	}
}
