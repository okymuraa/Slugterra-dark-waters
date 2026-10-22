using AppsMinistry.Core;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	internal class HideState : FollowerState
	{
		private float _allyAreaRadius;

		protected override void OnStart(object[] args)
		{
			Component allyArea = Controller.AllyArea;
			_allyAreaRadius = allyArea.GetComponent<SphereCollider>().radius;
			CoverNode coverNode = null;
			coverNode = MonoSingleton<CoverSystem>.Instance.GetNearestNode(Controller.Transform, Controller.DamageSystem.UnitLastDamage, 8f);
			if (coverNode != null)
			{
				Transform controlPoint = coverNode.GetControlPoint(Controller.DamageSystem.UnitLastDamage);
				if (_allyAreaRadius * _allyAreaRadius > Vector3.SqrMagnitude(controlPoint.position - Controller.PlayerTransform.position))
				{
					Setup(Controller.Model.WalkSpeed, controlPoint, 0.65f);
				}
				else
				{
					Finish();
				}
			}
			else
			{
				Finish();
			}
		}

		protected override void OnUpdate()
		{
			MoveAnimation();
			CheckDistance();
		}
	}
}
