using System;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Duel;
using Slugterra.Game.Input;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class DuelInputStrategy : InputStrategy
	{
		private PlayerDuelPoint _duelPoint;

		private bool _isDoubleTap;

		private InGameModel _inGameModel;

		private InGameModel InGameModel
		{
			get
			{
				if (_inGameModel == null)
				{
					InGameController inGameController = UnityEngine.Object.FindObjectOfType<InGameController>();
					if (inGameController != null)
					{
						_inGameModel = inGameController.Model;
					}
				}
				return _inGameModel;
			}
		}

		public DuelInputStrategy(PlayerController controller)
			: base(controller)
		{
			_isDoubleTap = false;
			_duelPoint = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.GetNearestPoint(controller.Transform.position);
			MoveToDuelPoint(controller.Model.RunSpeed);
		}

		public override void OnTap(Vector3 position, float actionTime)
		{
			if (Controller.StateMachine.CurrentState is IdleState || !(Controller.StateMachine.CurrentState != null))
			{
				_isDoubleTap = false;
				if (MonoSingleton<GameCollisionMap>.Instance.GetScreenWidth() / 2f > position.x)
				{
					_duelPoint = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.GetPrevPoint();
				}
				else
				{
					_duelPoint = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.GetNextPoint();
				}
				if (InGameModel != null)
				{
					InGameModel.IsLeftArrow = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.IsPrev();
					InGameModel.IsRightArrow = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.IsNext();
				}
				if (!(_duelPoint == null))
				{
					MoveToDuelPoint(Controller.Model.WalkSpeed);
				}
			}
		}

		public override void OnDoubleTap(Vector3 position)
		{
			if (Controller.StateMachine.CurrentState is IdleState || !(Controller.StateMachine.CurrentState != null))
			{
				if (_isDoubleTap)
				{
					_isDoubleTap = true;
				}
				if (MonoSingleton<GameCollisionMap>.Instance.GetScreenWidth() / 2f > position.x)
				{
					_duelPoint = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.GetPrevPoint();
				}
				else
				{
					_duelPoint = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.GetNextPoint();
				}
				if (InGameModel != null)
				{
					InGameModel.IsLeftArrow = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.IsPrev();
					InGameModel.IsRightArrow = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.IsNext();
				}
				if (!(_duelPoint == null))
				{
					MoveToDuelPoint(Controller.Model.RunSpeed);
				}
			}
		}

		public override void OnSwipe(Vector2 direction)
		{
			if (!Controller.CanSwipe)
			{
				return;
			}
			float y = Camera.main.transform.eulerAngles.y;
			y = y / 180f * (float)Math.PI;
			if (direction.x > 0f)
			{
				_duelPoint = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.GetNextPoint();
				direction.y = Mathf.Cos(y + (float)Math.PI / 2f);
				direction.x = Mathf.Sin(y + (float)Math.PI / 2f);
			}
			else
			{
				_duelPoint = MonoSingleton<DuelPointSystem>.Instance.PlayerPointManager.GetPrevPoint();
				direction.y = Mathf.Cos(y - (float)Math.PI / 2f);
				direction.x = Mathf.Sin(y - (float)Math.PI / 2f);
			}
			if (!(_duelPoint == null))
			{
				Controller.PathFinder.target.position = _duelPoint.transform.position;
				if (Controller.StateMachine.CurrentState is IdleState || Controller.StateMachine.CurrentState is MoveState)
				{
					Controller.StateMachine.ChangeState<RollState>(new object[1] { direction });
				}
			}
		}

		private void MoveToDuelPoint(float speed)
		{
			Controller.PathFinder.target.position = _duelPoint.transform.position;
			Controller.Model.CurrentSpeed = speed;
			Controller.StateMachine.ChangeState<MoveState>(new object[1] { 0.5f });
		}

		public override void OnUseSlug(SlugType slugType)
		{
			if (MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is FightState)
			{
				Slug slug = (Controller.Model as Humanoid).GetSlug(slugType);
				if (Controller.IsCanUseSlug(slug) && slug.Available)
				{
					Controller.SetNextSlug(slugType);
				}
			}
		}
	}
}
