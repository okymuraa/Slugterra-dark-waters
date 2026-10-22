using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class TornadoAbility : SlugAbility
	{
		[HideInInspector]
		[SerializeField]
		private List<float> _effectTimes;

		[SerializeField]
		[HideInInspector]
		private List<float> _dotRadius;

		[SerializeField]
		[HideInInspector]
		private List<float> _damagePerSecond;

		public List<float> EffectTimes
		{
			get
			{
				if (_effectTimes == null || _effectTimes.Count < 5)
				{
					Init(ref _effectTimes);
				}
				return _effectTimes;
			}
		}

		public List<float> DotRadius
		{
			get
			{
				if (_dotRadius == null || _dotRadius.Count < 5)
				{
					Init(ref _dotRadius);
				}
				return _dotRadius;
			}
		}

		public List<float> DamagePerSecond
		{
			get
			{
				if (_damagePerSecond == null || _damagePerSecond.Count < 5)
				{
					Init(ref _damagePerSecond);
				}
				return _damagePerSecond;
			}
		}

		protected override void OnUsePositiveEffect(Transform unitTransform)
		{
			base.OnUsePositiveEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				MonoCommand.ExecuteOn<TornadoEffect>(component.GameObject, new object[5]
				{
					component,
					EffectTimes[Controller.Model.Level],
					DotRadius[Controller.Model.Level],
					DamagePerSecond[Controller.Model.Level],
					ParticleEffect
				});
			}
		}
	}
}
