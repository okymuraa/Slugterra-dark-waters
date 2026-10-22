using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.State;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class BillyController : EnemyController
	{
		private float _retargetTimeDelay = 3f;

		private float _nextRetarget;

		private bool isMultiplayer;

		public override EnemyType mEnemyType
		{
			get
			{
				return EnemyType.Billy;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			if (isMultiplayer)
			{
				ViewArea.SetRadius(40f);
			}
			Patrol();
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			GameController gameController = Object.FindObjectOfType<GameController>();
			GameObject blasterPrefab = GameFactory.GetBlasterPrefab(_model.Blaster.Type);
			isMultiplayer = gameController.IsMultiplayer;
			if (gameController.IsMultiplayer)
			{
				blasterPrefab = GameFactory.GetBlasterPrefab(BlasterType.Default);
				_model.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(BlasterType.LeaveBlaster, 6), BlasterType.LeaveBlaster);
				BlasterType = BlasterType.LeaveBlaster;
				GameObject gameObject = Object.Instantiate(blasterPrefab, ShotPoint.position, Quaternion.identity);
				gameObject.transform.parent = ShotPoint;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localEulerAngles = Vector3.zero;
				_model.SetArmor(MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(ArmorType.Default, 0));
			}
		}

		protected override void OnPlayerAreaExit(Transform item)
		{
			if (!(StateMachine.CurrentState is RunAwayState) && !(StateMachine.CurrentState is DeathState) && AttackTarget != null && AttackTarget.Equals(item))
			{
				Vector3 position = AttackTarget.position;
				UpdateTarget();
				if (AttackTarget == null)
				{
					StateMachine.ChangeState<MoveState>(new object[2] { position, Model.RunSpeed });
				}
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();
			if (!(StateMachine.CurrentState is DeathState))
			{
				StateMachine.ChangeState<DeathState>(new object[0]);
				MonoCommand.ExecuteOn<DestroyUnitCommand>(GameObject, new object[2] { Transform, 0.2f });
				LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
				if (currentGameLevel.Scene == typeof(DesertBase1Multiplayer) || currentGameLevel.Scene == typeof(EastSet1Multiplayer))
				{
					GameController gameController = Object.FindObjectOfType<GameController>();
					gameController.OnResultmultiplayerWin();
				}
			}
		}

		public void OnHide()
		{
			if (!(StateMachine.CurrentState is DeathState) && !(StateMachine.CurrentState is HideState))
			{
				if (!CanHide() && !(StateMachine.CurrentState is ChaseState))
				{
					StateMachine.ChangeState<ChaseState>(new object[4] { AttackTarget, Model, MoveToWarPointChance, WaitInWarPointTime });
				}
				else if (CanHide())
				{
					StateMachine.ChangeState<HideState>(new object[1] { _hideTime });
				}
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (!Model.IsDead() && !(StateMachine.CurrentState is HideState) && !(StateMachine.CurrentState is RunAwayState))
			{
				if (AttackTarget == null)
				{
					UpdateTarget();
				}
				else if (_nextRetarget < Time.time)
				{
					UpdateTarget();
					_nextRetarget += _retargetTimeDelay;
				}
				if (AttackTarget != null)
				{
					OnHide();
				}
			}
		}

		private Transform ChooseTarget()
		{
			Transform result = null;
			if (AttackTarget == null)
			{
				if (DamageSystem.UnitLastDamage != null)
				{
					return DamageSystem.UnitLastDamage;
				}
				if (DamageSystem.UnitMaxDamage != null)
				{
					return DamageSystem.UnitMaxDamage;
				}
			}
			if (ViewArea.Items.Count > 1)
			{
				float num = float.MaxValue;
				{
					foreach (Transform item in ViewArea.Items)
					{
						if (num > Vector3.SqrMagnitude(item.position - Transform.position))
						{
							num = Vector3.SqrMagnitude(item.position - Transform.position);
							result = item;
						}
					}
					return result;
				}
			}
			if (ViewArea.Items.Count == 1)
			{
				return ViewArea.Items[0];
			}
			return result;
		}

		private void UpdateTarget()
		{
			Transform transform = ChooseTarget();
			if (transform != AttackTarget)
			{
				AttackTarget = transform;
			}
		}

		public override void Next(MonoState previousState)
		{
			if (previousState is MoveState || previousState is MovePathState)
			{
				Patrol();
			}
			if (previousState is ChaseState && AttackTarget == null)
			{
				Patrol();
			}
		}

		public void Patrol()
		{
			if (AttackTarget != null)
			{
				return;
			}
			if (MovePath != null && MovePathIndex < MovePath.GetPointCount())
			{
				Transform nextPoint = MovePath.GetNextPoint(MovePathIndex);
				if (nextPoint != null)
				{
					StateMachine.ChangeState<MovePathState>(new object[0]);
					return;
				}
			}
			if (PatrolPath == null)
			{
				StateMachine.ChangeState<IdleState>(new object[0]);
			}
			else
			{
				StateMachine.ChangeState<PatrolState>(new object[0]);
			}
		}
	}
}
