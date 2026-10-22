using UnityEngine;

public class ItemShowAmountEvent : MonoBehaviour
{
	public TextMesh _TextMesh;

	private Transform _transform;

	public float _ExistTime = 1f;

	private void Start()
	{
		Vector3 position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		_transform = gameObject.transform;
		Quaternion rotation = Quaternion.LookRotation(position);
		_transform.rotation = rotation;
	}

	private void Update()
	{
		_ExistTime -= Time.deltaTime;
		if (_ExistTime < 0f)
		{
			_TextMesh.text = string.Empty;
			Object.Destroy(gameObject);
		}
	}
}
