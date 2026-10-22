using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pathfinding.Serialization.Zip;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Serialization
{
	public class AstarSerializer
	{
		private AstarData data;

		private ZipFile zip;

		private MemoryStream zipStream;

		private GraphMeta meta;

		private SerializeSettings settings;

		private NavGraph[] graphs;

		private Dictionary<NavGraph, int> graphIndexInZip;

		private int graphIndexOffset;

		private const string binaryExt = ".binary";

		private const string jsonExt = ".binary";

		private uint checksum = uint.MaxValue;

		private static StringBuilder _stringBuilder = new StringBuilder();

		public AstarSerializer(AstarData data)
		{
			this.data = data;
			settings = SerializeSettings.Settings;
		}

		public AstarSerializer(AstarData data, SerializeSettings settings)
		{
			this.data = data;
			this.settings = settings;
		}

		private static StringBuilder GetStringBuilder()
		{
			_stringBuilder.Length = 0;
			return _stringBuilder;
		}

		public void SetGraphIndexOffset(int offset)
		{
			graphIndexOffset = offset;
		}

		private void AddChecksum(byte[] bytes)
		{
			checksum = Checksum.GetChecksum(bytes, checksum);
		}

		public uint GetChecksum()
		{
			return checksum;
		}

		public void OpenSerialize()
		{
			zip = new ZipFile();
			zip.AlternateEncoding = Encoding.UTF8;
			zip.AlternateEncodingUsage = ZipOption.Always;
			meta = new GraphMeta();
		}

		public byte[] CloseSerialize()
		{
			byte[] bytes = SerializeMeta();
			AddChecksum(bytes);
			zip.AddEntry("meta.binary", bytes);
			MemoryStream memoryStream = new MemoryStream();
			zip.Save(memoryStream);
			bytes = memoryStream.ToArray();
			memoryStream.Dispose();
			zip.Dispose();
			zip = null;
			return bytes;
		}

		public void SerializeGraphs(NavGraph[] _graphs)
		{
			if (graphs != null)
			{
				throw new InvalidOperationException("Cannot serialize graphs multiple times.");
			}
			graphs = _graphs;
			if (zip == null)
			{
				throw new NullReferenceException("You must not call CloseSerialize before a call to this function");
			}
			if (graphs == null)
			{
				graphs = new NavGraph[0];
			}
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					byte[] bytes = Serialize(graphs[i]);
					AddChecksum(bytes);
					zip.AddEntry("graph" + i + ".binary", bytes);
				}
			}
		}

		private byte[] SerializeMeta()
		{
			if (graphs == null)
			{
				throw new Exception("No call to SerializeGraphs has been done");
			}
			meta.version = AstarPath.Version;
			meta.graphs = graphs.Length;
			meta.guids = new string[graphs.Length];
			meta.typeNames = new string[graphs.Length];
			meta.nodeCounts = new int[graphs.Length];
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					meta.guids[i] = graphs[i].guid.ToString();
					meta.typeNames[i] = graphs[i].GetType().FullName;
				}
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write("A*");
			binaryWriter.Write(meta.version.Major);
			binaryWriter.Write(meta.version.Minor);
			binaryWriter.Write(meta.version.Build);
			binaryWriter.Write(meta.version.Revision);
			binaryWriter.Write(meta.graphs);
			binaryWriter.Write(meta.guids.Length);
			for (int j = 0; j < meta.guids.Length; j++)
			{
				binaryWriter.Write(meta.guids[j] ?? string.Empty);
			}
			binaryWriter.Write(meta.typeNames.Length);
			for (int k = 0; k < meta.typeNames.Length; k++)
			{
				binaryWriter.Write(meta.typeNames[k] ?? string.Empty);
			}
			binaryWriter.Write(meta.nodeCounts.Length);
			for (int l = 0; l < meta.nodeCounts.Length; l++)
			{
				binaryWriter.Write(meta.nodeCounts[l]);
			}
			return memoryStream.ToArray();
		}

		public byte[] Serialize(NavGraph graph)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(writer);
			graph.SerializeSettings(ctx);
			return memoryStream.ToArray();
		}

		[Obsolete("Not used anymore. You can safely remove the call to this function.")]
		public void SerializeNodes()
		{
		}

		private static int GetMaxNodeIndexInAllGraphs(NavGraph[] graphs)
		{
			int maxIndex = 0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] == null)
				{
					continue;
				}
				graphs[i].GetNodes((GraphNode node) =>
				{
					maxIndex = Math.Max(node.NodeIndex, maxIndex);
					if (node.NodeIndex == -1)
					{
						Debug.LogError("Graph contains destroyed nodes. This is a bug.");
					}
					return true;
				});
			}
			return maxIndex;
		}

		private static byte[] SerializeNodeIndices(NavGraph[] graphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter wr = new BinaryWriter(memoryStream);
			int maxNodeIndexInAllGraphs = GetMaxNodeIndexInAllGraphs(graphs);
			wr.Write(maxNodeIndexInAllGraphs);
			int maxNodeIndex2 = 0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					graphs[i].GetNodes((GraphNode node) =>
					{
						maxNodeIndex2 = Math.Max(node.NodeIndex, maxNodeIndex2);
						wr.Write(node.NodeIndex);
						return true;
					});
				}
			}
			if (maxNodeIndex2 != maxNodeIndexInAllGraphs)
			{
				throw new Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
			}
			byte[] result = memoryStream.ToArray();
			wr.Close();
			return result;
		}

		private static byte[] SerializeGraphExtraInfo(NavGraph graph)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryWriter);
			graph.SerializeExtraInfo(ctx);
			byte[] result = memoryStream.ToArray();
			binaryWriter.Close();
			return result;
		}

		private static byte[] SerializeGraphNodeReferences(NavGraph graph)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryWriter);
			graph.GetNodes((GraphNode node) =>
			{
				node.SerializeReferences(ctx);
				return true;
			});
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public void SerializeExtraInfo()
		{
			if (!settings.nodes)
			{
				return;
			}
			if (graphs == null)
			{
				throw new InvalidOperationException("Cannot serialize extra info with no serialized graphs (call SerializeGraphs first)");
			}
			byte[] bytes = SerializeNodeIndices(graphs);
			AddChecksum(bytes);
			zip.AddEntry("graph_references.binary", bytes);
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					bytes = SerializeGraphExtraInfo(graphs[i]);
					AddChecksum(bytes);
					zip.AddEntry("graph" + i + "_extra.binary", bytes);
					bytes = SerializeGraphNodeReferences(graphs[i]);
					AddChecksum(bytes);
					zip.AddEntry("graph" + i + "_references.binary", bytes);
				}
			}
		}

		public void SerializeEditorSettings(GraphEditorBase[] editors)
		{
			if (editors != null && settings.editorSettings)
			{
			}
		}

		public bool OpenDeserialize(byte[] bytes)
		{
			zipStream = new MemoryStream();
			zipStream.Write(bytes, 0, bytes.Length);
			zipStream.Position = 0L;
			try
			{
				zip = ZipFile.Read(zipStream);
			}
			catch (Exception ex)
			{
				Debug.LogError("Caught exception when loading from zip\n" + ex);
				zipStream.Dispose();
				return false;
			}
			meta = DeserializeMeta(zip["meta.binary"]);
			if (FullyDefinedVersion(meta.version) > FullyDefinedVersion(AstarPath.Version))
			{
				Debug.LogWarning(string.Concat("Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: ", AstarPath.Version, " Data version: ", meta.version, "\nThis is usually fine as the stored data is usually backwards and forwards compatible.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n"));
			}
			else if (FullyDefinedVersion(meta.version) < FullyDefinedVersion(AstarPath.Version))
			{
				Debug.LogWarning(string.Concat("Trying to load data from an older version of the A* Pathfinding Project\nCurrent version: ", AstarPath.Version, " Data version: ", meta.version, "\nThis is usually fine, it just means you have upgraded to a new version.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n"));
			}
			return true;
		}

		private static Version FullyDefinedVersion(Version v)
		{
			return new Version(Mathf.Max(v.Major, 0), Mathf.Max(v.Minor, 0), Mathf.Max(v.Build, 0), Mathf.Max(v.Revision, 0));
		}

		public void CloseDeserialize()
		{
			zipStream.Dispose();
			zip.Dispose();
			zip = null;
			zipStream = null;
		}

		private NavGraph DeserializeGraph(int zipIndex, int graphIndex)
		{
			Type graphType = meta.GetGraphType(zipIndex);
			if (object.Equals(graphType, null))
			{
				return null;
			}
			ZipEntry zipEntry = zip["graph" + zipIndex + ".binary"];
			if (zipEntry == null)
			{
				throw new FileNotFoundException("Could not find data for graph " + zipIndex + " in zip. Entry 'graph" + zipIndex + ".binary' does not exist");
			}
			NavGraph navGraph = data.CreateGraph(graphType);
			navGraph.graphIndex = (uint)graphIndex;
			BinaryReader binaryReader = GetBinaryReader(zipEntry);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryReader, null, navGraph.graphIndex);
			navGraph.DeserializeSettings(ctx);
			if (navGraph.guid.ToString() != meta.guids[zipIndex])
			{
				throw new Exception(string.Concat("Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n", navGraph.guid, " != ", meta.guids[zipIndex]));
			}
			return navGraph;
		}

		public NavGraph[] DeserializeGraphs()
		{
			List<NavGraph> list = new List<NavGraph>();
			graphIndexInZip = new Dictionary<NavGraph, int>();
			for (int i = 0; i < meta.graphs; i++)
			{
				int graphIndex = list.Count + graphIndexOffset;
				NavGraph navGraph = DeserializeGraph(i, graphIndex);
				if (navGraph != null)
				{
					list.Add(navGraph);
					graphIndexInZip[navGraph] = i;
				}
			}
			graphs = list.ToArray();
			return graphs;
		}

		private bool DeserializeExtraInfo(NavGraph graph)
		{
			int num = graphIndexInZip[graph];
			ZipEntry zipEntry = zip["graph" + num + "_extra.binary"];
			if (zipEntry == null)
			{
				return false;
			}
			BinaryReader binaryReader = GetBinaryReader(zipEntry);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryReader, null, graph.graphIndex);
			graph.DeserializeExtraInfo(ctx);
			return true;
		}

		private bool AnyDestroyedNodesInGraphs()
		{
			bool result = false;
			for (int i = 0; i < graphs.Length; i++)
			{
				graphs[i].GetNodes((GraphNode node) =>
				{
					if (node.Destroyed)
					{
						result = true;
					}
					return true;
				});
			}
			return result;
		}

		private GraphNode[] DeserializeNodeReferenceMap()
		{
			ZipEntry zipEntry = zip["graph_references.binary"];
			if (zipEntry == null)
			{
				throw new Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			BinaryReader reader = GetBinaryReader(zipEntry);
			int num = reader.ReadInt32();
			GraphNode[] int2Node = new GraphNode[num + 1];
			try
			{
				for (int i = 0; i < graphs.Length; i++)
				{
					graphs[i].GetNodes((GraphNode node) =>
					{
						int num2 = reader.ReadInt32();
						int2Node[num2] = node;
						return true;
					});
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", innerException);
			}
			if (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				throw new Exception(reader.BaseStream.Length / 4 + " nodes were serialized, but only data for " + reader.BaseStream.Position / 4 + " nodes was found. The data looks corrupt.");
			}
			reader.Close();
			return int2Node;
		}

		private void DeserializeNodeReferences(NavGraph graph, GraphNode[] int2Node)
		{
			int num = graphIndexInZip[graph];
			ZipEntry zipEntry = zip["graph" + num + "_references.binary"];
			if (zipEntry == null)
			{
				throw new Exception("Node references for graph " + num + " not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			BinaryReader binaryReader = GetBinaryReader(zipEntry);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryReader, int2Node, graph.graphIndex);
			graph.GetNodes((GraphNode node) =>
			{
				node.DeserializeReferences(ctx);
				return true;
			});
		}

		public void DeserializeExtraInfo()
		{
			bool flag = false;
			for (int i = 0; i < graphs.Length; i++)
			{
				flag |= DeserializeExtraInfo(graphs[i]);
			}
			if (flag)
			{
				if (AnyDestroyedNodesInGraphs())
				{
					Debug.LogError("Graph contains destroyed nodes. This is a bug.");
				}
				GraphNode[] int2Node = DeserializeNodeReferenceMap();
				for (int j = 0; j < graphs.Length; j++)
				{
					DeserializeNodeReferences(graphs[j], int2Node);
				}
			}
		}

		public void PostDeserialization()
		{
			for (int i = 0; i < graphs.Length; i++)
			{
				graphs[i].PostDeserialization();
			}
		}

		public void DeserializeEditorSettings(GraphEditorBase[] graphEditors)
		{
		}

		private static BinaryReader GetBinaryReader(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			return new BinaryReader(memoryStream);
		}

		private static string GetString(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			string result = streamReader.ReadToEnd();
			memoryStream.Position = 0L;
			streamReader.Dispose();
			return result;
		}

		private GraphMeta DeserializeMeta(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new Exception("No metadata found in serialized data.");
			}
			GraphMeta graphMeta = new GraphMeta();
			BinaryReader binaryReader = GetBinaryReader(entry);
			if (binaryReader.ReadString() != "A*")
			{
				throw new Exception("Invalid magic number in saved data");
			}
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			int num3 = binaryReader.ReadInt32();
			int num4 = binaryReader.ReadInt32();
			if (num < 0)
			{
				graphMeta.version = new Version(0, 0);
			}
			else if (num2 < 0)
			{
				graphMeta.version = new Version(num, 0);
			}
			else if (num3 < 0)
			{
				graphMeta.version = new Version(num, num2);
			}
			else if (num4 < 0)
			{
				graphMeta.version = new Version(num, num2, num3);
			}
			else
			{
				graphMeta.version = new Version(num, num2, num3, num4);
			}
			graphMeta.graphs = binaryReader.ReadInt32();
			graphMeta.guids = new string[binaryReader.ReadInt32()];
			for (int i = 0; i < graphMeta.guids.Length; i++)
			{
				graphMeta.guids[i] = binaryReader.ReadString();
			}
			graphMeta.typeNames = new string[binaryReader.ReadInt32()];
			for (int j = 0; j < graphMeta.typeNames.Length; j++)
			{
				graphMeta.typeNames[j] = binaryReader.ReadString();
			}
			graphMeta.nodeCounts = new int[binaryReader.ReadInt32()];
			for (int k = 0; k < graphMeta.nodeCounts.Length; k++)
			{
				graphMeta.nodeCounts[k] = binaryReader.ReadInt32();
			}
			return graphMeta;
		}

		public static void SaveToFile(string path, byte[] data)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.Write(data, 0, data.Length);
			}
		}

		public static byte[] LoadFromFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				byte[] array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				return array;
			}
		}
	}
}
