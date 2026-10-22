using System.Collections.Generic;
using UnityEngine;

public class ShopItemShaderProxy : MonoBehaviour
{
	private Dictionary<Material, Shader> _materialsStored = new Dictionary<Material, Shader>();

	[SerializeField]
	private Shader sourceShader;

	public void Initialize()
	{
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
			{
				if (!_materialsStored.ContainsKey(componentsInChildren[i].materials[j]))
				{
					_materialsStored.Add(componentsInChildren[i].materials[j], componentsInChildren[i].materials[j].shader);
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			for (int l = 0; l < componentsInChildren2[k].materials.Length; l++)
			{
				if (!_materialsStored.ContainsKey(componentsInChildren2[k].materials[l]))
				{
					_materialsStored.Add(componentsInChildren2[k].materials[l], componentsInChildren2[k].materials[l].shader);
				}
			}
		}
		ApplyOpacityShader();
	}

	private void ApplyOpacityShader()
	{
		Shader shader = Shader.Find("Slugterra/ShopItem");
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
			{
				componentsInChildren[i].materials[j].shader = shader;
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			for (int l = 0; l < componentsInChildren2[k].materials.Length; l++)
			{
				componentsInChildren2[k].materials[l].shader = shader;
			}
		}
	}

	public void SetOpacity(float opacity)
	{
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
			{
				componentsInChildren[i].materials[j].SetFloat("_Opacity", opacity);
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			for (int l = 0; l < componentsInChildren2[k].materials.Length; l++)
			{
				componentsInChildren2[k].materials[l].SetFloat("_Opacity", opacity);
			}
		}
	}

	public void SetGraySclae(float grayScale)
	{
		MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
			{
				componentsInChildren[i].materials[j].SetFloat("_Colorfulness", grayScale);
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			for (int l = 0; l < componentsInChildren2[k].materials.Length; l++)
			{
				componentsInChildren2[k].materials[l].SetFloat("_Colorfulness", grayScale);
			}
		}
	}
}
