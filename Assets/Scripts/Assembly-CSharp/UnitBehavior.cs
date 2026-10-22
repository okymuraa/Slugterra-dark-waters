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
			return dataHolder.Me.MovementSettings.CanMove;
		}
		set
		{
			if (dataHolder == null)
			{
				dataHolder = (BehaviorDataHolder)MyBehaviorTree.GetVariable("DataHolder").GetValue();
			}
			dataHolder.Me.MovementSettings.CanMove = value;
		}
	}

	public override bool CanPatrol
	{
		get
		{
			return dataHolder.Me.MovementSettings.CanPatrol;
		}
		set
		{
			dataHolder.Me.MovementSettings.CanPatrol = value;
		}
	}

	private void OnEnable()
	{
		if (MyBehaviorTree == null)
		{
			Debug.Log("<color=red>Behavior tree link on UnitBehavior is null</color>");
			return;
		}
		SharedVariable variable = MyBehaviorTree.GetVariable("DataHolder");
		if (variable != null)
		{
			dataHolder = (BehaviorDataHolder)variable.GetValue();
		}
		else
		{
			Debug.Log("<color=red>sharedVariable is null</color>");
		}
		if (dataHolder != null && dataHolder.Me != null && dataHolder.Me.Controller != null)
		{
			dataHolder.Me.Controller.OnDamageRecievedEvent += OnDamage;
		}
		else
		{
			Debug.Log("<color=red>Problem with dataHolder something in dataHolder is null</color>");
		}
		GameController gameController = Object.FindObjectOfType<GameController>();
		if (gameController != null && gameController.IsMultiplayer)
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
		if (!dataHolder.Enemy.Exist && !dataHolder.Me.Effects.InFear && attacker.transform.GetComponent<IDestructible>() != null)
		{
			MoveTo(attacker.transform.position);
			dataHolder.Me.Controller.PathFinder.speed = dataHolder.Me.Controller.Model.RunSpeed;
		}
	}

	public override void CutSceneUnlock()
	{
		dataHolder.Me.UnitSettings.CutSceneUnlock = true;
	}

	public override void Fear()
	{
		dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
		dataHolder.Me.Effects.InFear = true;
	}

	public override void UnFear()
	{
		dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
		dataHolder.Me.Effects.InFear = false;
		dataHolder.Me.Controller.ViewArea.UpdateTargets();
	}

	public override void Stun()
	{
		dataHolder.Me.Effects.InStun = true;
	}

	public override void UnStun()
	{
		dataHolder.Me.Effects.InStun = false;
	}

	public override void Blind()
	{
		dataHolder.Me.AttackSettings.CanShoot = false;
		dataHolder.Enemy.Controller = null;
		dataHolder.Enemy.Root = null;
		dataHolder.Enemy.AttackTargetPoint = null;
	}

	public override void UnBlind()
	{
		dataHolder.Me.AttackSettings.CanShoot = true;
		dataHolder.Me.Controller.ViewArea.UpdateTargets();
	}

	public override void AttackTarget(Transform target)
	{
		if (target == null)
		{
			dataHolder.Enemy.Root = null;
			dataHolder.Enemy.Controller = null;
			dataHolder.Enemy.AttackTargetPoint = null;
			dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
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
		dataHolder.Enemy.Root = null;
		dataHolder.Enemy.Controller = null;
		dataHolder.Enemy.AttackTargetPoint = null;
	}

	public override void ConvertToEnemy()
	{
		dataHolder.Enemy.Root = null;
		dataHolder.Enemy.Controller = null;
		dataHolder.Enemy.AttackTargetPoint = null;
	}

	public override void OnFallDown()
	{
		dataHolder.Me.AttackSettings.CanShoot = false;
		dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", false);
	}

	public override IEnumerator OnStandUp(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		dataHolder.Me.AttackSettings.CanShoot = true;
		dataHolder.Me.Controller.Animator.SetBool("EnemyInSight", dataHolder.Enemy.Exist);
	}

	public override void MoveTo(Vector3 movePoint)
	{
		dataHolder.Me.Movement.MovePoint.position = movePoint;
		dataHolder.Me.Movement.IsMoveOrderRecieved = true;
		CutSceneUnlock();
	}

	public override void UseFiniteCustomPath(MovePath path)
	{
		if (path.Points != null && path.Points.Count != 0)
		{
			if (customPatrolPath == null)
			{
				customPatrolPath = gameObject.AddComponent<PatrolPathHolder>();
			}
			customPatrolPath.SetPath(path.Points);
			customPatrolPath.EndlessPath = false;
			dataHolder = (BehaviorDataHolder)MyBehaviorTree.GetVariable("DataHolder").GetValue();
			dataHolder.Me.MovementSettings.PatrolPath = customPatrolPath;
		}
	}

	public override void UseCustomWalkSpeed(float customSpeed)
	{
		dataHolder.Me.MovementSettings.UseCustomWalkSpeed = true;
		dataHolder.Me.MovementSettings.CustomWalkSpeed = customSpeed;
	}

	public override void UseCustomPatrolSpeed(float customSpeed)
	{
		dataHolder.Me.MovementSettings.UseCustomPatrolSpeed = true;
		dataHolder.Me.MovementSettings.CustomPatrolSpeed = customSpeed;
	}

	public override void AttackSpeedCoefChangged()
	{
		dataHolder.Me.Attack.ReloadCoefChanged = true;
	}
}
