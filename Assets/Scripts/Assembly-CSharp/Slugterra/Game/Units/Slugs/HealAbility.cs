using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class HealAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _hp;

		public List<float> HP
		{
			get
			{
				if (_hp == null || _hp.Count < 5)
				{
					InitHP();
				}
				return _hp;
			}
		}

		private void InitHP()
		{
			_hp = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_hp.Add(1f);
			}
		}

		protected override void OnUsePositiveEffect(Transform unitTransform)
		{
			base.OnUsePositiveEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null && component.Model != null)
			{
				component.Model.Health += _hp[Controller.Model.Level];
			}
		}
	}
}
