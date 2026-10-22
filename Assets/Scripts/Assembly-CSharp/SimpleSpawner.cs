using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
	public string poolName;

	public Transform testPrefab;

	public int spawnAmount = 50;

	public float spawnInterval = 0.25f;

	public string particlesPoolName;

	public ParticleSystem particleSystemPrefab;

	private void Start()
	{
		StartCoroutine(Spawner());
		if (particlesPoolName != string.Empty)
		{
			StartCoroutine(ParticleSpawner());
		}
	}

	private IEnumerator ParticleSpawner()
	{
		SpawnPool particlesPool = PoolManager.Pools[particlesPoolName];
		ParticleSystem prefab = particleSystemPrefab;
		Vector3 prefabXform = particleSystemPrefab.transform.position;
		Quaternion prefabRot = particleSystemPrefab.transform.rotation;
		while (true)
		{
			ParticleSystem emitter = particlesPool.Spawn(prefab, prefabXform, prefabRot);
			while (emitter.IsAlive(true))
			{
				yield return new WaitForSeconds(1f);
			}
			ParticleSystem inst = particlesPool.Spawn(prefab, prefabXform, prefabRot);
			particlesPool.Despawn(inst.transform, 2f);
			yield return new WaitForSeconds(2f);
		}
	}

	private IEnumerator Spawner()
	{
		int count = spawnAmount;
		SpawnPool shapesPool = PoolManager.Pools[poolName];
		while (count > 0)
		{
			Transform inst = shapesPool.Spawn(testPrefab);
			inst.localPosition = new Vector3(spawnAmount + 2 - count, 0f, 0f);
			count--;
			yield return new WaitForSeconds(spawnInterval);
		}
		StartCoroutine(Despawner());
		yield return null;
	}

	private IEnumerator Despawner()
	{
		SpawnPool shapesPool = PoolManager.Pools[poolName];
		List<Transform> spawnedCopy = new List<Transform>(shapesPool);
		Debug.Log(shapesPool.ToString());
		foreach (Transform instance in spawnedCopy)
		{
			shapesPool.Despawn(instance);
			yield return new WaitForSeconds(spawnInterval);
		}
		StartCoroutine(Spawner());
		yield return null;
	}
}
