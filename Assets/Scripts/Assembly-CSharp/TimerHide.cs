using UnityEngine;

public class TimerHide : MonoBehaviour
{
	public float LifeTime = 0.5f;

	private float lifeTime;

	private Transform transformObj;

	private void Awake()
	{
		lifeTime = LifeTime;
		transformObj = transform;
	}

	private void FixedUpdate()
	{
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0f)
		{
			Hide();
		}
	}

	private void Hide()
	{
		transformObj.gameObject.SetActive(false);
		lifeTime = LifeTime;
	}
}
