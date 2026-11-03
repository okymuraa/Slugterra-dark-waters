using System.Collections;
using BehaviorDesigner.Runtime;
using Slugterra.Behavior;
using Slugterra.Game;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Mechanisms;
using Slugterra.Game.Units.Mechanisms.Spline;
using Slugterra.Game.Wave;
using Slugterra.UI.Game;
using UnityEngine;

public class UnitBehavior : IBehavior
{
	private PatrolPathHolder customPatrolPath;

	private BehaviorDataHolder dataHolder;

	public override bool CanMove
	{
		get
		{
			return dataHolder != null && dataHolder.Me.MovementSettings.CanMove;
		}
		set
		{
			if (dataHolder == null && MyBehaviorTree != null && MyBehaviorTree.GetVariable("DataHolder") != null)
			{
				dataHolder = (BehaviorDataHolder)MyBehaviorTree.GetVariable("DataHolder").GetValue();
			}
			if (dataHolder != null)
			{
				dataHolder.Me.MovementSettings.CanMove = value;
			}
		}
	}

	public override bool CanPatrol
	{
		get
		{
			return dataHolder != null && dataHolder.Me.MovementSettings.CanPatrol;
		}
		set
		{
			if (dataHolder != null)
			{
				dataHolder.Me.MovementSettings.CanPatrol = value;
			}
		}
	}

	private void OnEnable()
	{
		if (MyBehaviorTree == null)
		{
			return;
		}
		SharedVariable variable = MyBehaviorTree.GetVariable("DataHolder");
		if (variable != null)
		{
			dataHolder = (BehaviorDataHolder)variable.GetValue();
		}
		if (dataHolder != null && dataHolder.Me != null && dataHolder.Me.Controller != null)
		{
			dataHolder.Me.Controller.OnDamageRecievedEvent += OnDamage;
		}
		GameController gameController = Object.FindObjectOfType<GameController>();
		if (dataHolder != null && gameController != null && gameController.IsMultiplayer)
		{
			EliController eliController = Object.FindObjectOfType<EliController>();
			if (eliController != null)
			{
				GameObject gameObject = new GameObject("~pointToMove");
				gameObject.transform.position = eliController.transform.parent.position;
				dataHolder.Me.Movement.MovePoint = gameObject.transform;
			}
		}
	}

	private void OnDisable()
	{
		if (dataHolder != null && dataHolder.Me.Controller != null)
		{
			dataHolder.Me.Controller.OnDamageRecievedEvent -= OnDamage;
		}
	}

	public override void OnDamage(float damage, Attacker attacker)
	{
		if (dataHolder != null && !dataHolder.Enemy.Exist && !dataHolder.Me.Effects.InFear && attacker != null && attacker.transform != null && attacker.transform.GetComponent<IDestructible>() != null)
		{
			MoveTo(attacker.transform.position);
			dataHolder.Me.Controller.PathFinder.speed = dataHolder.Me.Controller.Model.RunSpeed;
		}
	}

