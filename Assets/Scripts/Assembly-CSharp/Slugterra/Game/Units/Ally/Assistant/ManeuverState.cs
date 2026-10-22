using Pathfinding;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	internal class ManeuverState : FollowerState
	{
		private Transform _finishPoint;

		private bool _isCalculated;

		private float _allyAreaRadius;

		private const float PRIVATE_ELI_RADIUS = 2f;

		private const float PRIVATE_ENEMY_RADIUS = 2f;

		protected override void OnStart(object[] args)
		{
			_finishPoint = new GameObject("FinishPoint").transform;
			Component allyArea = Controller.AllyArea;
			_allyAreaRadius = allyArea.GetComponent<SphereCollider>().radius;
		}

		private void OnPathComplete(Path p)
		{
			if (IsFinished)
			{
				return;
			}
			if (Controller.PathFinder == null || Controller == null)
			{
				Finish();
				return;
			}
			_finishPoint.position = ((RandomPath)p).endPoint;
			Vector3 position = Controller.PlayerTransform.position;
			Ray ray = new Ray(Controller.Transform.position + Vector3.up * 0.5f, _finishPoint.position - Controller.Transform.position + Vector3.up * 0.5f);
			int num = LayerMask.NameToLayer("AllyLayer");
			int layerMask = 1 << num;
			bool flag = Vector3.SqrMagnitude(position - _finishPoint.position) < _allyAreaRadius * _allyAreaRadius;
			bool flag2 = PhysicsFacade.Raycast(ray, float.PositiveInfinity, layerMask);
			bool flag3 = Vector3.SqrMagnitude(position - _finishPoint.position) < 4f;
			bool flag4 = !(Controller.AttackTarget == null) && Vector3.SqrMagnitude(_finishPoint.position - Controller.AttackTarget.position) < 4f;
			if (flag && !flag2 && (!flag3 & !flag4))
			{
				Setup(Controller.Model.WalkSpeed, _finishPoint, 0.45f);
				_isCalculated = true;
			}
		}

		protected override void OnUpdate()
		{
			if (!_isCalculated)
			{
				FleePath fleePath = FleePath.Construct(Controller.Transform.position, Controller.PlayerTransform.position, 2000, OnPathComplete);
				fleePath.spread = 1000;
				fleePath.aimStrength = 0.01f;
				fleePath.aim = Vector3.zero;
				AstarPath.StartPath(fleePath);
			}
			if (_isCalculated)
			{
				MoveAnimation();
				CheckDistance(true);
			}
		}

		protected override void OnFinish()
		{
			if (_finishPoint != null)
			{
				Object.Destroy(_finishPoint.gameObject);
			}
		}
	}
}
