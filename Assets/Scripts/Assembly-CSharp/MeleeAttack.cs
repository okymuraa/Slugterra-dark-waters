using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game;
using Slugterra.Game.Units.Animals;
using Slugterra.UI.Game;
using UnityEngine;

[TaskCategory("Slugterra/Units")]
public class MeleeAttack : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private Slug _slug;

	private TaskStatus attackStatus;

	private bool isAttackInProgress;

	private bool useFlopper = true;

	private GameController gameController;

	public override void OnAwake()
	{
		if (gameController == null)
		{
			gameController = Object.FindObjectOfType<GameController>();
		}
	}

	public override void OnStart()
	{
		if (!dataHolder.Value.Me.Attack.IsMecanimSubscribed)
		{
			dataHolder.Value.Me.Attack.IsMecanimSubscribed = true;
			dataHolder.Value.Me.Controller.Animator.GetComponent<MecanimDelegate>().OnAttackEvent += DealDelayedDamage;
		}
		attackStatus = TaskStatus.Running;
		isAttackInProgress = false;
	}

	private void BiteShinyMetalAss()
	{
		dataHolder.Value.Me.Controller.Animator.SetTrigger("Attack");
		if (dataHolder.Value.Me.MovementSettings.StepBackAfterAttack)
		{
			dataHolder.Value.Me.Movement.StepBackAfterAttack = Random.Range(0f, 1f) < dataHolder.Value.Me.MovementSettings.StepBackChance;
		}
	}

	private void DealDelayedDamage()
	{
		if (dataHolder.Value.Me.UnitSettings.IsALive && dataHolder.Value.Enemy.Exist)
		{
			float sqrMagnitude = (dataHolder.Value.Me.Root.position - dataHolder.Value.Enemy.Root.position).sqrMagnitude;
			if (sqrMagnitude < Mathf.Pow((dataHolder.Value.Me.Controller as AnimalController).AttackDistance, 2f) + 0.1f)
			{
				attackStatus = TaskStatus.Success;
				dataHolder.Value.Enemy.Controller.OnDamage((dataHolder.Value.Me.Controller.Model as Animal).Damage, new Attacker(dataHolder.Value.Me.Controller.GetType(), dataHolder.Value.Me.Root));
			}
			else
			{
				attackStatus = TaskStatus.Failure;
			}
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (!isAttackInProgress)
		{
			if (!dataHolder.Value.Enemy.Exist)
			{
				return TaskStatus.Failure;
			}
			BiteShinyMetalAss();
			isAttackInProgress = true;
		}
		if (dataHolder.Value.Me.Effects.InFear || dataHolder.Value.Enemy.IsReloadSlug)
		{
			return TaskStatus.Failure;
		}
		if (!dataHolder.Value.Enemy.Exist)
		{
			return TaskStatus.Failure;
		}
		if (gameController.IsPlayCutScene && !dataHolder.Value.Me.UnitSettings.CutSceneUnlock)
		{
			return TaskStatus.Failure;
		}
		float sqrMagnitude = (dataHolder.Value.Enemy.Root.position - dataHolder.Value.Me.Root.position).sqrMagnitude;
		if (sqrMagnitude > dataHolder.Value.Me.Attack.AttackDistanceSQR)
		{
			return TaskStatus.Failure;
		}
		return attackStatus;
	}
}
