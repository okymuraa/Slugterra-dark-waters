using Slugterra.Scene.Game;
using UnityEngine;

public class Deadweed_4_1_Trigger : MonoBehaviour
{
	public enum Action
	{
		enable = 0,
		disable = 1
	}

	[SerializeField]
	private Deadweed_4_1 deadweed_4_1;

	public Action actionGhosts;

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.tag != "Player"))
		{
			if (actionGhosts.ToString() == "enable")
			{
				deadweed_4_1.EnableMobs();
			}
			if (actionGhosts.ToString() == "disable")
			{
				deadweed_4_1.DisableMobs();
			}
		}
	}
}
