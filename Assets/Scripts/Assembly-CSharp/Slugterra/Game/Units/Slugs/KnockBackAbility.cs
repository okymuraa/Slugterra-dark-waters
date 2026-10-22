using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class KnockBackAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _effectTimes;

		[SerializeField]
		[HideInInspector]
		private List<float> _stunChances;

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

		public List<float> StunChances
		{
			get
			{
				if (_stunChances == null || _stunChances.Count < 5)
				{
					InitStunChnaces();
				}
				return _stunChances;
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

		private void InitStunChnaces()
		{
			_stunChances = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_stunChances.Add(0f);
			}
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				MonoCommand.ExecuteOn<KnockBackEffect>(component.GameObject, new object[4]
				{
					component,
					_effectTimes[Controller.Model.Level],
					_stunChances[Controller.Model.Level],
					ParticleEffect
				});
			}
		}
	}
}
