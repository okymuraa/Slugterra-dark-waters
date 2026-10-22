using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Domain.Game.Units;
using UnityEngine;

public class BlasterReload : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private float unitReloadTime;

	private float reloadTime;

	public override void OnStart()
	{
		reloadTime = 0f;
		if (dataHolder.Value.Me.AttackSettings.IsRangeAttacker)
		{
			unitReloadTime = ((dataHolder.Value.Me.Controller.Model as Humanoid).Blaster.FireRate + 0.02f) * (dataHolder.Value.Me.Controller.Model as Humanoid).CurrentAttackSpeedCoeficient();
		}
		else if (!dataHolder.Value.Me.Movement.StepBackAfterAttack)
		{
			unitReloadTime = 1.7f + Random.Range(-0.2f, 0.2f);
		}
		else
		{
			unitReloadTime = 0.1f;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (dataHolder.Value.Me.Effects.InFear || dataHolder.Value.Enemy.IsReloadSlug)
		{
			return TaskStatus.Failure;
		}
		reloadTime += Time.deltaTime;
		if (dataHolder.Value.Me.Attack.ReloadCoefChanged)
		{
			unitReloadTime = ((dataHolder.Value.Me.Controller.Model as Humanoid).Blaster.FireRate + 0.02f) * (dataHolder.Value.Me.Controller.Model as Humanoid).CurrentAttackSpeedCoeficient();
		}
		if (reloadTime >= unitReloadTime)
		{
			return TaskStatus.Success;
		}
		return TaskStatus.Running;
	}
}
