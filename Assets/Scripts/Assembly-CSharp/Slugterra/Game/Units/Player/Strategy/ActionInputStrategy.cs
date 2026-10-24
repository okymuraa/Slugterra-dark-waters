using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Input;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class ActionInputStrategy : InputStrategy
	{
		private enum TapOn
		{
			eNone = 0,
			eTerrain = 1,
			eEnemy = 2
		}

		private TapOn _tapOn;

		private Vector3 _terrainPosition;

		private Transform _enemy;

		private WalkAnimationCommand _walkCommand;

		public ActionInputStrategy(PlayerController controller)
			: base(controller)
		{
		}

		public override void OnPress(Vector3 position)
		{
			_tapOn = TapOn.eNone;
			if (MonoSingleton<GameCollisionMap>.Instance.IsTapOnEnemy(position, out _enemy))
			{
				_tapOn = TapOn.eEnemy;
			}
			else if (MonoSingleton<GameCollisionMap>.Instance.IsTapOnTerrain(position, out _terrainPosition))
			{
				_tapOn = TapOn.eTerrain;
			}
			bool flag = false;
			if (_tapOn == TapOn.eTerrain)
			{
				if (Controller.PathFinder != null)
				{
					Controller.PathFinder.target.position = _terrainPosition;
				}
			}
			else if (_tapOn == TapOn.eEnemy)
			{
				Controller.AttackTarget = _enemy;
				Controller.UseAutoattack = true;
				Controller.IsIgnorEnemy = false;
				Controller.RotateToTarget = true;
				flag = Vector3.SqrMagnitude(Controller.AttackTarget.position - Controller.Transform.position) > Mathf.Pow((Controller.Model as Humanoid).Blaster.Range, 2f);
				if (flag && Controller.PathFinder != null)
				{
					Controller.PathFinder.target.position = _enemy.position;
				}
			}
			if (Controller.StateMachine.CurrentState is RollState)
			{
				(Controller.StateMachine.CurrentState as RollState).OnTap();
				return;
			}
			Controller.Model.Run();
			if (_tapOn == TapOn.eTerrain)
			{
				Controller.StateMachine.ChangeState<MoveState>(new object[1] { 0.5f });
			}
			else if (_tapOn == TapOn.eEnemy && flag)
			{
				Controller.StateMachine.ChangeState<MoveState>(new object[1] { (Controller.Model as Humanoid).Blaster.Range - 2f });
			}
			Controller.IsIgnorEnemy = false;
		}

		public override void OnDoubleTap(Vector3 position)
		{
			float num = 0.2f;
			if (_tapOn == TapOn.eTerrain)
			{
				if (Controller.PathFinder != null)
				{
					Controller.PathFinder.target.position = _terrainPosition;
				}
				Controller.StateMachine.ChangeState<MoveState>(new object[1] { num });
			}
			if (_tapOn == TapOn.eEnemy && Controller.AttackTarget == _enemy)
			{
				Controller.AttackTarget = null;
			}
		}

		public override void OnSwipe(Vector2 direction)
		{
			if (Controller.CanSwipe)
			{
				if (Controller.StateMachine.CurrentState is IdleState || Controller.StateMachine.CurrentState is MoveState)
				{
					Controller.StateMachine.ChangeState<RollState>(new object[1] { direction });
				}
				else if (Controller.StateMachine.CurrentState is RollState)
				{
					(Controller.StateMachine.CurrentState as RollState).OnSwipe(direction);
				}
				else if (Controller.StateMachine.CurrentState == null)
				{
					Controller.StateMachine.ChangeState<RollState>(new object[1] { direction });
				}
			}
		}

		public override void OnUseSlug(SlugType slugType)
		{
			if (!Controller.IsReloadSlug)
			{
				Slug slug = (Controller.Model as Humanoid).GetSlug(slugType);
				if (Controller.IsCanUseSlug(slug) && slug.Available)
				{
					Controller.OnReloadSlug(slugType);
				}
			}
		}

		public override void OnMove(Vector2 axises)
		{
			Controller.Model.Run();
			if (Controller.Model.CurrentSpeed != 0f)
			{
				if (_walkCommand == null)
				{
					_walkCommand = MonoCommand.ExecuteOn<WalkAnimationCommand>(Controller.GameObject, new object[1] { Controller });
				}
				if (_walkCommand != null && !_walkCommand.enabled)
				{
					_walkCommand.enabled = true;
				}
				if (Controller.StateMachine.CurrentState != null)
				{
					Controller.StateMachine.Terminate();
				}
				if (Controller.PathFinder != null)
				{
					Vector3 worldDir = new Vector3(axises.x, 0f, axises.y);
					if (Camera.main != null)
					{
						Vector3 camForward = Camera.main.transform.forward;
						camForward.y = 0f;
						camForward.Normalize();
						Vector3 camRight = Camera.main.transform.right;
						camRight.y = 0f;
						camRight.Normalize();
						worldDir = camRight * axises.x + camForward * axises.y;
					}
					Controller.PathFinder.target.position = Controller.Transform.position + worldDir * 3f;
					if (!Controller.PathFinder.target.gameObject.activeSelf)
					{
						Controller.PathFinder.target.gameObject.SetActive(true);
					}
					Controller.PathFinder.canSearch = true;
					Controller.PathFinder.canMove = true;
					Controller.PathFinder.speed = Controller.Model.CurrentSpeed * Mathf.Clamp01(axises.magnitude);
				}
			}
		}

		public override void OnMoveEnd()
		{
			Controller.StateMachine.ChangeState<IdleState>(new object[0]);
			if (_walkCommand != null && _walkCommand.enabled)
			{
				_walkCommand.enabled = false;
			}
			if (Controller.PathFinder != null)
			{
				Controller.PathFinder.canMove = false;
				Controller.PathFinder.target.position = Controller.Transform.position;
				if (Controller.PathFinder.target.gameObject.activeSelf)
				{
					Controller.PathFinder.target.gameObject.SetActive(false);
				}
			}
		}

		public override void OnNextTarget()
		{
			if (Controller.AttackTarget != null)
			{
				List<Transform> list = Controller.BuildTargetsPriorityList();
				List<Transform> list2 = list.FindAll(Controller.CheckTargetInFOV);
				int num = list2.IndexOf(Controller.AttackTarget);
				Transform transform = null;
				if (num < list2.Count - 1)
				{
					transform = list2[num + 1];
				}
				if (transform != null)
				{
					Controller.AttackTarget = transform;
				}
			}
		}

		public override void OnPrevTarget()
		{
			if (Controller.AttackTarget != null)
			{
				List<Transform> list = Controller.BuildTargetsPriorityList();
				List<Transform> list2 = list.FindAll(Controller.CheckTargetInFOV);
				int num = list2.IndexOf(Controller.AttackTarget);
				Transform transform = null;
				if (num > 0)
				{
					transform = list2[num - 1];
				}
				if (transform != null)
				{
					Controller.AttackTarget = transform;
				}
			}
		}

		public override void OnTouch(Vector3 position, float actionTime)
		{
			if (!MonoSingleton<GameCollisionMap>.Instance.IsTapOnTerrain(position, out _terrainPosition))
			{
				return;
			}
			if (Controller.PathFinder != null)
			{
				Controller.PathFinder.target.position = _terrainPosition;
				if (!Controller.PathFinder.target.gameObject.activeSelf)
				{
					Controller.PathFinder.target.gameObject.SetActive(true);
				}
				Controller.PathFinder.canSearch = true;
				Controller.PathFinder.canMove = true;
				Controller.PathFinder.speed = Controller.Model.CurrentSpeed;
			}
			if (_walkCommand == null)
			{
				_walkCommand = MonoCommand.ExecuteOn<WalkAnimationCommand>(Controller.GameObject, new object[1] { Controller });
			}
			if (_walkCommand != null && !_walkCommand.enabled)
			{
				_walkCommand.enabled = true;
			}
			if (Controller.StateMachine.CurrentState != null && !(Controller.StateMachine.CurrentState is MoveState))
			{
				Controller.StateMachine.Terminate();
			}
			Controller.Model.Run();
		}

		public override void OnRelease(Vector3 direction, float actionTime)
		{
			if (actionTime > 0.2f)
			{
				Controller.StateMachine.ChangeState<IdleState>(new object[0]);
				if (_walkCommand != null && _walkCommand.enabled)
				{
					_walkCommand.enabled = false;
				}
				if (Controller.PathFinder != null)
				{
					Controller.PathFinder.canMove = false;
					if (Controller.PathFinder.target.gameObject.activeSelf)
					{
						Controller.PathFinder.target.gameObject.SetActive(false);
					}
				}
			}
		}
	}
}
