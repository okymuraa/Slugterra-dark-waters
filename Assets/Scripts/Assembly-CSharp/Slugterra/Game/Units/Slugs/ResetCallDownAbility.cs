using System.Collections.Generic;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class ResetCallDownAbility : SlugAbility
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

		protected override void OnUsePositiveEffect(Transform unitTransform)
		{
			base.OnUsePositiveEffect(unitTransform);
			HumanoidController component = unitTransform.GetComponent<HumanoidController>();
			if (component != null)
			{
				(component.Model as Humanoid).ResetCallDown(_effectTimes[Controller.Model.Level]);
			}
		}
	}
}
