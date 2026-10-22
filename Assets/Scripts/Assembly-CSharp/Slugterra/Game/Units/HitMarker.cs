using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class HitMarker : MonoBehaviour
	{
		[SerializeField]
		private List<Transform> _markers;

		public List<Transform> Markers
		{
			get
			{
				return _markers;
			}
		}

		public HitMarker()
		{
			_markers = new List<Transform>();
		}

		private void Start()
		{
			if (_markers == null)
			{
				_markers = new List<Transform>();
			}
			if (_markers.Count == 0)
			{
				_markers.Add(transform);
			}
		}

		public Transform GetRandomMarker()
		{
			return _markers[Random.Range(0, _markers.Count)];
		}
	}
}
