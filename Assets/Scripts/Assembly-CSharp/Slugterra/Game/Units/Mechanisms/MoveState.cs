using System.Collections.Generic;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Units.Mechanisms;
using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms
{
	internal class MoveState : WagonState, IObserver
	{
		private List<Transform> _routeNodes;

		private const float STOP_DISTANCE = 1f;

		protected override void OnStart(object[] args)
		{
			_routeNodes = (List<Transform>)args[0];
			if (_routeNodes == null)
			{
				Debug.LogWarning("RouteNodes is unexpectedly equal to null.");
				Finish();
			}
			Controller.PathFinder.speed = Controller.Model.Speed;
			Controller.PathFinder.turningSpeed = 0.5f;
			Controller.PathFinder.canMove = true;
			Controller.Animator.SetFloat("Speed", Controller.PathFinder.speed);
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetFloat("Speed", Controller.PathFinder.speed);
			}
			Controller.Model.AddObserver(this);
		}

		protected override void OnUpdate()
		{
			float num = Vector3.SqrMagnitude(Controller.PathFinder.target.position - Controller.Transform.position);
			if (!(num < 1f))
			{
				return;
			}
			if (_routeNodes.Count != 0)
			{
				if (_routeNodes[0] != null)
				{
					Controller.PathFinder.target.position = _routeNodes[0].position;
					Controller.PathFinder.SearchPath();
				}
				_routeNodes.RemoveAt(0);
			}
			else if (num <= 1f)
			{
				Finish();
			}
		}

		protected override void OnFinish()
		{
			Controller.Animator.SetFloat("Speed", 0f);
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetFloat("Speed", 0f);
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Mechanism)
			{
				Mechanism mechanism = observable as Mechanism;
				if (mechanism.Brake)
				{
					Controller.PathFinder.speed = 0f;
					Controller.Animator.SetFloat("Speed", 0f);
				}
				else
				{
					Controller.PathFinder.speed = Controller.Model.Speed;
					Controller.Animator.SetFloat("Speed", Controller.Model.Speed);
				}
			}
		}
	}
}
