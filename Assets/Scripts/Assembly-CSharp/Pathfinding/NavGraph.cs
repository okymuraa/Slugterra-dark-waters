using System;
using Pathfinding.Serialization;
using Pathfinding.Serialization.JsonFx;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public abstract class NavGraph
	{
		public byte[] _sguid;

		public AstarPath active;

		[JsonMember]
		public uint initialPenalty;

		[JsonMember]
		public bool open;

		public uint graphIndex;

		[JsonMember]
		public string name;

		[JsonMember]
		public bool drawGizmos = true;

		[JsonMember]
		public bool infoScreenOpen;

		public Matrix4x4 matrix = Matrix4x4.identity;

		public Matrix4x4 inverseMatrix = Matrix4x4.identity;

		[JsonMember]
		public Pathfinding.Util.Guid guid
		{
			get
			{
				if (_sguid == null || _sguid.Length != 16)
				{
					_sguid = Pathfinding.Util.Guid.NewGuid().ToByteArray();
				}
				return new Pathfinding.Util.Guid(_sguid);
			}
			set
			{
				_sguid = value.ToByteArray();
			}
		}

		public virtual int CountNodes()
		{
			int count = 0;
			GraphNodeDelegateCancelable del = (GraphNode node) =>
			{
				count++;
				return true;
			};
			GetNodes(del);
			return count;
		}

		public abstract void GetNodes(GraphNodeDelegateCancelable del);

		public void SetMatrix(Matrix4x4 m)
		{
			matrix = m;
			inverseMatrix = m.inverse;
		}

		public virtual void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
		{
			Matrix4x4 inverse = oldMatrix.inverse;
			Matrix4x4 m = newMatrix * inverse;
			GetNodes((GraphNode node) =>
			{
				node.position = (Int3)m.MultiplyPoint((Vector3)node.position);
				return true;
			});
			SetMatrix(newMatrix);
		}

		public NNInfo GetNearest(Vector3 position)
		{
			return GetNearest(position, NNConstraint.None);
		}

		public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
		{
			return GetNearest(position, constraint, null);
		}

		public virtual NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			float maxDistSqr = ((!constraint.constrainDistance) ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr);
			float minDist = float.PositiveInfinity;
			GraphNode minNode = null;
			float minConstDist = float.PositiveInfinity;
			GraphNode minConstNode = null;
			GetNodes((GraphNode node) =>
			{
				float sqrMagnitude = (position - (Vector3)node.position).sqrMagnitude;
				if (sqrMagnitude < minDist)
				{
					minDist = sqrMagnitude;
					minNode = node;
				}
				if (sqrMagnitude < minConstDist && sqrMagnitude < maxDistSqr && constraint.Suitable(node))
				{
					minConstDist = sqrMagnitude;
					minConstNode = node;
				}
				return true;
			});
			NNInfo result = new NNInfo(minNode);
			result.constrainedNode = minConstNode;
			if (minConstNode != null)
			{
				result.constClampedPosition = (Vector3)minConstNode.position;
			}
			else if (minNode != null)
			{
				result.constrainedNode = minNode;
				result.constClampedPosition = (Vector3)minNode.position;
			}
			return result;
		}

		public virtual NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			return GetNearest(position, constraint);
		}

		public virtual void Awake()
		{
		}

		public virtual void OnDestroy()
		{
			GetNodes((GraphNode node) =>
			{
				node.Destroy();
				return true;
			});
		}

		public void ScanGraph()
		{
			if (AstarPath.OnPreScan != null)
			{
				AstarPath.OnPreScan(AstarPath.active);
			}
			if (AstarPath.OnGraphPreScan != null)
			{
				AstarPath.OnGraphPreScan(this);
			}
			ScanInternal();
			if (AstarPath.OnGraphPostScan != null)
			{
				AstarPath.OnGraphPostScan(this);
			}
			if (AstarPath.OnPostScan != null)
			{
				AstarPath.OnPostScan(AstarPath.active);
			}
		}

		[Obsolete("Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had")]
		public void Scan()
		{
			throw new Exception("This method is deprecated. Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had.");
		}

		public void ScanInternal()
		{
			ScanInternal(null);
		}

		public abstract void ScanInternal(OnScanStatus statusCallback);

		public virtual Color NodeColor(GraphNode node, PathHandler data)
		{
			Color result = AstarColor.NodeConnection;
			switch (AstarPath.active.debugMode)
			{
			case GraphDebugMode.Areas:
				result = AstarColor.GetAreaColor(node.Area);
				break;
			case GraphDebugMode.Penalty:
				result = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, ((float)node.Penalty - AstarPath.active.debugFloor) / (AstarPath.active.debugRoof - AstarPath.active.debugFloor));
				break;
			case GraphDebugMode.Tags:
				result = AstarColor.GetAreaColor(node.Tag);
				break;
			default:
			{
				if (data == null)
				{
					return AstarColor.NodeConnection;
				}
				PathNode pathNode = data.GetPathNode(node);
				switch (AstarPath.active.debugMode)
				{
				case GraphDebugMode.G:
					result = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, ((float)pathNode.G - AstarPath.active.debugFloor) / (AstarPath.active.debugRoof - AstarPath.active.debugFloor));
					break;
				case GraphDebugMode.H:
					result = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, ((float)pathNode.H - AstarPath.active.debugFloor) / (AstarPath.active.debugRoof - AstarPath.active.debugFloor));
					break;
				case GraphDebugMode.F:
					result = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, ((float)pathNode.F - AstarPath.active.debugFloor) / (AstarPath.active.debugRoof - AstarPath.active.debugFloor));
					break;
				}
				break;
			}
			}
			result.a *= 0.5f;
			return result;
		}

		public virtual void SerializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		public virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		public virtual void PostDeserialization()
		{
		}

		public virtual void SerializeSettings(GraphSerializationContext ctx)
		{
			ctx.writer.Write(guid.ToByteArray());
			ctx.writer.Write(initialPenalty);
			ctx.writer.Write(open);
			ctx.writer.Write(name ?? string.Empty);
			ctx.writer.Write(drawGizmos);
			ctx.writer.Write(infoScreenOpen);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					ctx.writer.Write(matrix.GetRow(i)[j]);
				}
			}
		}

		public virtual void DeserializeSettings(GraphSerializationContext ctx)
		{
			guid = new Pathfinding.Util.Guid(ctx.reader.ReadBytes(16));
			initialPenalty = ctx.reader.ReadUInt32();
			open = ctx.reader.ReadBoolean();
			name = ctx.reader.ReadString();
			drawGizmos = ctx.reader.ReadBoolean();
			infoScreenOpen = ctx.reader.ReadBoolean();
			for (int i = 0; i < 4; i++)
			{
				Vector4 zero = Vector4.zero;
				for (int j = 0; j < 4; j++)
				{
					zero[j] = ctx.reader.ReadSingle();
				}
				matrix.SetRow(i, zero);
			}
		}

		public static bool InSearchTree(GraphNode node, Path path)
		{
			if (path == null || path.pathHandler == null)
			{
				return true;
			}
			PathNode pathNode = path.pathHandler.GetPathNode(node);
			return pathNode.pathID == path.pathID;
		}

		public virtual void OnDrawGizmos(bool drawNodes)
		{
			if (!drawNodes)
			{
				return;
			}
			PathHandler data = AstarPath.active.debugPathData;
			GraphNode node = null;
			GraphNodeDelegate drawConnection = (GraphNode otherNode) =>
			{
				Gizmos.DrawLine((Vector3)node.position, (Vector3)otherNode.position);
			};
			GetNodes((GraphNode _node) =>
			{
				node = _node;
				Gizmos.color = NodeColor(node, AstarPath.active.debugPathData);
				if (AstarPath.active.showSearchTree && !InSearchTree(node, AstarPath.active.debugPath))
				{
					return true;
				}
				PathNode pathNode = ((data == null) ? null : data.GetPathNode(node));
				if (AstarPath.active.showSearchTree && pathNode != null && pathNode.parent != null)
				{
					Gizmos.DrawLine((Vector3)node.position, (Vector3)pathNode.parent.node.position);
				}
				else
				{
					node.GetConnections(drawConnection);
				}
				return true;
			});
		}

		internal virtual void UnloadGizmoMeshes()
		{
		}
	}
}
