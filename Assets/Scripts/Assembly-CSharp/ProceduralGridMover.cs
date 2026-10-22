using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_procedural_grid_mover.php")]
public class ProceduralGridMover : MonoBehaviour
{
	public float updateDistance = 10f;

	public Transform target;

	public bool floodFill;

	private GridGraph graph;

	private GridNode[] tmp;

	public bool updatingGraph { get; private set; }

	public void Start()
	{
		if (AstarPath.active == null)
		{
			throw new Exception("There is no AstarPath object in the scene");
		}
		graph = AstarPath.active.astarData.gridGraph;
		if (graph == null)
		{
			throw new Exception("The AstarPath object has no GridGraph");
		}
		UpdateGraph();
	}

	private void Update()
	{
		Vector3 a = PointToGraphSpace(graph.center);
		Vector3 b = PointToGraphSpace(target.position);
		if (VectorMath.SqrDistanceXZ(a, b) > updateDistance * updateDistance)
		{
			UpdateGraph();
		}
	}

	private Vector3 PointToGraphSpace(Vector3 p)
	{
		return graph.inverseMatrix.MultiplyPoint(p);
	}

	public void UpdateGraph()
	{
		if (updatingGraph)
		{
			return;
		}
		updatingGraph = true;
		IEnumerator ie = UpdateGraphCoroutine();
		AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((bool force) =>
		{
			if (force)
			{
				while (ie.MoveNext())
				{
				}
			}
			bool flag = !ie.MoveNext();
			if (flag)
			{
				updatingGraph = false;
			}
			return flag;
		}));
	}

	private IEnumerator UpdateGraphCoroutine()
	{
		Vector3 dir = PointToGraphSpace(target.position) - PointToGraphSpace(graph.center);
		dir.x = Mathf.Round(dir.x);
		dir.z = Mathf.Round(dir.z);
		dir.y = 0f;
		if (dir == Vector3.zero)
		{
			yield break;
		}
		Int2 offset = new Int2(-Mathf.RoundToInt(dir.x), -Mathf.RoundToInt(dir.z));
		graph.center += graph.matrix.MultiplyVector(dir);
		graph.GenerateMatrix();
		if (tmp == null || tmp.Length != graph.nodes.Length)
		{
			tmp = new GridNode[graph.nodes.Length];
		}
		int width = graph.width;
		int depth = graph.depth;
		GridNode[] nodes = graph.nodes;
		if (Mathf.Abs(offset.x) <= width && Mathf.Abs(offset.y) <= depth)
		{
			for (int i = 0; i < depth; i++)
			{
				int num = i * width;
				int num2 = (i + offset.y + depth) % depth * width;
				for (int j = 0; j < width; j++)
				{
					tmp[num2 + (j + offset.x + width) % width] = nodes[num + j];
				}
			}
			yield return null;
			for (int k = 0; k < depth; k++)
			{
				int num3 = k * width;
				for (int l = 0; l < width; l++)
				{
					GridNode gridNode = tmp[num3 + l];
					gridNode.NodeInGridIndex = num3 + l;
					nodes[num3 + l] = gridNode;
				}
			}
			IntRect r = new IntRect(0, 0, offset.x, offset.y);
			int minz = r.ymax;
			int maxz = depth;
			if (r.xmin > r.xmax)
			{
				int xmax = r.xmax;
				r.xmax = width + r.xmin;
				r.xmin = width + xmax;
			}
			if (r.ymin > r.ymax)
			{
				int ymax = r.ymax;
				r.ymax = depth + r.ymin;
				r.ymin = depth + ymax;
				minz = 0;
				maxz = r.ymin;
			}
			r = r.Expand(graph.erodeIterations + 1);
			r = IntRect.Intersection(r, new IntRect(0, 0, width, depth));
			yield return null;
			for (int m = r.ymin; m < r.ymax; m++)
			{
				for (int n = 0; n < width; n++)
				{
					graph.UpdateNodePositionCollision(nodes[m * width + n], n, m, false);
				}
			}
			yield return null;
			for (int num4 = minz; num4 < maxz; num4++)
			{
				for (int num5 = r.xmin; num5 < r.xmax; num5++)
				{
					graph.UpdateNodePositionCollision(nodes[num4 * width + num5], num5, num4, false);
				}
			}
			yield return null;
			for (int num6 = r.ymin; num6 < r.ymax; num6++)
			{
				for (int num7 = 0; num7 < width; num7++)
				{
					graph.CalculateConnections(num7, num6, nodes[num6 * width + num7]);
				}
			}
			yield return null;
			for (int num8 = minz; num8 < maxz; num8++)
			{
				for (int num9 = r.xmin; num9 < r.xmax; num9++)
				{
					graph.CalculateConnections(num9, num8, nodes[num8 * width + num9]);
				}
			}
			yield return null;
			for (int num10 = 0; num10 < depth; num10++)
			{
				for (int num11 = 0; num11 < width; num11++)
				{
					if (num11 == 0 || num10 == 0 || num11 >= width - 1 || num10 >= depth - 1)
					{
						graph.CalculateConnections(num11, num10, nodes[num10 * width + num11]);
					}
				}
			}
		}
		else
		{
			for (int num12 = 0; num12 < depth; num12++)
			{
				for (int num13 = 0; num13 < width; num13++)
				{
					graph.UpdateNodePositionCollision(nodes[num12 * width + num13], num13, num12, false);
				}
			}
			for (int num14 = 0; num14 < depth; num14++)
			{
				for (int num15 = 0; num15 < width; num15++)
				{
					graph.CalculateConnections(num15, num14, nodes[num14 * width + num15]);
				}
			}
		}
		if (floodFill)
		{
			yield return null;
			AstarPath.active.QueueWorkItemFloodFill();
		}
	}
}
