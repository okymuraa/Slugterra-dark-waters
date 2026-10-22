using AppsMinistry.Core.Command;
using AppsMinistry.Core.Sound;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class DetonateEffect : UnitEffect
	{
		private Attacker _parent;

		private float _damage;

		private int[] _layers;

		private UnitController _unitController;

		private float _delay = 3f;

		private float _detonateTime;

		private float _hittingArea = 1f;

		private GameObject _effectLable;

		private GameObject _explosionParticle;

		private SoundContainer _explosionSound;

		protected override void OnStart(object[] args)
		{
			_unitController = (UnitController)args[0];
			_damage = (float)args[1];
			if (args.Length > 2)
			{
				_delay = (float)args[2];
			}
			if (args.Length > 3)
			{
				_hittingArea = (float)args[3];
			}
			if (args.Length > 4)
			{
				_parent = (Attacker)args[4];
			}
			if (_effectLable == null)
			{
				GameObject gameObject = args[5] as GameObject;
				if (gameObject != null)
				{
					_effectLable = Object.Instantiate(gameObject, transform.position, Quaternion.identity);
					_effectLable.transform.parent = transform;
				}
			}
			if (args.Length > 6)
			{
				_explosionParticle = args[6] as GameObject;
			}
			if (args.Length > 7)
			{
				_explosionSound = args[7] as SoundContainer;
			}
			if (_explosionSound != null)
			{
				_explosionSound.RegisterInSoundManager();
			}
			_layers = new int[3];
			_layers[0] = LayerMask.NameToLayer("EnemyLayer");
			_layers[1] = LayerMask.NameToLayer("AllyLayer");
			_layers[2] = LayerMask.NameToLayer("NeutralLayer");
			_detonateTime = Time.time + _delay;
		}

		private void Update()
		{
			if (_unitController == null || _unitController.Model.IsDead())
			{
				Finish();
			}
			else if (_detonateTime <= Time.time)
			{
				if (_explosionParticle != null)
				{
					Object.Instantiate(_explosionParticle, transform.position, Quaternion.identity);
				}
				MonoCommand.Execute<AreaDamageCommand>(new object[5]
				{
					gameObject.transform.position,
					_hittingArea,
					_damage,
					_layers,
					_parent
				});
				PlayExplosionSound();
				Finish();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_effectLable != null)
			{
				Object.Destroy(_effectLable);
			}
		}

		private SoundItem PlayExplosionSound()
		{
			return (!(_explosionSound != null)) ? null : _explosionSound.Play();
		}
	}
}
