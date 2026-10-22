using System;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class EnemySlugStrategy : SlugStrategy
	{
		private const float SMOOTH_TIME = 0.15f;

		private const float MAX_ROTATE_Z = 35f;

		private const float FLY_TIME = 10f;

		private float _speed = 5f;

		private Vector3 _velocity;

		private Vector3 _rotateVelocity;

		private bool _isDecreaseDistance;

		private float _lastDistance;

		private float _destroyTime;

		private float _prevAngle;

		private float _flyTime;

		private float _turnTime = 0.7f;

		public EnemySlugStrategy(SlugController controller, int[] layers)
			: base(controller, layers)
		{
			_prevAngle = Controller.Transform.eulerAngles.y;
			_speed = Controller.Model.ProjectileSpeed;
			_flyTime = Time.time + 10f;
			SlugTurnTime component = controller.GetComponent<SlugTurnTime>();
			if (component != null)
			{
				_turnTime = component.TurnTime[controller.Model.Level];
			}
		}

		public override bool OnTriggerEnter(Transform transform)
		{
			if (_maxCollisionCount <= 0)
			{
				return false;
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
			if (_flyTime <= Time.time)
			{
				Controller.Target = null;
			}
			if (Controller.Target == null)
			{
				Controller.IsAutoAttack = false;
				float f = _prevAngle / 180f * (float)Math.PI;
				Vector3 velocity = new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f));
				Controller.SetVelocity(velocity);
				return;
			}
			Vector3 normalized = (Controller.Target.position - Controller.Transform.position).normalized;
			float angle = MathUtil.GetAngle(Vector2.right, new Vector2(normalized.x, normalized.z));
			float num = angle + 90f - Controller.Transform.eulerAngles.y;
			Vector3 eulerAngles = Controller.Transform.eulerAngles;
			if (Time.timeScale != 0f)
			{
				eulerAngles.y = Mathf.SmoothDampAngle(eulerAngles.y, angle + 90f, ref _rotateVelocity.y, _turnTime);
				if (num < -1f)
				{
					eulerAngles.z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, 35f, ref _rotateVelocity.z, 0.15f);
				}
				else if (num > 1f)
				{
					eulerAngles.z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, -35f, ref _rotateVelocity.z, 0.15f);
				}
				else
				{
					eulerAngles.z = Mathf.SmoothDampAngle(Controller.Transform.eulerAngles.z, 0f, ref _rotateVelocity.z, 0.15f);
				}
			}
			Controller.Transform.eulerAngles = eulerAngles;
			_prevAngle = eulerAngles.y;
			float f2 = eulerAngles.y / 180f * (float)Math.PI;
			float z = _speed * Mathf.Cos(f2);
			float x = _speed * Mathf.Sin(f2);
			Controller.Transform.position += new Vector3(x, _speed * normalized.y, z) * Time.deltaTime;
		}
	}
}
