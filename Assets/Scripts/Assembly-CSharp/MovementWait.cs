using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MovementWait : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private float usualWaitTimeMin = 1f;

	private float usualWaitTimeMax = 1.5f;

	private float loseTargetWaitTimeMin = 3f;

	private float loseTargetWaitTimeMax = 9f;

	private float usualWaitTime;

	private float loseTargetWaitTime;

	private bool enemyExist;

	public override void OnStart()
	{
		if (dataHolder.Value.Me.AttackSettings.IsRangeAttacker)
		{
			usualWaitTime = Random.Range(usualWaitTimeMin, usualWaitTimeMax);
		}
		else
		{
			usualWaitTime = 0f;
		}
		loseTargetWaitTime = Random.Range(loseTargetWaitTimeMin, loseTargetWaitTimeMax);
		enemyExist = dataHolder.Value.Enemy.Exist;
	}

	public override TaskStatus OnUpdate()
	{
		usualWaitTime -= Time.deltaTime;
		loseTargetWaitTime -= Time.deltaTime;
		if (dataHolder.Value.Enemy.Exist && !enemyExist)
		{
			return TaskStatus.Success;
		}
		if (dataHolder.Value.Me.Movement.IsMoveOrderRecieved)
		{
			dataHolder.Value.Me.UnitSettings.DelayAfterLoseTarget = false;
			return TaskStatus.Success;
		}
		if (dataHolder.Value.Me.Effects.InFear || dataHolder.Value.Me.Effects.InStun)
		{
			return TaskStatus.Success;
		}
		if (usualWaitTime < 0f && (!dataHolder.Value.Me.UnitSettings.DelayAfterLoseTarget || dataHolder.Value.Enemy.Exist))
		{
			return TaskStatus.Success;
		}
		if (loseTargetWaitTime < 0f)
		{
			dataHolder.Value.Me.UnitSettings.DelayAfterLoseTarget = false;
			return TaskStatus.Success;
		}
		return TaskStatus.Running;
	}
}
