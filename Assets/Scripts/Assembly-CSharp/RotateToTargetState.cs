using BehaviorDesigner.Runtime.Tasks;
using Slugterra.UI.Game;
using UnityEngine;

public class RotateToTargetState : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private float _rotationPower;

	private Quaternion _neededRot;

	private GameController gameController;

	public override void OnStart()
	{
		if (gameController == null)
		{
			gameController = Object.FindObjectOfType<GameController>();
		}
		_rotationPower = dataHolder.Value.Me.Controller.PathFinder.turningSpeed * 60f;
		if (!dataHolder.Value.Me.AttackSettings.IsRangeAttacker)
		{
			dataHolder.Value.Me.Controller.PathFinder.canMove = false;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (!dataHolder.Value.Me.UnitSettings.IsALive || (dataHolder.Value.Me.AttackSettings.IsRangeAttacker && !dataHolder.Value.Me.AttackSettings.CanShoot))
		{
			return TaskStatus.Failure;
		}
		if (gameController.IsPlayCutScene && !dataHolder.Value.Me.UnitSettings.CutSceneUnlock)
		{
			return TaskStatus.Failure;
		}
		if (!dataHolder.Value.Enemy.Exist)
		{
			return TaskStatus.Failure;
		}
		if (dataHolder.Value.Me.Effects.InStun)
		{
			return TaskStatus.Running;
		}
		if (dataHolder.Value.Me.Effects.InFear || dataHolder.Value.Enemy.IsReloadSlug)
		{
			return TaskStatus.Failure;
		}
		if (dataHolder.Value.Me.Root == null)
		{
			Debug.Log("null my root");
		}
		if (dataHolder.Value.Enemy.Root == null)
		{
			Debug.Log("null enemy root");
		}
		Vector3 normalized = (dataHolder.Value.Enemy.AttackTargetPoint.position - dataHolder.Value.Me.Root.position).normalized;
		float num = Vector3.Angle(Vector3.forward, normalized);
		num *= ((!(normalized.x > 0f)) ? (-1f) : 1f);
		_neededRot = Quaternion.AngleAxis(num, Vector3.up);
		float sqrMagnitude = (dataHolder.Value.Enemy.Root.position - dataHolder.Value.Me.Root.position).sqrMagnitude;
		if (sqrMagnitude > dataHolder.Value.Me.Attack.AttackDistanceSQR)
		{
			return TaskStatus.Failure;
		}
		if (Mathf.Abs(dataHolder.Value.Me.Root.rotation.eulerAngles.y - _neededRot.eulerAngles.y) < ((!dataHolder.Value.Me.AttackSettings.IsRangeAttacker) ? 10f : 10f) || Mathf.Abs(dataHolder.Value.Me.Root.rotation.eulerAngles.y - _neededRot.eulerAngles.y) > ((!dataHolder.Value.Me.AttackSettings.IsRangeAttacker) ? 350f : 350f))
		{
			return TaskStatus.Success;
		}
		dataHolder.Value.Me.Root.rotation = Quaternion.RotateTowards(dataHolder.Value.Me.Root.rotation, _neededRot, _rotationPower * Time.deltaTime);
		return TaskStatus.Running;
	}
}
