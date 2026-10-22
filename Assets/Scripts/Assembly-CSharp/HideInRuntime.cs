using UnityEngine;

[AddComponentMenu("Slugterra/Editor/HideInRuntime")]
public class HideInRuntime : MonoBehaviour
{
	private MeshRenderer v_ThisMeshRender;

	private void Start()
	{
		v_ThisMeshRender = GetComponent<MeshRenderer>();
		v_ThisMeshRender.enabled = false;
	}
}
