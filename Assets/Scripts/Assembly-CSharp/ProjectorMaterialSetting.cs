using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorMaterialSetting : MonoBehaviour
{
	public enum ActionType
	{
		notUseProjector = 0,
		UseProjector = 1
	}

	public ActionType actionType;

	public Texture _projTexture;

	public Texture _projAddTexture;

	public bool notUseInEditor = true;

	public float texturePower = 1f;

	public float scaleX;

	public float scaleY;

	public float offsetX;

	public float offsetY;

	public CustomMaterialsChange[] customMaterialsChange;

	private Material[] _material;

	private MeshRenderer _meshRendererUpdate;

	private SkinnedMeshRenderer _skinnedMeshRendererUpdate;

	public List<Material> _curMaterial = new List<Material>();

	private string shaderName = "Mobile/Diffuse_projector";

	private void Start()
	{
		if (notUseInEditor)
		{
			if (!Application.isEditor)
			{
				StartCoroutine(Action());
			}
		}
		else
		{
			StartCoroutine(Action());
		}
	}

	private IEnumerator Action()
	{
		yield return new WaitForSeconds(0.5f);
		_material = (Material[])Resources.FindObjectsOfTypeAll(typeof(Material));
		Material[] material = _material;
		foreach (Material material2 in material)
		{
			if (material2 != null && material2.shader != null && material2.shader.name != null && material2.shader.name == "Mobile/Diffuse_projector")
			{
				_curMaterial.Add(material2);
			}
		}
		if (actionType.ToString() == "notUseProjector")
		{
			for (int j = 0; j < _curMaterial.Count; j++)
			{
				_curMaterial[j].SetFloat("_UseProjTexture", 0f);
			}
		}
		if (actionType.ToString() == "UseProjector")
		{
			for (int k = 0; k < _curMaterial.Count; k++)
			{
				_curMaterial[k].SetFloat("_UseProjTexture", 1f);
				_curMaterial[k].SetTexture("_ProjTexture", _projTexture);
				_curMaterial[k].SetFloat("_ProjTexturePower", texturePower);
				_curMaterial[k].SetFloat("_ScaleX", scaleX);
				_curMaterial[k].SetFloat("_ScaleY", scaleY);
				_curMaterial[k].SetFloat("_OffsetX", offsetX);
				_curMaterial[k].SetFloat("_OffsetY", offsetY);
			}
			if (customMaterialsChange != null)
			{
				for (int l = 0; l < customMaterialsChange.Length; l++)
				{
					if (customMaterialsChange[l] != null && customMaterialsChange[l].material.shader.name != null)
					{
						customMaterialsChange[l].material.SetFloat("_ProjTexturePower", customMaterialsChange[l].projTexturePower);
					}
				}
			}
		}
		_material = null;
	}

	public void UpdateFullMaterial()
	{
		_material = (Material[])Resources.FindObjectsOfTypeAll(typeof(Material));
		Material[] material = _material;
		foreach (Material material2 in material)
		{
			if (material2 != null && material2.shader != null && material2.shader.name != null && material2.shader.name == "Mobile/Diffuse_projector")
			{
				_curMaterial.Add(material2);
			}
		}
		for (int j = 0; j < _curMaterial.Count; j++)
		{
			_curMaterial[j].SetFloat("_UseProjTexture", 1f);
			_curMaterial[j].SetTexture("_ProjTexture", _projTexture);
			_curMaterial[j].SetFloat("_ProjTexturePower", texturePower);
			_curMaterial[j].SetFloat("_ScaleX", scaleX);
			_curMaterial[j].SetFloat("_ScaleY", scaleY);
			_curMaterial[j].SetFloat("_OffsetX", offsetX);
			_curMaterial[j].SetFloat("_OffsetY", offsetY);
		}
		if (customMaterialsChange != null)
		{
			for (int k = 0; k < customMaterialsChange.Length; k++)
			{
				if (customMaterialsChange[k] != null && customMaterialsChange[k].material.shader.name != null)
				{
					customMaterialsChange[k].material.SetFloat("_ProjTexturePower", customMaterialsChange[k].projTexturePower);
				}
			}
		}
		_material = null;
	}

	public void UpdateCurMaterial()
	{
		for (int i = 0; i < _curMaterial.Count; i++)
		{
			_curMaterial[i].SetFloat("_UseProjTexture", 1f);
			_curMaterial[i].SetTexture("_ProjTexture", _projTexture);
			_curMaterial[i].SetFloat("_ProjTexturePower", texturePower);
			_curMaterial[i].SetFloat("_ScaleX", scaleX);
			_curMaterial[i].SetFloat("_ScaleY", scaleY);
			_curMaterial[i].SetFloat("_OffsetX", offsetX);
			_curMaterial[i].SetFloat("_OffsetY", offsetY);
		}
	}

	public void ReplaseProjTexture(Transform Unit)
	{
		if (!(actionType.ToString() == "notUseProjector") && (!Application.isEditor || !notUseInEditor))
		{
			_meshRendererUpdate = Unit.GetComponentInChildren<MeshRenderer>();
			_skinnedMeshRendererUpdate = Unit.GetComponentInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < _curMaterial.Count; i++)
			{
				_curMaterial[i].SetTexture("_ProjTexture", _projAddTexture);
			}
		}
	}

	public void ReplaseProjTextureDefault(Transform Unit)
	{
		if (!(actionType.ToString() == "notUseProjector") && (!Application.isEditor || !notUseInEditor))
		{
			_meshRendererUpdate = Unit.GetComponentInChildren<MeshRenderer>();
			_skinnedMeshRendererUpdate = Unit.GetComponentInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < _curMaterial.Count; i++)
			{
				_curMaterial[i].SetTexture("_ProjTexture", _projTexture);
			}
		}
	}
}
