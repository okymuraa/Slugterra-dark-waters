using Slugterra.Game.Units;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
	protected Animator animator;

	public bool ikActive;

	private Vector3 ik;

	public HumanoidController controller;

	public Transform IKRoot;

	public Transform IKfix;

	private Quaternion StartIKRootRot;

	private float Weight;

	private void Start()
	{
		animator = GetComponent<Animator>();
		StartIKRootRot = IKRoot.rotation;
		ikActive = true;
	}

	public void ResetWeight()
	{
		Weight = 0f;
	}

	private void OnAnimatorIK()
	{
		if (!animator)
		{
			return;
		}
		if (ikActive)
		{
			if (controller.AttackTarget != null && Vector3.Angle(controller.AttackTarget.position - controller.Transform.position, controller.Transform.forward) < 80f)
			{
				animator.SetLookAtWeight(1f);
				animator.SetLookAtPosition(controller.AttackTarget.position + Vector3.up * 1.65f);
			}
			else
			{
				animator.SetLookAtWeight(0f);
			}
			if (controller.AttackTarget != null && animator.GetBool("EnemyInSight") && Vector3.Angle(controller.AttackTarget.position - controller.Transform.position, controller.Transform.forward) < 45f)
			{
				if (Weight < 1f)
				{
					Weight += Time.deltaTime;
					if (Weight > 1f)
					{
						Weight = 1f;
					}
				}
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, Weight);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, Weight);
				IKRoot.LookAt(controller.AttackTarget.GetComponent<HitMarker>().Markers[0].transform);
				ik.x = IKRoot.localEulerAngles.x;
				animator.SetIKPosition(AvatarIKGoal.RightHand, IKfix.position);
				animator.SetIKRotation(AvatarIKGoal.RightHand, IKfix.rotation);
				if (Mathf.Abs(animator.GetFloat("VelocityX")) < 1f || (Mathf.Abs(animator.GetFloat("VelocityX")) < 1f && Mathf.Abs(animator.GetFloat("VelocityY")) == 0f))
				{
					animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, Weight);
				}
				else
				{
					animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
				}
				animator.SetIKPosition(AvatarIKGoal.LeftHand, IKfix.position);
				return;
			}
			if (Weight > 0f)
			{
				Weight -= Time.deltaTime;
				if (Weight < 0f)
				{
					Weight = 0f;
				}
			}
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, Weight);
			animator.SetIKRotationWeight(AvatarIKGoal.RightHand, Weight);
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, Weight);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, Weight);
		}
		else
		{
			IKRoot.rotation = StartIKRootRot;
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
			animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
			animator.SetLookAtWeight(0f);
		}
	}
}
