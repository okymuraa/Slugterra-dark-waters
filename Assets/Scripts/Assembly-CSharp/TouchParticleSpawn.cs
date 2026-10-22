using UnityEngine;

public class TouchParticleSpawn : MonoBehaviour
{
	public GameObject Ring;

	public GameObject[] Pool;

	public LayerMask LayerUI;

	private RaycastHit hit;

	private int i;

	private void Awake()
	{
		Pool = new GameObject[10];
		for (i = 0; i < Pool.Length; i++)
		{
			Pool[i] = Object.Instantiate(Ring, Vector3.zero, Quaternion.identity);
			Pool[i].SetActive(false);
		}
	}

	private void Update()
	{
		if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began || !Physics.Raycast(transform.parent.GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position), out hit, 50f, LayerUI))
		{
			return;
		}
		for (i = 0; i < Pool.Length; i++)
		{
			if (!Pool[i].activeSelf)
			{
				Pool[i].transform.position = hit.point;
				Pool[i].SetActive(true);
				break;
			}
		}
	}
}
