using System.Collections.Generic;
using Slugterra.Game.Units.Ally;
using UnityEngine;

namespace Slugterra.Game.Environment
{
	public class TerrainConveyor : MonoBehaviour
	{
		private class Terrain
		{
			public GameObject Prefab;

			public bool IsUsed;

			public Terrain(GameObject prefab, bool isUsed)
			{
				Prefab = prefab;
				IsUsed = isUsed;
			}
		}

		private const int TERRAIN_COUNT = 4;

		[SerializeField]
		private GameObject[] _terrainBlock_left;

		[SerializeField]
		private GameObject[] _terrainBlock_right;

		[SerializeField]
		private Transform _eliTransform;

		[SerializeField]
		private float _instantiateOffsetZ;

		[SerializeField]
		private float _rightPosX;

		[SerializeField]
		private float _leftPosX;

		[SerializeField]
		private float _destroyOffsetZ;

		[SerializeField]
		private float _speed;

		private float _terrainFreq;

		private float _prevInstantiateTime;

		private const float TERRAIN_SIZE = 47.6f;

		private List<Terrain> _terrains;

		private List<Terrain> _terrains2;

		private Terrain _lastTerrain;

		private Vector3 _prevEliPos;

		private float _currentSpeed;

		private void LoadTerrainBlock()
		{
		}

		private void Start()
		{
			_terrains = new List<Terrain>();
			_terrains2 = new List<Terrain>();
			if (_eliTransform == null)
			{
				_eliTransform = Object.FindObjectOfType<EliController>().transform;
			}
			LoadTerrainBlock();
			for (int i = 0; i < 4; i++)
			{
				_terrains.Add(new Terrain(_terrainBlock_left[i], false));
				_terrains2.Add(new Terrain(_terrainBlock_right[i], false));
			}
			_terrainFreq = 1f / (47.6f / _speed);
			_prevInstantiateTime = Time.time - 1f / _terrainFreq;
			_prevEliPos = _eliTransform.position;
		}

		private void Update()
		{
			if (_lastTerrain == null || (_lastTerrain.Prefab.transform.position + _lastTerrain.Prefab.transform.forward * 30f).z <= (_eliTransform.position + _lastTerrain.Prefab.transform.forward * _instantiateOffsetZ).z)
			{
				for (int i = 0; i < _terrains.Count; i++)
				{
					if (!_terrains[i].IsUsed)
					{
						_terrains[i].Prefab.transform.position = new Vector3(_eliTransform.position.x + _leftPosX, 0f, _eliTransform.position.z + _instantiateOffsetZ);
						_terrains[i].Prefab.GetComponent<SMMoveThis>().translationSpeed = -1f * Vector3.forward * _speed;
						_terrains[i].IsUsed = true;
						_lastTerrain = _terrains[i];
						break;
					}
					if (i == _terrains.Count - 1)
					{
						_terrains.Add(new Terrain(_terrainBlock_left[0], false));
					}
				}
				for (int j = 0; j < _terrains2.Count; j++)
				{
					if (!_terrains2[j].IsUsed)
					{
						_terrains2[j].Prefab.transform.position = new Vector3(_eliTransform.position.x + _rightPosX, 0f, _eliTransform.position.z + _instantiateOffsetZ);
						_terrains2[j].Prefab.GetComponent<SMMoveThis>().translationSpeed = -1f * Vector3.forward * _speed;
						_terrains2[j].IsUsed = true;
						break;
					}
					if (j == _terrains2.Count - 1)
					{
						_terrains2.Add(new Terrain(_terrainBlock_right[0], false));
					}
				}
				_prevInstantiateTime = Time.time;
			}
			for (int k = 0; k < _terrains.Count; k++)
			{
				if (_terrains[k].IsUsed && _terrains[k].Prefab.transform.position.z < _eliTransform.position.z - _destroyOffsetZ)
				{
					_terrains[k].Prefab.GetComponent<SMMoveThis>().translationSpeed = Vector3.zero;
					_terrains[k].IsUsed = false;
				}
			}
			for (int l = 0; l < _terrains2.Count; l++)
			{
				if (_terrains2[l].IsUsed && _terrains2[l].Prefab.transform.position.z < _eliTransform.position.z - _destroyOffsetZ)
				{
					_terrains2[l].Prefab.GetComponent<SMMoveThis>().translationSpeed = Vector3.zero;
					_terrains2[l].IsUsed = false;
				}
			}
			_prevEliPos = _eliTransform.position;
		}
	}
}
