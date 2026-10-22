using System.Collections;
using UnityEngine;

public class TriggerLootBox : MonoBehaviour
{
	public GameObject[] _EnemyList;

	public GameObject[] _SpawnObjects;

	public GameObject _SpawnFX;

	public float _SpawnDelay = 1f;

	public float _CheckTime = 1f;

	private Transform _MyTransform;

	public Animation _Animation;

	public Vector2 _RandomLootSpawnPosRange;

	private void Start()
	{
		StartCoroutine(CheckEnemy());
		_MyTransform = transform;
	}

	private IEnumerator CheckEnemy()
	{
		yield return new WaitForSeconds(_CheckTime);
		bool b_ListEmpty = true;
		for (int i = 0; i < _EnemyList.Length; i++)
		{
			if (_EnemyList[i] != null)
			{
				b_ListEmpty = false;
			}
		}
		if (b_ListEmpty)
		{
			_Animation.Play();
			yield return new WaitForSeconds(2f);
			for (int j = 0; j < _SpawnObjects.Length; j++)
			{
				Vector3 _RandomPosition = new Vector3(_MyTransform.position.x + Random.Range(0f - _RandomLootSpawnPosRange.x, _RandomLootSpawnPosRange.x), _MyTransform.position.y, _MyTransform.position.z + Random.Range(0f - _RandomLootSpawnPosRange.y, _RandomLootSpawnPosRange.y));
				if (_SpawnFX != null)
				{
					Object.Instantiate(_SpawnFX, _RandomPosition, _MyTransform.rotation);
				}
				Object.Instantiate(_SpawnObjects[j], _RandomPosition, _MyTransform.rotation);
				yield return new WaitForSeconds(_SpawnDelay);
			}
		}
		else
		{
			StartCoroutine(CheckEnemy());
		}
	}
}
