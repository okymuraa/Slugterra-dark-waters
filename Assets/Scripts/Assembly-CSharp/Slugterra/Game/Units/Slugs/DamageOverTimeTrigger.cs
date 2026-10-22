using System.Collections.Generic;
using Slugterra.Game.Triggers;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	internal class DamageOverTimeTrigger : Trigger
	{
		public float DamagePerSecond = 1f;

		private float _nextHelthUpdate;

		private const float SPAN = 0.1f;

		private List<UnitController> _unitsUnderDOT = new List<UnitController>();

		protected override void OnStart()
		{
			base.OnStart();
			Mode = TriggerMode.OnStay;
			Delay = 0f;
			_nextHelthUpdate = Time.time + 0.1f;
		}

		protected override void OnTriggerEnter(Collider collider)
		{
			base.OnTriggerEnter(collider);
			if (ActivateByTriggers(collider))
			{
				_unitsUnderDOT.Add(collider.GetComponent<UnitController>());
			}
		}

		protected override void OnTriggerExit(Collider collider)
		{
			base.OnTriggerExit(collider);
			if (ActivateByTriggers(collider))
			{
				_unitsUnderDOT.Remove(collider.GetComponent<UnitController>());
			}
		}

		protected override void OnTrigger()
		{
			base.OnTrigger();
			if (!(Time.time > _nextHelthUpdate))
			{
				return;
			}
			float time = Time.time;
			float damage = DamagePerSecond * 0.1f;
			foreach (UnitController item in _unitsUnderDOT)
			{
				if (item != null)
				{
					item.OnDamage(damage);
				}
			}
			_nextHelthUpdate = time + 0.1f;
		}
	}
}
