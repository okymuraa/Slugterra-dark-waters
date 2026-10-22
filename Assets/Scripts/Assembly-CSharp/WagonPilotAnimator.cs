using Slugterra.Game.Units.Mechanisms.Spline;
using UnityEngine;

public class WagonPilotAnimator : MonoBehaviour
{
	[SerializeField]
	private TeamController tController;

	private Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (tController.Model.Health <= 0f)
		{
			anim.SetBool("pilotDown", true);
		}
		else
		{
			anim.SetBool("pilotDown", false);
		}
	}
}
