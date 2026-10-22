using System;
using System.IO;
using UnityEngine;

namespace Pathfinding.Serialization
{
	public class GraphSerializationContext
	{
		private readonly GraphNode[] id2NodeMapping;

		public readonly BinaryReader reader;

		public readonly BinaryWriter writer;

		public readonly uint graphIndex;

		public GraphSerializationContext(BinaryReader reader, GraphNode[] id2NodeMapping, uint graphIndex)
		{
			this.reader = reader;
			this.id2NodeMapping = id2NodeMapping;
			this.graphIndex = graphIndex;
		}

		public GraphSerializationContext(BinaryWriter writer)
		{
			this.writer = writer;
		}

		public int GetNodeIdentifier(GraphNode node)
		{
			return (node != null) ? node.NodeIndex : (-1);
		}

		public GraphNode GetNodeFromIdentifier(int id)
		{
			if (id2NodeMapping == null)
			{
				throw new Exception("Calling GetNodeFromIdentifier when serializing");
			}
			if (id == -1)
			{
				return null;
			}
			GraphNode graphNode = id2NodeMapping[id];
			if (graphNode == null)
			{
				throw new Exception("Invalid id (" + id + ")");
			}
			return graphNode;
		}

		public void SerializeVector3(Vector3 v)
		{
			writer.Write(v.x);
			writer.Write(v.y);
			writer.Write(v.z);
		}

		public Vector3 DeserializeVector3()
		{
			return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public int DeserializeInt(int defaultValue)
		{
			if (reader.BaseStream.Position <= reader.BaseStream.Length - 4)
			{
				return reader.ReadInt32();
			}
			return defaultValue;
		}

		public float DeserializeFloat(float defaultValue)
		{
			if (reader.BaseStream.Position <= reader.BaseStream.Length - 4)
			{
				return reader.ReadSingle();
			}
			return defaultValue;
		}

		public void SerializeUnityObject(UnityEngine.Object ob)
		{
			if (ob == null)
			{
				writer.Write(int.MaxValue);
				return;
			}
			int instanceID = ob.GetHashCode();
			string name = ob.name;
			string assemblyQualifiedName = ob.GetType().AssemblyQualifiedName;
			string value = string.Empty;
			Component component = ob as Component;
			GameObject gameObject = ob as GameObject;
			if (component != null || gameObject != null)
			{
				if (component != null && gameObject == null)
				{
					gameObject = component.gameObject;
				}
				UnityReferenceHelper unityReferenceHelper = gameObject.GetComponent<UnityReferenceHelper>();
				if (unityReferenceHelper == null)
				{
					Debug.Log("Adding UnityReferenceHelper to Unity Reference '" + ob.name + "'");
					unityReferenceHelper = gameObject.AddComponent<UnityReferenceHelper>();
				}
				unityReferenceHelper.Reset();
				value = unityReferenceHelper.GetGUID();
			}
			writer.Write(instanceID);
			writer.Write(name);
			writer.Write(assemblyQualifiedName);
			writer.Write(value);
		}

		public UnityEngine.Object DeserializeUnityObject()
		{
			int num = reader.ReadInt32();
			if (num == int.MaxValue)
			{
				return null;
			}
			string text = reader.ReadString();
			string text2 = reader.ReadString();
			string text3 = reader.ReadString();
			Type type = Type.GetType(text2);
			if (type == null)
			{
				Debug.LogError("Could not find type '" + text2 + "'. Cannot deserialize Unity reference");
				return null;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				UnityReferenceHelper[] array = UnityEngine.Object.FindObjectsOfType(typeof(UnityReferenceHelper)) as UnityReferenceHelper[];
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].GetGUID() == text3)
					{
						if (type == typeof(GameObject))
						{
							return array[i].gameObject;
						}
						return array[i].GetComponent(type);
					}
				}
			}
			UnityEngine.Object[] array2 = Resources.LoadAll(text, type);
			for (int j = 0; j < array2.Length; j++)
			{
				if (array2[j].name == text || array2.Length == 1)
				{
					return array2[j];
				}
			}
			return null;
		}
	}
}
