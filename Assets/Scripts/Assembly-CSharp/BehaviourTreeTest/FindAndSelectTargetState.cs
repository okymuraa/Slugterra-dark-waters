using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Mechanisms;
using Slugterra.Game.Units.Mechanisms.Spline;
using UnityEngine;

namespace BehaviourTreeTest
{
	[TaskCategory("Slugterra/Units")]
	public class FindAndSelectTargetState : Conditional
	{
		public BehaviorDataHolderSharedVariable dataHolder;

		private bool isSubscribed;

		public override void OnStart()
		{
			if (!isSubscribed)
			{
				Subscribe();
				isSubscribed = true;
			}
		}

		private void OnViewTriggerEnter(Transform tr)
		{
			if (dataHolder.Value.Me.Effects.InFear)
			{
				return;
			}
			IDestructible component = tr.GetComponent<IDestructible>();
			if ((dataHolder.Value.Me.AttackSettings.IsRangeAttacker || (!(tr.GetComponent<TeamController>() != null) && !(tr.GetComponent<MechanismController>() != null))) && component != null)
			{
				TeamController component2 = tr.GetComponent<TeamController>();
				MechanismController component3 = tr.GetComponent<MechanismController>();
				UnitController component4 = tr.GetComponent<UnitController>();
				if ((!(component2 != null) || !component2.Model.IsDead()) && (!(component3 != null) || !component3.Model.IsDead()) && (!(component4 != null) || !component4.Model.IsDead()))
				{
					dataHolder.Value.Enemy.Root = tr;
					dataHolder.Value.Enemy.Controller = component;
					dataHolder.Value.Enemy.AttackTargetPoint = ((!(tr.GetComponent<HitMarker>() != null)) ? tr : tr.GetComponent<HitMarker>().Markers[0]);
					dataHolder.Value.Me.Controller.Animator.SetBool("EnemyInSight", true);
					dataHolder.Value.Me.UnitSettings.DelayAfterLoseTarget = false;
				}
			}
		}

		private void OnViewTriggerExit(Transform tr)
		{
			if (!(tr == null))
			{
				dataHolder.Value.Enemy.Controller = null;
				dataHolder.Value.Enemy.AttackTargetPoint = null;
				dataHolder.Value.Enemy.Root = null;
				dataHolder.Value.Me.Controller.Animator.SetBool("EnemyInSight", false);
				if (dataHolder.Value.Me.AttackSettings.IsRangeAttacker)
				{
					dataHolder.Value.Me.UnitSettings.DelayAfterLoseTarget = true;
				}
				dataHolder.Value.Me.ViewAreaTrigger.UpdateTargets();
				if (!dataHolder.Value.Me.AttackSettings.IsRangeAttacker && !dataHolder.Value.Enemy.Exist && tr != null)
				{
					dataHolder.Value.Enemy.LastKnownPos = tr.position + new Vector3(1f * Random.Range(-1f, 1f), 0f, 1f * Random.Range(-1f, 1f));
					dataHolder.Value.Enemy.IsTargetLost = true;
				}
			}
		}

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}

		private void Subscribe()
		{
			dataHolder.Value.Me.ViewAreaTrigger.OnViewTriggerEnterEvent += OnViewTriggerEnter;
			dataHolder.Value.Me.ViewAreaTrigger.OnViewTriggerExitEvent += OnViewTriggerExit;
		}
	}
}