	public override void CutSceneUnlock()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.UnitSettings.CutSceneUnlock = true;
		}
	}

	public override void Fear()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
			dataHolder.Me.Effects.InFear = true;
		}
	}

	public override void UnFear()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
			dataHolder.Me.Effects.InFear = false;
			dataHolder.Me.Controller.ViewArea.UpdateTargets();
		}
	}

	public override void Stun()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.Effects.InStun = true;
		}
	}

	public override void UnStun()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.Effects.InStun = false;
		}
	}

	public override void Blind()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.AttackSettings.CanShoot = false;
			dataHolder.Enemy.Controller = null;
			dataHolder.Enemy.Root = null;
			dataHolder.Enemy.AttackTargetPoint = null;
		}
	}

	public override void UnBlind()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.AttackSettings.CanShoot = true;
			dataHolder.Me.Controller.ViewArea.UpdateTargets();
		}
	}

	public override void AttackTarget(Transform target)
	{
		if (dataHolder == null)
		{
			return;
		}
		if (target == null)
		{
			dataHolder.Enemy.Root = null;
			dataHolder.Enemy.Controller = null;
			dataHolder.Enemy.AttackTargetPoint = null;
			if (dataHolder.Me.Controller != null && dataHolder.Me.Controller.Animator != null)
			{
				dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
			}
		}
		else
		{
			if (dataHolder.Me.Effects.InFear)
			{
				return;
			}
			IDestructible component = target.GetComponent<IDestructible>();
			if ((dataHolder.Me.AttackSettings.IsRangeAttacker || (!(target.GetComponent<TeamController>() != null) && !(target.GetComponent<MechanismController>() != null))) && component != null)
			{
				TeamController component2 = target.GetComponent<TeamController>();
				MechanismController component3 = target.GetComponent<MechanismController>();
				UnitController component4 = target.GetComponent<UnitController>();
				if ((!(component2 != null) || !component2.Model.IsDead()) && (!(component3 != null) || !component3.Model.IsDead()) && (!(component4 != null) || !component4.Model.IsDead()))
				{
					dataHolder.Enemy.Root = target;
					dataHolder.Enemy.Controller = component;
					dataHolder.Enemy.AttackTargetPoint = ((!(target.GetComponent<HitMarker>() != null)) ? target : target.GetComponent<HitMarker>().Markers[0]);
					dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", true);
					dataHolder.Me.UnitSettings.DelayAfterLoseTarget = false;
					CutSceneUnlock();
				}
			}
		}
	}

	public override void ConvertToAlly()
	{
		if (dataHolder != null)
		{
			dataHolder.Enemy.Root = null;
			dataHolder.Enemy.Controller = null;
			dataHolder.Enemy.AttackTargetPoint = null;
		}
	}

	public override void ConvertToEnemy()
	{
		if (dataHolder != null)
		{
			dataHolder.Enemy.Root = null;
			dataHolder.Enemy.Controller = null;
			dataHolder.Enemy.AttackTargetPoint = null;
		}
	}

	public override void OnFallDown()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.AttackSettings.CanShoot = false;
			if (dataHolder.Me.Controller != null && dataHolder.Me.Controller.Animator != null)
			{
				dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
			}
		}
	}

	public override IEnumerator OnStandUp(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if (dataHolder != null)
		{
			dataHolder.Me.AttackSettings.CanShoot = true;
			if (dataHolder.Me.Controller != null && dataHolder.Me.Controller.Animator != null)
			{
				dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", dataHolder.Enemy.Exist);
			}
		}
	}

	public override void MoveTo(Vector3 movePoint)
	{
		if (dataHolder != null && dataHolder.Me.Movement.MovePoint != null)
		{
			dataHolder.Me.Movement.MovePoint.position = movePoint;
			dataHolder.Me.Movement.IsMoveOrderRecieved = true;
			CutSceneUnlock();
		}
	}

	public override void UseFiniteCustomPath(MovePath path)
	{
		if (path != null && path.Points != null && path.Points.Count != 0)
		{
			if (customPatrolPath == null)
			{
				customPatrolPath = gameObject.AddComponent<PatrolPathHolder>();
			}
			customPatrolPath.SetPath(path.Points);
			customPatrolPath.EndlessPath = false;
			if (MyBehaviorTree != null && MyBehaviorTree.GetVariable("DataHolder") != null)
			{
				dataHolder = (BehaviorDataHolder)MyBehaviorTree.GetVariable("DataHolder").GetValue();
			}
			if (dataHolder != null)
			{
				dataHolder.Me.MovementSettings.PatrolPath = customPatrolPath;
			}
		}
	}

	public override void UseCustomWalkSpeed(float customSpeed)
	{
		if (dataHolder != null)
		{
			dataHolder.Me.MovementSettings.UseCustomWalkSpeed = true;
			dataHolder.Me.MovementSettings.CustomWalkSpeed = customSpeed;
		}
	}

	public override void UseCustomPatrolSpeed(float customSpeed)
	{
		if (dataHolder != null)
		{
			dataHolder.Me.MovementSettings.UseCustomPatrolSpeed = true;
			dataHolder.Me.MovementSettings.CustomPatrolSpeed = customSpeed;
		}
	}

	public override void AttackSpeedCoefChangged()
	{
		if (dataHolder != null)
		{
			dataHolder.Me.Attack.ReloadCoefChanged = true;
		}
	}
}
