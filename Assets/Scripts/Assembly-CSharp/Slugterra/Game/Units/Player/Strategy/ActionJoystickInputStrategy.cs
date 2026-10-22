using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Input;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class ActionJoystickInputStrategy : InputStrategy
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

		private ETCJoystick _joystick;

		private WalkAnimationCommand _walkCommand;

		public ActionJoystickInputStrategy(PlayerController controller)
			: base(controller)
		{
		}

		public override void OnMove(Vector2 axises)
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
			Controller.PathFinder.target.position = Controller.Transform.position + new Vector3(axises.x, 0f, axises.y) * 3f;
			Controller.PathFinder.canSearch = true;
			Controller.PathFinder.canMove = true;
			Controller.Model.Run();
			Controller.PathFinder.speed = Controller.Model.CurrentSpeed * axises.magnitude;
		}

		public override void OnMoveEnd()
		{
			Controller.StateMachine.ChangeState<IdleState>(new object[0]);
			if (_walkCommand != null && _walkCommand.enabled)
			{
				_walkCommand.enabled = false;
			}
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
				Controller.PathFinder.target.position = _terrainPosition;
			}
			else if (_tapOn == TapOn.eEnemy && Controller.AttackTarget != _enemy)
			{
				Controller.AttackTarget = _enemy;
				flag = Vector3.SqrMagnitude(Controller.AttackTarget.position - Controller.Transform.position) > Mathf.Pow((Controller.Model as Humanoid).Blaster.Range, 2f);
				if (flag)
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
				Controller.PathFinder.target.position = _terrainPosition;
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
	}
}
