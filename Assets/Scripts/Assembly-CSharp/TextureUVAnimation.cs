using UnityEngine;

public class TextureUVAnimation : MonoBehaviour
{
	public float uvOffsetSpeedX;

	public float uvOffsetSpeedY;

	private Vector2 offset;

	public bool pingPong;

	private bool _pause;

	private float dirX = 1f;

	private float dirY = 1f;

	public float minX = -1f;

	public float maxX = 1f;

	public float minY = -1f;

	public float maxY = 1f;

	public bool pause
	{
		get
		{
			return _pause;
		}
		set
		{
			_pause = value;
		}
	}

	private void Start()
	{
		offset = new Vector2(0f, 0f);
	}

	private void Update()
	{
		if (_pause)
		{
			return;
		}
		if (pingPong)
		{
			offset = new Vector2(offset.x + dirX * Time.deltaTime * uvOffsetSpeedX, offset.y + dirY * Time.deltaTime * uvOffsetSpeedY);
			if (offset.x > maxX)
			{
				offset.x = 2f * maxX - offset.x;
				dirX = 0f - dirX;
			}
			else if (offset.x < minX)
			{
				offset.x = 2f * minX - offset.x;
				dirX = 0f - dirX;
			}
			if (offset.y > maxY)
			{
				offset.y = 2f * maxY - offset.y;
				dirY = 0f - dirY;
			}
			else if (offset.y < minY)
			{
				offset.y = 2f * minY - offset.y;
				dirY = 0f - dirY;
			}
		}
		else
		{
			offset = new Vector2(offset.x + Time.deltaTime * uvOffsetSpeedX, offset.y + Time.deltaTime * uvOffsetSpeedY);
			if (offset.x > maxX)
			{
				offset.x = offset.x - maxX + minX;
			}
			else if (offset.x < minX)
			{
				offset.x = offset.x + maxX - minX;
			}
			if (offset.y > maxY)
			{
				offset.y = offset.y - maxY + minY;
			}
			else if (offset.y < minY)
			{
				offset.y = offset.y + maxY - minY;
			}
		}
		if ((bool)GetComponent<Renderer>())
		{
			GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
		}
	}
}
