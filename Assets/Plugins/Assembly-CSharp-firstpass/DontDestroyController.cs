using UnityEngine;

public class DontDestroyController : MonoBehaviour
{
	[SerializeField]
	private GameObject[] dontDestroyOnLoadObjects;

	[SerializeField]
	private Shader[] preloadShaders;

	private void Start()
	{
		Object.DontDestroyOnLoad(gameObject);
	}
}
