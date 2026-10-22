using System.Collections.Generic;
using AppsMinistry.Core;
using UnityEngine;

namespace Slugterra.Game
{
	public class WarSystem : MonoSingleton<WarSystem>
	{
		private List<WarNode> _nodes;

		protected override void OnCreate()
		{
			WarNode[] collection = Object.FindObjectsOfType<WarNode>();
			_nodes = new List<WarNode>(collection);
		}

		public WarNode GetNode(Transform baseTransform, Transform targetTransform, float distance)
		{
			WarNode result = null;
			if (baseTransform == null || targetTransform == null)
			{
				return result;
			}
			Vector3 position = baseTransform.position;
			Vector3 position2 = targetTransform.position;
			float num = float.MaxValue;
			foreach (WarNode node in _nodes)
			{
				if (node.IsContainEmptyPoints())
				{
					float optimalWeight = node.GetOptimalWeight(position, position2, distance);
					if (num > optimalWeight)
					{
						num = optimalWeight;
						result = node;
					}
				}
			}
			return result;
		}
	}
}
