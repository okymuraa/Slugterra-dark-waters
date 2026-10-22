using System;
using Slugterra.Configuration;
using Slugterra.UI.Game.Camera;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class AllySlugStrategy : SlugStrategy
	{
		private float _speed;

		private float _prevAngle;

		private Vector3 _velocity;

		public AllySlugStrategy(SlugController controller, int[] layers)
			: base(controller, layers)
		{
			_speed = Controller.Model.ProjectileSpeed;
			_prevAngle = Controller.Transform.eulerAngles.y;
		}

		public override bool OnTriggerEnter(Transform transform)
		{
			if (_maxCollisionCount <= 0)
			{
				return false;
			}
			if (Controller.Model.Type != SlugType.Flopper)
			{
				CameraSmoothFollow cameraSmoothFollow = UnityEngine.Object.FindObjectOfType<CameraSmoothFollow>();
				if (cameraSmoothFollow != null)
				{
					cameraSmoothFollow.DoShake();
				}
			}
			int[] layers = _layers;
			foreach (int num in layers)
			{
				if (transform.gameObject.layer == num)
				{
					Controller.OnExplosion();
					OnUseAbility(transform);
					if (Controller.IsSplash)
					{
						SplashDamage(transform);
					}
					else
					{
						OnHit(transform);
					}
					_maxCollisionCount--;
					return true;
				}
			}
			if (CollisionWithLevelItem(transform))
			{
				Controller.OnExplosion();
				OnUseAbility(transform);
				if (Controller.IsSplash)
				{
					SplashDamage(transform);
				}
				return true;
			}
			return false;
		}

		public override void OnHit(Transform transform)
		{
			if (!(transform == null))
			{
				IDestructible component = transform.GetComponent<IDestructible>();
				if (component != null)
				{
					component.OnDamage(Controller.Damage * Controller.Parent.Model.CurrentAttackCoeficient(), new Attacker(Controller.Parent.GetType(), Controller.Parent.Transform, Controller.GetType(), Controller.Model.Type, DamageType.Direct, transform));
				}
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (Controller.IsAutoAttack)
			{
				OnFly();
			}
		}

		private void OnFly()
		{
			float num = 0f;
			if (Controller.Target != null)
			{
				num = Vector3.SqrMagnitude(Controller.Transform.position - Controller.Target.position);
			}
			if (num <= 0.1f)
			{
				Controller.Target = null;
			}
			if (Controller.Target == null)
			{
				Controller.IsAutoAttack = false;
				float f = _prevAngle / 180f * (float)Math.PI;
				Vector3 velocity = new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f));
				Controller.SetVelocity(velocity);
			}
			else
			{
				Controller.Transform.LookAt(Controller.Target.position);
				Vector3 normalized = (Controller.Target.position - Controller.Transform.position).normalized;
				Controller.Transform.position += normalized * Time.deltaTime * _speed;
				_prevAngle = Controller.Transform.eulerAngles.y;
			}
		}
	}
}
