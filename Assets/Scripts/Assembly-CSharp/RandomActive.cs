using UnityEngine;

public class RandomActive : MonoBehaviour
{
	[Range(0f, 100f)]
	public byte _rollActive = 50;

	public GameObject _object;

	private void Start()
	{
		int num = Random.Range(0, 100);
		if (num >= _rollActive)
		{
			if (_object == null)
			{
				transform.gameObject.SetActive(false);
			}
			else
			{
				_object.gameObject.SetActive(false);
			}
		}
	}
}
