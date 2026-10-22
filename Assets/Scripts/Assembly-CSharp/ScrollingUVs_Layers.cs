using UnityEngine;

public class ScrollingUVs_Layers : MonoBehaviour
{
	public Vector2 uvAnimationRate = new Vector2(1f, 0f);

	public string textureName = "_MainTex";

	private Vector2 uvOffset = Vector2.zero;

	private Renderer mesh;

	private void Start()
	{
		mesh = GetComponent<Renderer>();
	}

	private void LateUpdate()
	{
		uvOffset += uvAnimationRate * Time.unscaledDeltaTime;
		if (uvAnimationRate.x > 0f)
		{
			uvOffset.x = Mathf.Repeat(uvOffset.x, 1f);
		}
		else
		{
			uvOffset.x = Mathf.Repeat(uvOffset.x, -1f);
		}
		if (uvAnimationRate.y > 0f)
		{
			uvOffset.y = Mathf.Repeat(uvOffset.y, 1f);
		}
		else
		{
			uvOffset.y = Mathf.Repeat(uvOffset.y, -1f);
		}
		if (mesh.enabled)
		{
			mesh.sharedMaterial.SetTextureOffset(textureName, uvOffset);
		}
	}
}
