using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class AttackRateAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _effectTimes;

		[HideInInspector]
		[SerializeField]
		private List<float> _effectDeceys;

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

		public List<float> EffectDeceys
		{
			get
			{
				if (_effectDeceys == null || _effectDeceys.Count < 5)
				{
					InitEffectDeceys();
				}
				return _effectDeceys;
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

		private void InitEffectDeceys()
		{
			_effectDeceys = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_effectDeceys.Add(0f);
			}
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				MonoCommand.ExecuteOn<AttackRateEffect>(component.GameObject, new object[4]
				{
					component,
					_effectTimes[Controller.Model.Level],
					_effectDeceys[Controller.Model.Level],
					ParticleEffect
				});
			}
		}

		protected override void OnUsePositiveEffect(Transform unitTransform)
		{
			base.OnUsePositiveEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				MonoCommand.ExecuteOn<AttackRateEffect>(component.GameObject, new object[4]
				{
					component,
					_effectTimes[Controller.Model.Level],
					_effectDeceys[Controller.Model.Level],
					ParticleEffect
				});
			}
		}
	}
}
