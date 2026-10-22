using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.App;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class RandomTargetDamageAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<float> _effectTimes;

		[HideInInspector]
		[SerializeField]
		private List<float> _damages;

		[HideInInspector]
		[SerializeField]
		private List<float> _damageIntervals;

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

		public List<float> Damage
		{
			get
			{
				if (_damages == null || _damages.Count < 5)
				{
					Init(ref _damages);
				}
				return _damages;
			}
		}

		public List<float> DamageIntervals
		{
			get
			{
				if (_damageIntervals == null || _damageIntervals.Count < 5)
				{
					Init(ref _damageIntervals);
				}
				return _damageIntervals;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_isCommon = true;
		}

		protected override void OnUseNegativeEffect(Transform[] unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			List<UnitController> list = new List<UnitController>();
			foreach (Transform transform in unitTransform)
			{
				UnitController component = transform.GetComponent<UnitController>();
				if (component != null)
				{
					list.Add(component);
				}
			}
			MonoCommand.ExecuteOn<RandomTargetDamageEffect>(MonoSingleton<AppController>.Instance.gameObject, new object[6]
			{
				list,
				_effectTimes[Controller.Model.Level],
				_damages[Controller.Model.Level],
				_damageIntervals[Controller.Model.Level],
				new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type),
				ParticleEffect
			});
		}
	}
}
