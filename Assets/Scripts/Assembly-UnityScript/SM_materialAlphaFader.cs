using System;
using UnityEngine;

[Serializable]
public class SM_materialAlphaFader : MonoBehaviour
{
	public float fadeSpeed;

	public float beginTintAlpha;

	public SM_materialAlphaFader()
	{
		fadeSpeed = 1f;
		beginTintAlpha = 0.5f;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		beginTintAlpha -= Time.deltaTime * fadeSpeed;
		GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f, 1f, 1f, beginTintAlpha));
	}

	public virtual void Main()
	{
	}
}
