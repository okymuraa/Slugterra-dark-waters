using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class FearAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _effectTimes;

		[SerializeField]
		[HideInInspector]
		private List<float> _ammoFearTimes;

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

		public List<float> AmmoFearTimes
		{
			get
			{
				if (_ammoFearTimes == null || _ammoFearTimes.Count < 5)
				{
					InitAmmoFearTimes();
				}
				return _ammoFearTimes;
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

		private void InitAmmoFearTimes()
		{
			_ammoFearTimes = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_ammoFearTimes.Add(1f);
			}
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (!(component == null))
			{
				MonoCommand.ExecuteOn<FearEffect>(component.GameObject, new object[3]
				{
					component,
					_effectTimes[Controller.Model.Level],
					ParticleEffect
				});
				if (component is HumanoidController)
				{
					MonoCommand.ExecuteOn<AmmoFearEffect>(component.GameObject, new object[2]
					{
						component,
						_ammoFearTimes[Controller.Model.Level]
					});
				}
			}
		}
	}
}
