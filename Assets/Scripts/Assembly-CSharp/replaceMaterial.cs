using UnityEngine;

[ExecuteInEditMode]
public class replaceMaterial : MonoBehaviour
{
	private MeshRenderer[] _material;

	public Material curMaterial;

	public bool replase;

	private void Start()
	{
	}

	private void Update()
	{
		if (replase)
		{
			replase = false;
			_material = (MeshRenderer[])Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
			MeshRenderer[] material = _material;
			foreach (MeshRenderer meshRenderer in material)
			{
				meshRenderer.material = curMaterial;
			}
		}
	}
}
