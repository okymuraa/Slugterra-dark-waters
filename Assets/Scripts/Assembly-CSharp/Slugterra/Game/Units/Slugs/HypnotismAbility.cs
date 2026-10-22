using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class HypnotismAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _effectTimes;

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

		private void InitEffectTimes()
		{
			_effectTimes = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_effectTimes.Add(1f);
			}
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null && !component.NewVersionAI)
			{
				MonoCommand.ExecuteOn<HypnotismEffect>(component.GameObject, new object[4]
				{
					component,
					_effectTimes[Controller.Model.Level],
					new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type),
					ParticleEffect
				});
			}
			else if (component != null)
			{
				component.HypnotizmEffect(Controller.Parent.Transform.GetComponent<UnitController>(), _effectTimes[Controller.Model.Level], ParticleEffect);
			}
		}
	}
}
