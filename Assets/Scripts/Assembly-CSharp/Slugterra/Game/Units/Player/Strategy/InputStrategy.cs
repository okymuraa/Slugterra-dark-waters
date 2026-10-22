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
		}

		public virtual void OnPrevTarget()
		{
		}
	}
}
