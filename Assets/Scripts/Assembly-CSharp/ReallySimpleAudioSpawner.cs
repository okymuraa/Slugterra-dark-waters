using System.Collections;
using PathologicalGames;
using UnityEngine;

public class ReallySimpleAudioSpawner : MonoBehaviour
{
	public AudioSource prefab;

	public float spawnInterval = 2f;

	private SpawnPool pool;

	private void Start()
	{
		pool = GetComponent<SpawnPool>();
		StartCoroutine(Spawner());
	}

	private IEnumerator Spawner()
	{
		while (true)
		{
			AudioSource current = pool.Spawn(prefab, transform.position, transform.rotation);
			current.pitch = Random.Range(0.7f, 1.4f);
			yield return new WaitForSeconds(spawnInterval);
		}
	}
}
