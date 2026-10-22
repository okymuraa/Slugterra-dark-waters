using UnityEngine;

public class SMMaterialAlphaFader : MonoBehaviour
{
	public float fadeSpeed = 1f;

	public float beginTintAlpha = 0.5f;

	private void Update()
	{
		beginTintAlpha -= Time.deltaTime * fadeSpeed;
		GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f, 1f, 1f, beginTintAlpha));
	}
}
