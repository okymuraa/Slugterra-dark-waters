using UnityEngine;

namespace Slugterra.Game.Units
{
	public class ArmorBoost : BoostEffect
	{
		private float _finishTime;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_finishTime = Time.time + boost.DamageDuration;
			controller.Model.AddDamageCoeficient(GetType(), boost.DamageRatio);
		}

		private void Update()
		{
			if (_finishTime < Time.time)
			{
				controller.Model.RemoveDamageCoeficient(GetType());
				Finish();
			}
		}
	}
}
