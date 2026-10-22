using UnityEngine;

namespace Slugterra.Game.Units
{
	public class SpeedMotion : BoostEffect
	{
		private float _finishTime;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_finishTime = Time.time + boost.SpeedDuration;
			controller.Model.AddSpeedCoeficient(GetType(), boost.SpeedRatio);
		}

		private void Update()
		{
			if (_finishTime < Time.time)
			{
				controller.Model.RemoveSpeedCoeficient(GetType());
				Finish();
			}
		}
	}
}
