using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class DamageOverTimeAbility : SlugAbility
	{
		[HideInInspector]
		[SerializeField]
		private List<float> _effectTimes;

		[HideInInspector]
		[SerializeField]
		private List<float> _damagePerSeconds;

		public List<float> EffectTimes
		{
			get
			{
				if (_effectTimes == null || _effectTimes.Count < 5)
				{
					InitEffectTimes();
				}
				return _effectTimes;
			}
		}

		public List<float> DamagePerSeconds
		{
			get
			{
				if (_damagePerSeconds == null || _damagePerSeconds.Count < 5)
				{
					InitDamagePerSeconds();
				}
				return _damagePerSeconds;
			}
		}

		private void InitEffectTimes()
		{
			_effectTimes = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_effectTimes.Add(1f);
			}
		}

		private void InitDamagePerSeconds()
		{
			_damagePerSeconds = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_damagePerSeconds.Add(1f);
			}
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				MonoCommand.ExecuteOn<DamageOverTimeEffect>(component.GameObject, new object[5]
				{
					component,
					_effectTimes[Controller.Model.Level],
					_damagePerSeconds[Controller.Model.Level],
					new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type, DamageType.Direct, unitTransform),
					ParticleEffect
				});
			}
		}
	}
}
