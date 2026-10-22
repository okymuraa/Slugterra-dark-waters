using UnityEngine;

public class SMAnimSpeedRandomizer : MonoBehaviour
{
	public float minSpeed = 0.7f;

	public float maxSpeed = 1.5f;

	private void Start()
	{
		Animation component = GetComponent<Animation>();
		if (component == null)
		{
			return;
		}
		AnimationClip clip = component.clip;
		if (!(clip == null))
		{
			AnimationState animationState = component[clip.name];
			if (!(animationState == null))
			{
				animationState.speed = Random.Range(minSpeed, maxSpeed);
			}
		}
	}
}
