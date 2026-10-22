using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BehaviourTreeTest
{
	[TaskCategory("Slugterra/Units")]
	public class IsAliveState : Conditional
	{
		public BehaviorDataHolderSharedVariable dataHolder;

		private float destroyTime;

		private bool mustDestroy;

		public override TaskStatus OnUpdate()
		{
			if (dataHolder.Value.Me.Controller.Model == null)
			{
				return TaskStatus.Success;
			}
			bool flag = dataHolder.Value.Me.Controller.Model.Health > 0f;
			if (flag != dataHolder.Value.Me.UnitSettings.IsALive)
			{
				dataHolder.Value.Me.UnitSettings.IsALive = !dataHolder.Value.Me.Controller.Model.IsDead();
				if (!flag)
				{
					dataHolder.Value.Me.Controller.Animator.SetTrigger("Death");
					dataHolder.Value.Me.Controller.Animator.SetTrigger("OnDeath");
					dataHolder.Value.Me.Controller.Model.Health = dataHolder.Value.Me.Controller.Model.Health;
					mustDestroy = true;
					GameObject gameObject = Resources.Load("Prefabs/Game/Units/prf_FX_Soul_Up_Smoke") as GameObject;
					gameObject.transform.position = dataHolder.Value.Me.Controller.transform.position;
					return TaskStatus.Failure;
				}
			}
			if (mustDestroy)
			{
				destroyTime = Mathf.MoveTowards(destroyTime, 0f, Time.deltaTime);
				if (destroyTime == 0f)
				{
					DestroyUnit();
				}
			}
			return (!flag) ? TaskStatus.Failure : TaskStatus.Success;
		}

		private void DestroyUnit()
		{
			Object.Destroy(gameObject);
			if (dataHolder.Value.Me.Controller != null && dataHolder.Value.Me.Controller.Transform.parent != null)
			{
				Object.Destroy(dataHolder.Value.Me.Controller.Transform.parent.gameObject);
			}
		}
	}
}
