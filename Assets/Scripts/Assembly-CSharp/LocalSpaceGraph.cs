using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_local_space_graph.php")]
public class LocalSpaceGraph : MonoBehaviour
{
	protected Matrix4x4 originalMatrix;

	private void Start()
	{
		originalMatrix = transform.localToWorldMatrix;
	}

	public Matrix4x4 GetMatrix()
	{
		return transform.worldToLocalMatrix * originalMatrix;
	}
}
