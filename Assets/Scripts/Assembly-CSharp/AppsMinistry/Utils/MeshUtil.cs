using UnityEngine;

namespace AppsMinistry.Utils
{
	public static class MeshUtil
	{
		public static Vector3 GetPivot(Mesh mesh)
		{
			if (mesh != null)
			{
				Bounds bounds = mesh.bounds;
				Vector3 vector = bounds.center * -1f;
				return new Vector3(vector.x / bounds.extents.x, vector.y / bounds.extents.y, vector.z / bounds.extents.z);
			}
			return Vector3.zero;
		}

		public static Mesh FindFirstMeshInObjectHierarchy(GameObject go)
		{
			if (go != null)
			{
				MeshFilter componentInChildren = go.GetComponentInChildren<MeshFilter>();
				SkinnedMeshRenderer componentInChildren2 = go.GetComponentInChildren<SkinnedMeshRenderer>();
				Mesh result;
				if (componentInChildren != null)
				{
					result = componentInChildren.sharedMesh;
				}
				else
				{
					result = ((!(componentInChildren2 != null)) ? null : componentInChildren2.sharedMesh);
				}
				return result;
			}
			return null;
		}
	}
}
