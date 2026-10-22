using UnityEngine;

public class SMUVScroller : MonoBehaviour
{
	public int targetMaterialSlot;

	public float speedY = 0.5f;

	public float speedX;

	private float timeWentX;

	private float timeWentY;

	private void Start()
	{
	}

	private void Update()
	{
		timeWentY += Time.deltaTime * speedY;
		timeWentX += Time.deltaTime * speedX;
		GetComponent<Renderer>().materials[targetMaterialSlot].SetTextureOffset("_MainTex", new Vector2(timeWentX, timeWentY));
	}
}
