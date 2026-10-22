using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
	public GameObject TargetObject;

	public string Message = "start";

	public void EventActive()
	{
		TargetObject.SendMessage(Message);
	}
}
