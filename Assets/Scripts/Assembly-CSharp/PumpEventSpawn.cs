using UnityEngine;

public class PumpEventSpawn : MonoBehaviour
{
	public GameObject[] Fx;

	private int i;

	private void Start()
	{
		for (i = 0; i < Fx.Length; i++)
		{
			Fx[i].SetActive(false);
		}
		i = 0;
	}

	private void Spawn()
	{
		Fx[i].SetActive(true);
		i++;
		if (i == Fx.Length)
		{
			i = 0;
		}
	}
}
