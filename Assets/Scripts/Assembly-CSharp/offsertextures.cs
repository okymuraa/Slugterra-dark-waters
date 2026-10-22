using UnityEngine;

public class offsertextures : MonoBehaviour
{
	public Renderer rend;

	private float offsettemp;

	private float offset;

	private void Start()
	{
		rend = GetComponent<Renderer>();
		offsettemp = Random.Range(0, 50);
	}

	private void LateUpdate()
	{
		offset = Mathf.Sin(Time.unscaledTime + offsettemp);
		rend.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
	}
}
