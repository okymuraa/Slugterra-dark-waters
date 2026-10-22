using System;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class TPActionInputStrategy : InputStrategy
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

		private float MinimumX = 15f;

		private float MaximumX = 25f;

		public TPActionInputStrategy(PlayerController controller)
			: base(controller)
		{
		}

		public override void OnSwipe(Vector2 direction)
		{
			Debug.Log("ActionInputStrategy::OnSwipe");
		}

		public override void OnRoll(Vector2 direction, Vector2 move)
		{
			Debug.Log("ActionInputStrategy::Controller.CanSwipe = " + Controller.CanSwipe);
			if (Controller.CanSwipe)
			{
				if (Controller.StateMachine.CurrentState is IdleState || Controller.StateMachine.CurrentState is MoveState)
				{
					Controller.StateMachine.ChangeState<RollState>(new object[2] { direction, move });
					Debug.Log("ActionInputStrategy::ChangeState<RollState>");
				}
				else if (Controller.StateMachine.CurrentState is RollState)
				{
					(Controller.StateMachine.CurrentState as RollState).OnSwipe(direction);
				}
				else if (Controller.StateMachine.CurrentState == null)
				{
					Controller.StateMachine.ChangeState<RollState>(new object[2] { direction, move });
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
			Controller.MoveAxises = axises;
		}

		public override void OnMoveEnd()
		{
			Controller.MoveAxises = Vector3.zero;
		}

		public override void OnRotate(Vector2 axises)
		{
		}

		private Quaternion ClampRotationAroundXAxis(Quaternion q)
		{
			q.x /= q.w;
			q.y /= q.w;
			q.z /= q.w;
			q.w = 1f;
			float value = 114.59156f * Mathf.Atan(q.x);
			value = Mathf.Clamp(value, MinimumX, MaximumX);
			q.x = Mathf.Tan((float)Math.PI / 360f * value);
			return q;
		}

		public override void OnRotateEnd()
		{
		}
	}
}
