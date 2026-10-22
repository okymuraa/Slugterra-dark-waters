using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class FollowState : FollowerState
	{
		private const float RUN_DISTANCE = 6f;

		private Transform _targetTransform;

		private Vector3 _position;

		protected override void OnStart(object[] args)
		{
			_targetTransform = (Transform)args[0];
			float radius = Controller.AllyArea.GetComponent<SphereCollider>().radius;
			Vector2 vector;
			do
			{
				vector = Random.insideUnitCircle * radius * 0.9f;
			}
			while (vector.magnitude < 1.5f);
			Controller.PathFinder.repathRate = 0.5f;
			Controller.PathFinder.canSearch = true;
			_offset = new Vector3(vector.x, 0f, vector.y);
			Setup(Controller.Model.RunSpeed, _targetTransform, 0.5f);
		}

		protected override void OnUpdate()
		{
			MoveAnimation();
			CheckDistance(true, true);
		}
	}
}
