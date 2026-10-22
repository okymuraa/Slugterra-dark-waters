using System.Collections.Generic;
using AppsMinistry.Core;
using UnityEngine;

namespace Slugterra.Game
{
	public class CoverSystem : MonoSingleton<CoverSystem>
	{
		private List<CoverNode> _nodes;

		protected override void OnCreate()
		{
			CoverNode[] collection = Object.FindObjectsOfType<CoverNode>();
			_nodes = new List<CoverNode>(collection);
		}

		public CoverNode GetOptimalNode(Transform baseTransform, Transform targetTransform, float optimalDistance)
		{
			if (targetTransform == null)
			{
				return null;
			}
			Vector3 position = targetTransform.position;
			Vector3 position2 = baseTransform.position;
			float num = float.MaxValue;
			CoverNode result = null;
			foreach (CoverNode node in _nodes)
			{
				float num2 = Vector3.SqrMagnitude(position - node.Transform.position);
				if (num2 <= optimalDistance * optimalDistance)
				{
					float num3 = Vector3.SqrMagnitude(position2 - node.Transform.position) - num2;
					if (num3 < num)
					{
						result = node;
						num = num3;
					}
				}
			}
			return result;
		}

		public CoverNode GetNearestNode(Transform baseTransform, Transform targetTransform, float distance)
		{
			if (targetTransform == null)
			{
				return null;
			}
			Vector3 position = targetTransform.position;
			Vector3 position2 = baseTransform.position;
			float num = float.MinValue;
			CoverNode result = null;
			foreach (CoverNode node in _nodes)
			{
				float num2 = Vector3.SqrMagnitude(position2 - node.Transform.position);
				if (num2 <= distance * distance)
				{
					float num3 = Vector3.SqrMagnitude(position - node.Transform.position);
					if (num3 > num)
					{
						result = node;
						num = num3;
					}
				}
			}
			return result;
		}
	}
}
