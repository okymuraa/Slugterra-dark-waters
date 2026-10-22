using Slugterra.Scene.Game;
using UnityEngine;

public class DarkPeriphery_3_1_Messager : MonoBehaviour
{
	public GameObject scriptHolder;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			scriptHolder.GetComponent<DarkPeriphery_3_1>().MessageStart();
		}
	}
}
