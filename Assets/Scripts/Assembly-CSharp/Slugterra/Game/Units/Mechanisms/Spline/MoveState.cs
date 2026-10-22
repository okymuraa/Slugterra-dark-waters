using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Units.Mechanisms;
using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms.Spline
{
	internal class MoveState : TeamState, IObserver
	{
		protected override void OnStart(object[] args)
		{
			foreach (AnimationState item in Controller.Animation)
			{
				item.speed = Controller.Model.Speed;
			}
			Controller.Animator.SetFloat("Speed", Controller.Model.SpeedAnimationTrailer);
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetFloat("Speed", Controller.Model.SpeedAnimationFandango);
			}
			Controller.Model.AddObserver(this);
		}

		public void OnObjectChanged(object observable)
		{
			if (!(observable is Mechanism))
			{
				return;
			}
			Mechanism mechanism = observable as Mechanism;
			if (mechanism.Brake)
			{
				foreach (AnimationState item in Controller.Animation)
				{
					item.speed = 0f;
				}
				Controller.Animator.SetFloat("Speed", 0f);
				if (Controller.FernandoAnimator != null)
				{
					Controller.FernandoAnimator.SetFloat("Speed", 0f);
				}
				return;
			}
			foreach (AnimationState item2 in Controller.Animation)
			{
				item2.speed = Controller.Model.Speed;
			}
			Controller.Animator.SetFloat("Speed", Controller.Model.SpeedAnimationTrailer);
			if (Controller.FernandoAnimator != null)
			{
				Controller.FernandoAnimator.SetFloat("Speed", Controller.Model.SpeedAnimationFandango);
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			Controller.Model.RemoveObserver(this);
		}
	}
}
