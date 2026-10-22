using Slugterra.Scene.Game;
using UnityEngine;

public class Train_6_1_TrainTrigger : MonoBehaviour
{
	public enum Action
	{
		none = 0,
		action0 = 1,
		action1 = 2
	}

	public float moveTo;

	public float durationTrain;

	public Action action;

	[SerializeField]
	private Train_6_1 train_6_1;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Action_Move();
		}
	}

	public void Action_Move()
	{
		train_6_1.MoveTrain(moveTo, durationTrain, action.ToString());
		gameObject.SetActive(false);
	}
}
