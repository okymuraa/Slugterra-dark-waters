using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Domain;
using Newtonsoft.Json;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.Domain.Game.Units
{
	public abstract class Unit : DomainObject
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float> OnDamageEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnDeadEvent__BackingField;

		[JsonProperty]
		private float _health;

		[JsonProperty]
		private float _currentSpeed;

		[JsonProperty]
		private float _viewRadius;

		[JsonProperty]
		private float _viewAngle;

		[JsonProperty]
		private float _socialRadius;

		[JsonProperty]
		private IDictionary<Type, float> _speedCoeficients;

		[JsonProperty]
		private IDictionary<Type, float> _attackCoeficients;

		[JsonProperty]
		private IDictionary<Type, float> _attackSpeedCoeficients;

		[JsonProperty]
		private IDictionary<Type, float> _damageCoeficients;

		[JsonProperty]
		public bool Invulnerability { get; set; }

		public bool AntiNegativeAbilities { get; set; }

		[JsonProperty]
		public float MaxHealth { get; set; }

		[JsonProperty]
		public float Rank { get; set; }

		[JsonIgnore]
		public virtual float Health
		{
			get
			{
				return _health;
			}
			set
			{
				if (value != _health && (!Invulnerability || !(_health > value)))
				{
					float num = _health - value;
					if (num > 0f)
					{
						num *= CurrentDamageCoeficient();
						_health -= num;
						RaiseDamageEvent(num);
					}
					else
					{
						_health = ((!(value < MaxHealth)) ? MaxHealth : value);
					}
					if (IsDead())
					{
						RaiseDeadEvent();
					}
					CommitAndNotify();
				}
			}
		}

		[JsonProperty]
		public float WalkSpeed { get; private set; }

		[JsonProperty]
		public float RunSpeed { get; private set; }

		[JsonProperty]
		public float BackwardSpeed { get; private set; }

		[JsonIgnore]
		public virtual float CurrentSpeed
		{
			get
			{
				return _currentSpeed * CurrentSpeedCoeficient();
			}
			set
			{
				_currentSpeed = value;
				CommitAndNotify();
			}
		}

		[JsonIgnore]
		public float ViewRadius
		{
			get
			{
				return _viewRadius;
			}
		}

		[JsonIgnore]
		public float ViewAngle
		{
			get
			{
				return _viewAngle;
			}
		}

		[JsonIgnore]
		public float SocialRadius
		{
			get
			{
				return _socialRadius;
			}
		}

		public event Action<float> OnDamageEvent
		{
			add
			{
				Action<float> action = OnDamageEvent__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDamageEvent__BackingField, (Action<float>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<float> action = OnDamageEvent__BackingField;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDamageEvent__BackingField, (Action<float>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnDeadEvent
		{
			add
			{
				Action action = OnDeadEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDeadEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnDeadEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDeadEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		[JsonConstructor]
		protected Unit()
		{
		}

		public Unit(BaseUnitConfig config)
		{
			_speedCoeficients = new Dictionary<Type, float>();
			_attackCoeficients = new Dictionary<Type, float>();
			_attackSpeedCoeficients = new Dictionary<Type, float>();
			_damageCoeficients = new Dictionary<Type, float>();
			WalkSpeed = config.WalkSpeed;
			RunSpeed = config.RunSpeed;
			MaxHealth = config.Health;
			_health = MaxHealth;
			_viewRadius = config.ViewRadius;
			_viewAngle = config.ViewAngle;
			_socialRadius = config.SocialRadius;
		}

		public virtual void ChangeConfig(BaseUnitConfig config)
		{
			WalkSpeed = config.WalkSpeed;
			RunSpeed = config.RunSpeed;
			MaxHealth = config.Health;
			_health = MaxHealth;
			_viewRadius = config.ViewRadius;
			_viewAngle = config.ViewAngle;
			_socialRadius = config.SocialRadius;
		}

		public void AddHealth(float health, bool useDirect = false)
		{
			float health2 = Math.Min(_health + health, MaxHealth);
			if (useDirect)
			{
				_health = health2;
			}
			else
			{
				Health = Math.Min(_health + health, MaxHealth);
			}
		}

		public bool IsDead()
		{
			if (Health > 0f)
			{
				return false;
			}
			return true;
		}

		public void AddSpeedCoeficient(Type type, float coeficient)
		{
			_speedCoeficients[type] = coeficient;
			CommitAndNotify();
		}

		public void RemoveSpeedCoeficient(Type type)
		{
			if (_speedCoeficients.ContainsKey(type))
			{
				_speedCoeficients.Remove(type);
			}
			CommitAndNotify();
		}

		protected float CurrentSpeedCoeficient()
		{
			float num = 1f;
			foreach (Type key in _speedCoeficients.Keys)
			{
				num *= _speedCoeficients[key];
			}
			return num;
		}

		public virtual void AddAttackSpeedCoeficient(Type type, float coeficient)
		{
			_attackSpeedCoeficients[type] = coeficient;
			CommitAndNotify();
		}

		public virtual void RemoveAttackSpeedCoeficient(Type type)
		{
			if (_attackSpeedCoeficients.ContainsKey(type))
			{
				_attackSpeedCoeficients.Remove(type);
			}
			CommitAndNotify();
		}

		public float CurrentAttackSpeedCoeficient()
		{
			float num = 1f;
			foreach (Type key in _attackSpeedCoeficients.Keys)
			{
				num *= _attackSpeedCoeficients[key];
			}
			return num;
		}

		public void AddAttackCoeficient(Type type, float coeficient)
		{
			_attackCoeficients[type] = coeficient;
			CommitAndNotify();
		}

		public void RemoveAttackCoeficient(Type type)
		{
			if (_attackCoeficients.ContainsKey(type))
			{
				_attackCoeficients.Remove(type);
			}
			CommitAndNotify();
		}

		public float CurrentAttackCoeficient()
		{
			float num = 1f;
			foreach (Type key in _attackCoeficients.Keys)
			{
				num *= _attackCoeficients[key];
			}
			return num;
		}

		public void AddDamageCoeficient(Type type, float coeficient)
		{
			_damageCoeficients[type] = coeficient;
			CommitAndNotify();
		}

		public void RemoveDamageCoeficient(Type type)
		{
			if (_damageCoeficients.ContainsKey(type))
			{
				_damageCoeficients.Remove(type);
			}
			CommitAndNotify();
		}

		public virtual float CurrentDamageCoeficient()
		{
			float num = 1f;
			if (_damageCoeficients == null)
			{
				return num;
			}
			foreach (Type key in _damageCoeficients.Keys)
			{
				num *= _damageCoeficients[key];
			}
			return num;
		}

		public void Walk()
		{
			CurrentSpeed = WalkSpeed;
		}

		public void Run()
		{
			CurrentSpeed = RunSpeed;
		}

		public void SetRawHealth(float health)
		{
			float num = _health - health;
			_health = Mathf.Clamp(health, 0f, MaxHealth);
			if (num > 0f)
			{
				RaiseDamageEvent(num);
			}
			CommitAndNotify();
		}

		protected void RaiseDamageEvent(float offset)
		{
			if (OnDamageEvent__BackingField != null)
			{
				OnDamageEvent__BackingField(offset);
			}
		}

		protected void RaiseDeadEvent()
		{
			if (OnDeadEvent__BackingField != null)
			{
				OnDeadEvent__BackingField();
			}
		}
	}
}
