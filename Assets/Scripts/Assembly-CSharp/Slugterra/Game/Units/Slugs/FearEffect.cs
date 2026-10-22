using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Pathfinding;
using Slugterra.Domain.Game.UI;
using Slugterra.Game.Duel;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Animals;
using Slugterra.Game.Units.Enemy.Deadweed;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class FearEffect : UnitEffect
	{
		private float _finishTime;

		private UnitController _controller;

		private GameObject _effect;

		private Transform _finishPoint;

		private MoveCommand _moveCommand;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_controller = (UnitController)args[0];
			if (_controller == null || _controller.IsFallDown)
			{
				Finish();
				return;
			}
			_finishTime = (float)args[1] + Time.time;
			if (_controller.StateMachine.CurrentState != null)
			{
				_controller.StateMachine.CurrentState.enabled = false;
			}
			_controller.RotateToTarget = false;
			_controller.AttackTarget = null;
			_controller.Animator.transform.localEulerAngles = Vector3.zero;
			_controller.Animator.SetBool("Fear", true);
			_controller.enabled = false;
			if (_controller is EliController)
			{
				(_controller as EliController).DisableControl(GetType());
			}
			if (_effect == null)
			{
				GameObject gameObject = args[2] as GameObject;
				if (gameObject != null)
				{
					_effect = Object.Instantiate(gameObject, transform.position, Quaternion.identity);
					_effect.transform.parent = transform;
				}
			}
			if (_controller.NewVersionAI)
			{
				_controller.MyBehavior.Fear();
			}
			else
			{
				FindPoint();
			}
		}

		private void FindPoint()
		{
			if (_controller.AttackTarget != null)
			{
				FleePath fleePath = FleePath.Construct(_controller.Transform.position, _controller.AttackTarget.position, 10000, OnPathComplete);
				fleePath.spread = 2000;
				fleePath.aimStrength = 0.5f;
				AstarPath.StartPath(fleePath);
			}
			else
			{
				RandomPath randomPath = RandomPath.Construct(_controller.Transform.position, 10000, OnPathComplete);
				randomPath.spread = 2000;
				randomPath.aimStrength = 0.5f;
				randomPath.aim = Vector3.zero;
				AstarPath.StartPath(randomPath);
			}
		}

		private void OnPathComplete(Path p)
		{
			if (!(_controller.PathFinder == null) && !(_controller == null) && !IsFinished)
			{
				_finishPoint = new GameObject("FinishPoint").transform;
				_finishPoint.position = ((RandomPath)p).endPoint;
				_controller.Model.Run();
				_moveCommand = MonoCommand.ExecuteOn<MoveCommand>(gameObject, new object[3] { _controller, _finishPoint, 0.45f });
				_moveCommand.AsyncToken.AddResponder(new Responder<MoveCommand>(OnMoveFinish));
			}
		}

		private void OnMoveFinish(MoveCommand moveCommand)
		{
			if (!IsFinished)
			{
				FindPoint();
			}
		}

		private void Update()
		{
			_controller.AttackTarget = null;
			if (_controller == null || _controller.Model.IsDead())
			{
				Finish();
				return;
			}
			if (_finishTime <= Time.time)
			{
				Finish();
				return;
			}
			if (_controller is AnimalController || _controller is DeadweedController)
			{
				_controller.Animator.SetFloat("Speed", _controller.PathFinder.MoveVelocity.sqrMagnitude);
				return;
			}
			_controller.Animator.SetFloat("VelocityX", _controller.PathFinder.MoveVelocity.x);
			_controller.Animator.SetFloat("VelocityY", _controller.PathFinder.MoveVelocity.z);
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_moveCommand != null)
			{
				_moveCommand.Finish();
				_moveCommand = null;
			}
			if (_controller != null)
			{
				_controller.PathFinder.target.position = _controller.Transform.position;
				_controller.Animator.SetFloat("VelocityX", 0f);
				_controller.Animator.SetFloat("VelocityY", 0f);
				_controller.Animator.SetBool("Fear", false);
				_controller.enabled = true;
				if (_controller.NewVersionAI)
				{
					_controller.MyBehavior.UnFear();
				}
				if (_controller.StateMachine.CurrentState != null)
				{
					_controller.StateMachine.CurrentState.enabled = true;
				}
				if (_controller is EliController)
				{
					(_controller as EliController).EnableControl(GetType());
				}
				_controller = null;
			}
			if (_finishPoint != null)
			{
				Object.Destroy(_finishPoint.gameObject);
			}
			if (_effect != null)
			{
				Object.Destroy(_effect);
			}
			GameController gameController = Object.FindObjectOfType<GameController>();
			if (gameController.LevelType == LevelType.Arena)
			{
				MonoSingleton<DuelManager>.Instance.SetBossTarget();
			}
		}
	}
}
