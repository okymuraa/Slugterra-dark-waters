using PathologicalGames;
using UnityEngine;

public class OnSpawnedExample : MonoBehaviour
{
	private void OnSpawned(SpawnPool pool)
	{
		Debug.Log(string.Format("OnSpawnedExample | OnSpawned running for '{0}' in pool '{1}'.", name, pool.poolName));
	}

	private void OnDespawned(SpawnPool pool)
	{
		Debug.Log(string.Format("OnSpawnedExample | OnDespawned unning for '{0}' in pool '{1}'.", name, pool.poolName));
	}
}
