using System.Collections.Generic;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public abstract class InputStrategy : IInputStrategy
	{
		private PlayerController _controller;

		public PlayerController Controller
		{
			get
			{
				return _controller;
			}
		}

		public InputStrategy(PlayerController controller)
		{
			_controller = controller;
		}

		public virtual void OnPress(Vector3 position)
		{
		}

		public virtual void OnTap(Vector3 position, float actionTime)
		{
		}

		public virtual void OnDoubleTap(Vector3 position)
		{
		}

		public virtual void OnSwipe(Vector2 direction)
		{
		}

		public virtual void OnRoll(Vector2 direction, Vector2 move)
		{
		}

		public virtual void OnRelease(Vector3 direction, float actionTime)
		{
		}

		public virtual void OnUseSlug(SlugType slug)
		{
		}

		public virtual void OnMove(Vector2 axises)
		{
		}

		public virtual void OnMoveEnd()
		{
		}

		public virtual void OnRotate(Vector2 axises)
		{
		}

		public virtual void OnRotateEnd()
		{
		}

		public virtual void OnTouch(Vector3 position, float actionTime)
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnNextTarget()
		{
			if (Controller == null) return;
			List<Transform> list = Controller.BuildTargetsPriorityList();
			List<Transform> list2 = list.FindAll(Controller.CheckTargetInFOV);
			if (list2.Count == 0)
			{
				list2 = list;
			}
			if (list2.Count == 0)
			{
				return;
			}
			int num = -1;
			if (Controller.AttackTarget != null)
			{
				num = list2.IndexOf(Controller.AttackTarget);
			}
			int nextIndex = (num + 1) % list2.Count;
			Controller.AttackTarget = list2[nextIndex];
			Controller.UseAutoattack = true;
			Controller.RotateToTarget = true;
			Controller.IsIgnorEnemy = false;
		}

		public virtual void OnPrevTarget()
		{
			if (Controller == null) return;
			List<Transform> list = Controller.BuildTargetsPriorityList();
			List<Transform> list2 = list.FindAll(Controller.CheckTargetInFOV);
			if (list2.Count == 0)
			{
				list2 = list;
			}
			if (list2.Count == 0)
			{
				return;
			}
			int num = list2.Count;
			if (Controller.AttackTarget != null)
			{
				num = list2.IndexOf(Controller.AttackTarget);
				if (num < 0)
				{
					num = list2.Count;
				}
			}
			int prevIndex = (num - 1 + list2.Count) % list2.Count;
			Controller.AttackTarget = list2[prevIndex];
			Controller.UseAutoattack = true;
			Controller.RotateToTarget = true;
			Controller.IsIgnorEnemy = false;
		}
	}
}
