using System;
using UnityEngine;

[Serializable]
public class CustomLightmapData
{
	public MeshRenderer meshRenderer;

	public int lightmapIndex = -1;

	public Vector4 lightmapScaleOffset = Vector4.zero;
}
