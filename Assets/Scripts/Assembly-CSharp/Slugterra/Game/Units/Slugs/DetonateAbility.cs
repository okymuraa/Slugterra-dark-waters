using System.Collections.Generic;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.Sound;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class DetonateAbility : SlugAbility
	{
		[SerializeField]
		[HideInInspector]
		private List<int> _maxTargets;

		[HideInInspector]
		[SerializeField]
		private List<float> _damage;

		[HideInInspector]
		[SerializeField]
		private List<float> _delay;

		[SerializeField]
		[HideInInspector]
		private List<float> _hittingArea;

		[SerializeField]
		private GameObject _explosionParticle;

		[SerializeField]
		private SoundContainer _explosionSound;

		public List<int> MaxTargets
		{
			get
			{
				if (_maxTargets == null || _maxTargets.Count < 5)
				{
					Init(ref _maxTargets);
				}
				return _maxTargets;
			}
		}

		public List<float> Damage
		{
			get
			{
				if (_damage == null || _damage.Count < 5)
				{
					Init(ref _damage);
				}
				return _damage;
			}
		}

		public List<float> Delay
		{
			get
			{
				if (_delay == null || _delay.Count < 5)
				{
					Init(ref _delay);
				}
				return _delay;
			}
		}

		public List<float> HittingArea
		{
			get
			{
				if (_hittingArea == null || _hittingArea.Count < 5)
				{
					Init(ref _hittingArea);
				}
				return _hittingArea;
			}
		}

		protected GameObject ExplosionParticle
		{
			get
			{
				return _explosionParticle;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			MaxUnitsForSplash = _maxTargets[Controller.Model.Level];
		}

		protected override void OnUseNegativeEffect(Transform unitTransform)
		{
			base.OnUseNegativeEffect(unitTransform);
			UnitController component = unitTransform.GetComponent<UnitController>();
			if (component != null)
			{
				MonoCommand.ExecuteOn<DetonateEffect>(component.GameObject, new object[8]
				{
					component,
					_damage[Controller.Model.Level],
					_delay[Controller.Model.Level],
					_hittingArea[Controller.Model.Level],
					new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type),
					ParticleEffect,
					ExplosionParticle,
					_explosionSound
				});
			}
		}
	}
}
