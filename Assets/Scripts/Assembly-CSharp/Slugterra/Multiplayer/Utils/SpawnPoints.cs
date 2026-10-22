using System.Collections.Generic;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Multiplayer.Utils
{
	public class SpawnPoints : MonoBehaviour
	{
		[SerializeField]
		private Transform[] _spawnPoints;

		private int curPoint = -1;

		private Transform _cachedTransform;

		public Transform[] CurSpawnPoints
		{
			get
			{
				return _spawnPoints;
			}
			set
			{
				_spawnPoints = value;
			}
		}

		public Transform Transform
		{
			get
			{
				return _cachedTransform;
			}
		}

		private void Start()
		{
			_cachedTransform = transform;
		}

		public Transform GetNextSpawnPoint()
		{
			curPoint++;
			if (curPoint > _spawnPoints.Length - 1)
			{
				curPoint = 0;
			}
			return _spawnPoints[curPoint];
		}

		public Transform GetNextSpawnPoint(Vector3 position)
		{
			float num = float.MaxValue;
			for (int i = 0; i < _spawnPoints.Length; i++)
			{
				float magnitude = (_spawnPoints[i].transform.position - position).magnitude;
				if (magnitude < num)
				{
					num = magnitude;
					curPoint = i;
				}
			}
			return _spawnPoints[curPoint];
		}

		public Transform GetNextSpawnPoint(Vector3 position, float radius)
		{
			List<int> list = new List<int>();
			float num = float.MaxValue;
			for (int i = 0; i < _spawnPoints.Length; i++)
			{
				float magnitude = (_spawnPoints[i].transform.position - position).magnitude;
				if (magnitude <= radius)
				{
					list.Add(i);
				}
				if (magnitude < num)
				{
					num = magnitude;
					curPoint = i;
				}
			}
			if (list.Count > 0)
			{
				int index = Random.Range(0, list.Count - 1);
				curPoint = list[index];
			}
			return _spawnPoints[curPoint];
		}

		public Transform GetCurSpawnPoint()
		{
			return _spawnPoints[curPoint];
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Transform[] spawnPoints = _spawnPoints;
			foreach (Transform transform in spawnPoints)
			{
				Vector2 vector = MathUtil.Rotate(new Vector2(0f, 1f), transform.eulerAngles.y);
				Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + new Vector3(vector.x, 0f, vector.y) * 2f + Vector3.up * 0.1f);
			}
		}
	}
}
