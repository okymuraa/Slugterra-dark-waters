using UnityEngine;

[ExecuteInEditMode]
public class tk2dAnimationAdapter : MonoBehaviour
{
	public Color color = Color.white;

	private tk2dBaseSprite sprite;

	private void Start()
	{
		sprite = GetComponent<tk2dBaseSprite>();
		if (sprite != null)
		{
			sprite.color = color;
		}
	}

	private void Update()
	{
		if (sprite != null && sprite.color != color)
		{
			sprite.color = color;
		}
	}
}
