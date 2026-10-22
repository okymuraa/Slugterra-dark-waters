using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
	public string TagEnter = "Player";

	public GameObject TargetSendMessage;

	public string Message;

	public bool destroy;

	protected virtual void Enter()
	{
	}

	private void OnTriggerEnter(Collider _ColliderOther)
	{
		if (_ColliderOther.tag == TagEnter)
		{
			if ((bool)TargetSendMessage)
			{
				TargetSendMessage.SendMessage(Message);
			}
			if (destroy)
			{
				Object.Destroy(gameObject);
			}
			Enter();
		}
	}
}
