using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	[Serializable]
	public class LayerInfo
	{
		public string name;

		public int hash;

		public bool useColor;

		public bool generateCollider;

		public float z = 0.1f;

		public int unityLayer;

		public string sortingLayerName = string.Empty;

		public int sortingOrder;

		public bool skipMeshGeneration;

#if UNITY_6000_0_OR_NEWER
		public PhysicsMaterial physicMaterial;
#else
		public PhysicMaterial physicMaterial;
#endif

		public PhysicsMaterial2D physicsMaterial2D;

		public LayerInfo()
		{
			unityLayer = 0;
			useColor = true;
			generateCollider = true;
			skipMeshGeneration = false;
		}
	}
}
