using Holoville.HOTween;
using UnityEngine;

public class ItemBlinking : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve _blinkingAnimationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private float _time = 1f;

	[SerializeField]
	private int _loops = -1;

	public Color currentColor;

	public Color toColor;

	private Material[] _cachedMaterials;

	private Tweener _tweener;

	public void Play()
	{
		if (!HOTween.IsTweening(this) && _tweener != null)
		{
			_tweener.Play();
		}
	}

	public void Stop()
	{
		if (HOTween.IsTweening(this) && _tweener != null)
		{
			_tweener.Restart();
			_tweener.Pause();
		}
	}

	private void Start()
	{
		_cachedMaterials = gameObject.GetComponent<Renderer>().materials;
		if (_cachedMaterials[0].HasProperty("_Color"))
		{
			TweenParms p_parms = new TweenParms().Prop("currentColor", toColor).Loops(_loops).Ease(_blinkingAnimationCurve);
			_tweener = HOTween.To(this, _time, p_parms);
			_tweener.Pause();
		}
	}

	private void Update()
	{
		Material[] cachedMaterials = _cachedMaterials;
		foreach (Material material in cachedMaterials)
		{
			if (material.HasProperty("_Color"))
			{
				material.SetVector("_Color", new Vector4(currentColor.r, currentColor.g, currentColor.b, 1f));
			}
		}
	}
}
