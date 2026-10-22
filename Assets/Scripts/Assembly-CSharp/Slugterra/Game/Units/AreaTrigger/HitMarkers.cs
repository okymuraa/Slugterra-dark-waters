using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Units.AreaTrigger
{
	public class HitMarkers
	{
		private List<Transform> _markers;

		public List<Transform> Markers
		{
			get
			{
				return _markers;
			}
		}

		public HitMarkers()
		{
			_markers = new List<Transform>();
		}

		public Transform GetRandomMarker()
		{
			return _markers[Random.Range(0, _markers.Count)];
		}
	}
}
