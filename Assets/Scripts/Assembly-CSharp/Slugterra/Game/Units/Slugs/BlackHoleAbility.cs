using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class BlackHoleAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _effectTimes;

		[HideInInspector]
		[SerializeField]
		private List<float> _effectVelocity;

		[SerializeField]
		[HideInInspector]
		private List<float> _effectRadius;

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

		public List<float> EffectVelocity
		{
			get
			{
				if (_effectVelocity == null || _effectVelocity.Count < 5)
				{
					InitEffectVelocity();
				}
				return _effectVelocity;
			}
		}

		public List<float> EffectRadius
		{
			get
			{
				if (_effectRadius == null || _effectRadius.Count < 5)
				{
					InitEffectDeceys();
				}
				return _effectRadius;
			}
		}

		protected override float SplashArea
		{
			get
			{
				if (_effectRadius == null || Controller == null || Controller.Model == null)
				{
					return base.SplashArea;
				}
				return _effectRadius[Controller.Model.Level];
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

		private void InitEffectVelocity()
		{
			_effectVelocity = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_effectVelocity.Add(0.1f);
			}
		}

		private void InitEffectDeceys()
		{
			_effectRadius = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_effectRadius.Add(1f);
			}
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				Vector3 vector = new Vector3(Transform.position.x, unitTransform.position.y, Transform.position.z);
				Vector3 normalized = (unitTransform.position - vector).normalized;
				MonoCommand.ExecuteOn<BlackHoleEffect>(component.GameObject, new object[5]
				{
					component,
					_effectTimes[Controller.Model.Level],
					vector + normalized,
					_effectVelocity[Controller.Model.Level],
					ParticleEffect
				});
			}
		}
	}
}
