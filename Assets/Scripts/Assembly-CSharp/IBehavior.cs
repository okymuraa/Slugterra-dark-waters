using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using Slugterra.Game;
using Slugterra.Game.Units;
using Slugterra.Game.Wave;
using UnityEngine;

[Serializable]
public class IBehavior : MonoBehaviour
{
	[SerializeField]
	private BehaviorTree _myBehaviorTree;

	public BehaviorTree MyBehaviorTree
	{
		get
		{
			return _myBehaviorTree;
		}
		set
		{
			_myBehaviorTree = value;
		}
	}

	public virtual bool CanPatrol { get; set; }

	public virtual bool CanMove { get; set; }

	public virtual void Stun()
	{
		throw new NotImplementedException();
	}

	public virtual void UnStun()
	{
		throw new NotImplementedException();
	}

	public virtual void Blind()
	{
		throw new NotImplementedException();
	}

	public virtual void UnBlind()
	{
		throw new NotImplementedException();
	}

	public virtual void AttackTarget(Transform target)
	{
		throw new NotImplementedException();
	}

	public virtual void ConvertToAlly()
	{
		throw new NotImplementedException();
	}

	public virtual void ConvertToEnemy()
	{
		throw new NotImplementedException();
	}

	public virtual void UseFiniteCustomPath(MovePath path)
	{
		throw new NotImplementedException();
	}

	public virtual void OnAnnounce(Vector3 enemyPos)
	{
		throw new NotImplementedException();
	}

	public virtual void OnAnnounce(UnitController enemy)
	{
		throw new NotImplementedException();
	}

	public virtual void OnDamage(float damage, Attacker attacker)
	{
		throw new NotImplementedException();
	}

	public virtual void OnFallDown()
	{
		throw new NotImplementedException();
	}

	public virtual IEnumerator OnStandUp(float delayTime)
	{
		throw new NotImplementedException();
	}

	public virtual void UseCustomWalkSpeed(float customSpeed)
	{
		throw new NotImplementedException();
	}

	public virtual void UseCustomPatrolSpeed(float customSpeed)
	{
		throw new NotImplementedException();
	}

	public virtual void MoveTo(Vector3 movePoint)
	{
		throw new NotImplementedException();
	}

	public virtual void CutSceneUnlock()
	{
		throw new NotImplementedException();
	}

	public virtual void Fear()
	{
		throw new NotImplementedException();
	}

	public virtual void UnFear()
	{
		throw new NotImplementedException();
	}

	public virtual void AttackSpeedCoefChangged()
	{
	}
}
