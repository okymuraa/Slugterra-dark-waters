using System.Collections.Generic;
using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class RegenerationAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _effectTimes;

		[SerializeField]
		[HideInInspector]
		private List<float> _hpPerSeconds;

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

		public List<float> HPPerSeconds
		{
			get
			{
				if (_hpPerSeconds == null || _hpPerSeconds.Count < 5)
				{
					InitHPPerSeconds();
				}
				return _hpPerSeconds;
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

		private void InitHPPerSeconds()
		{
			_hpPerSeconds = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_hpPerSeconds.Add(0f);
			}
		}

		protected override void OnUsePositiveEffect(Transform unitTransform)
		{
			base.OnUsePositiveEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				MonoCommand.ExecuteOn<RegenerationEffect>(component.GameObject, new object[4]
				{
					component.Model,
					_effectTimes[Controller.Model.Level],
					_hpPerSeconds[Controller.Model.Level],
					ParticleEffect
				});
			}
		}
	}
}
