using UnityEngine;

namespace Slugterra.Game.Units
{
	public class RapidAttack : BoostEffect
	{
		private float _finishTime;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_finishTime = Time.time + boost.AttackDuration;
			controller.Model.AddAttackSpeedCoeficient(GetType(), boost.AttackRatio);
		}

		private void Update()
		{
			if (_finishTime < Time.time)
			{
				controller.Model.RemoveAttackSpeedCoeficient(GetType());
				Finish();
			}
		}
	}
}
