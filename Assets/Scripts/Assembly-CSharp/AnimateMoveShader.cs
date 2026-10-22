using UnityEngine;

public class AnimateMoveShader : MonoBehaviour
{
	[SerializeField]
	private Vector2 _speed = Vector2.one;

	private Vector2 _offset = Vector2.zero;

	private MeshRenderer _meshRenderer;

	private SkinnedMeshRenderer _SkinMeshRenderer;

	private void Start()
	{
		_meshRenderer = gameObject.GetComponent<MeshRenderer>();
		_SkinMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
	}

	private void Update()
	{
		if (_meshRenderer != null)
		{
			_offset = new Vector2(Mathf.Repeat(_speed.x * Time.time, 1f), Mathf.Repeat(_speed.y * Time.time, 1f));
			_meshRenderer.material.SetTextureOffset("_MainTex", _offset);
		}
		if (_SkinMeshRenderer != null)
		{
			_offset += _speed * Time.deltaTime;
			_SkinMeshRenderer.material.SetTextureOffset("_MainTex", _offset);
		}
	}
}
