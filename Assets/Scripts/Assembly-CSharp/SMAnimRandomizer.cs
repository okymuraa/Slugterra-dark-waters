using UnityEngine;

public class SMAnimRandomizer : MonoBehaviour
{
	public AnimationClip[] animList;

	public AnimationClip actualAnim;

	public float minSpeed = 0.7f;

	public float maxSpeed = 1.5f;

	private void Start()
	{
		float num = Mathf.Round(Random.Range(0, animList.Length));
		actualAnim = animList[(int)num];
		GetComponent<Animation>().Play(actualAnim.name);
		GetComponent<Animation>()[actualAnim.name].speed = Random.Range(minSpeed, maxSpeed);
	}
}
