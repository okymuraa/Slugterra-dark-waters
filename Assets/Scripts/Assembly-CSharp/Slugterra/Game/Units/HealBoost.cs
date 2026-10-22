using UnityEngine;

namespace Slugterra.Game.Units
{
	public class HealBoost : BoostEffect
	{
		private float _finishTime;

		private float _nextHelthUpdate;

		private const float SPAN = 0.1f;

		private float _lastUpdate;

		private float _appliedHeal;

		private float _fullHeal;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			float time = Time.time;
			_finishTime = time + boost.HealDuration;
			_nextHelthUpdate = time;
			_lastUpdate = time;
			_fullHeal = boost.HealDuration * boost.HealthPerSecond;
		}

		private void Update()
		{
			if (_finishTime < Time.time || controller == null || controller.Model.Health <= 0f)
			{
				controller.Model.Health += _fullHeal - _appliedHeal;
				Finish();
			}
			else if (Time.time > _nextHelthUpdate)
			{
				float time = Time.time;
				float num = boost.HealthPerSecond * (time - _lastUpdate);
				controller.Model.Health += num;
				_appliedHeal += num;
				_nextHelthUpdate = time + 0.1f;
				_lastUpdate = time;
			}
		}
	}
}
