using UnityEngine;

public class SequenceUVs_Layers : MonoBehaviour
{
	public string textureName = "_MainTex";

	public Texture[] Sequence;

	public float UpdateRate = 0.1f;

	private float _timer;

	private int i;

	private void Start()
	{
		_timer = UpdateRate;
	}

	private void LateUpdate()
	{
		_timer -= Time.deltaTime;
		if (!(_timer > 0f))
		{
			if (GetComponent<Renderer>().enabled)
			{
				GetComponent<Renderer>().sharedMaterial.SetTexture(textureName, Sequence[i]);
			}
			i++;
			if (i >= Sequence.Length)
			{
				i = 0;
			}
			_timer = UpdateRate;
		}
	}
}
