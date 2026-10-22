using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public interface IInputStrategy
	{
		void OnPress(Vector3 position);

		void OnTap(Vector3 position, float actionTime);

		void OnDoubleTap(Vector3 position);

		void OnSwipe(Vector2 direction);

		void OnRoll(Vector2 direction, Vector2 move);

		void OnRelease(Vector3 position, float actionTime);

		void OnUseSlug(SlugType slug);

		void OnMove(Vector2 axies);

		void OnMoveEnd();

		void OnRotate(Vector2 axies);

		void OnRotateEnd();

		void OnUpdate();

		void OnNextTarget();

		void OnPrevTarget();

		void OnTouch(Vector3 position, float actionTime);
	}
}
