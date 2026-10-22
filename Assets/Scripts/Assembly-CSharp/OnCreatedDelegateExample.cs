using PathologicalGames;
using UnityEngine;

public class OnCreatedDelegateExample : MonoBehaviour
{
	private void Awake()
	{
		PoolManager.Pools.AddOnCreatedDelegate("Audio", RunMe);
	}

	public void RunMe(SpawnPool pool)
	{
		Debug.Log("Delegate ran for pool " + pool.poolName);
	}
}
