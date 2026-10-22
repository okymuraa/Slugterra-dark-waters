using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.Sound;
using AppsMinistry.Core.State;
using AppsMinistry.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Wave;
using UnityEngine;

namespace Slugterra.Game.Units
{
	[Serializable]
	public abstract class UnitController : MonoBehaviour, IStateMachineController, IDestructible
	{
		public enum UnitMovementType
		{
			Forward = 0,
			Backward = 1,
			CustomLookPoint = 2
		}

		[SerializeField]
		private bool _useDefaultConfig;

		[SerializeField]
		private bool _useAdaptiveConfig;

		[SerializeField]
		private int _level;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private ViewAreaTrigger _viewArea;

		[SerializeField]
		private bool _social;

		[SerializeField]
		private float _socialRadius;

		[SerializeField]
		private float _maxHealth = 100f;

		[SerializeField]
		private bool _isLocal = true;

		[SerializeField]
		private SoundContainer _walkSound;

		[SerializeField]
		private SoundContainer _hurtSound;

		[SerializeField]
		private SoundContainer _deathSound;

		private float _nextPlayHurtSound;

		private UnitPathFinder _pathFinder;

		private Transform _transform;

		private HitMarker _hitMarker;

		private Transform _attackTarget;

		private Transform _moveTarget;

		private bool _rotateToTarget;

		private RotateToCommand _rotateToCommand;

		private DamageSystem _damageSystem;

		[SerializeField]
		private bool _newVersionAI;

		[SerializeField]
		private IBehavior _myBehavior;

		private StateMachine _stateMachine;

		private GameObject _gameObject;

		[SerializeField]
		private bool _canAttack = true;

		private MovePath _movePath;

		protected ProgressBarController _progressBar;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float, Attacker> OnDamageRecievedEvent__BackingField;

		[SerializeField]
		private Renderer _Renderer;

		[HideInInspector]
		public Transform CustomLookPoint
		{
			get
			{
				return PathFinder.CustomLookPoint;
			}
			set
			{
				PathFinder.CustomLookPoint = value;
			}
		}

		public bool UseDefaultConfig
		{
			get
			{
				return _useDefaultConfig;
			}
		}

		public bool UseAdaptiveConfig
		{
			get
			{
				return _useAdaptiveConfig;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
		}

		public Animator Animator
		{
			get
			{
				return _animator;
			}
		}

		public ViewAreaTrigger ViewArea
		{
			get
			{
				return _viewArea;
			}
		}

		public float SocialRadius
		{
			get
			{
				return _socialRadius;
			}
			set
			{
				if (value >= 0f)
				{
					_socialRadius = value;
				}
				else if (value <= 0f)
				{
					_socialRadius = 0f;
				}
			}
		}

		public float MaxHealth
		{
			get
			{
				return _maxHealth;
			}
		}

		public bool IsLocal
		{
			get
			{
				return _isLocal;
			}
			set
			{
				_isLocal = value;
			}
		}

		public UnitPathFinder PathFinder
		{
			get
			{
				return _pathFinder;
			}
		}

		public Transform Transform
		{
			get
			{
				return _transform;
			}
		}

		public HitMarker HitMarker
		{
			get
			{
				return _hitMarker;
			}
		}

		public virtual Transform AttackTarget
		{
			get
			{
				return _attackTarget;
			}
			set
			{
				_attackTarget = value;
				ViewArea.CurrentItem = _attackTarget;
				if (!NewVersionAI)
				{
					OnAttackTargetChanged();
				}
			}
		}

		public Transform MoveTarget
		{
			get
			{
				return _moveTarget;
			}
		}

		public bool RotateToTarget
		{
			get
			{
				return _rotateToTarget;
			}
			set
			{
				if (!(AttackTarget == null) || !value)
				{
					_rotateToTarget = value;
					RotateToAttackTarget();
				}
			}
		}

		public RotateToCommand RotateToCommand
		{
			get
			{
				return _rotateToCommand;
			}
		}

		public DamageSystem DamageSystem
		{
			get
			{
				return _damageSystem;
			}
		}

		public bool NewVersionAI
		{
			get
			{
				return _newVersionAI;
			}
			set
			{
				_newVersionAI = value;
			}
		}

		public IBehavior MyBehavior
		{
			get
			{
				return _myBehavior;
			}
			set
			{
				_myBehavior = value;
			}
		}

		public StateMachine StateMachine
		{
			get
			{
				return _stateMachine;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return _gameObject;
			}
		}

		public bool CanAttack
		{
			get
			{
				return _canAttack;
			}
			set
			{
				_canAttack = value;
			}
		}

		public bool IsFallDown { get; protected set; }

		public abstract Unit Model { get; }

		public MovePath MovePath
		{
			get
			{
				return _movePath;
			}
			set
			{
				if (NewVersionAI)
				{
					MyBehavior.UseFiniteCustomPath(value);
					MyBehavior.CanPatrol = true;
					if (value.UseCustomSpeed)
					{
						MyBehavior.UseCustomPatrolSpeed(value.CustomSpeed);
					}
				}
				else
				{
					_movePath = value;
				}
			}
		}

		public event Action<float, Attacker> OnDamageRecievedEvent
		{
			add
			{
				Action<float, Attacker> action = OnDamageRecievedEvent__BackingField;
				Action<float, Attacker> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDamageRecievedEvent__BackingField, (Action<float, Attacker>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<float, Attacker> action = OnDamageRecievedEvent__BackingField;
				Action<float, Attacker> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnDamageRecievedEvent__BackingField, (Action<float, Attacker>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public virtual void Next(MonoState state)
		{
		}

		protected virtual void OnCreate()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnReleaseResource()
		{
		}

		protected virtual void OnUseConfigStrategy()
		{
		}

		private void Awake()
		{
			_gameObject = gameObject;
			_transform = transform;
			_pathFinder = GetComponent<UnitPathFinder>();
			_stateMachine = _gameObject.AddComponent<StateMachine>();
			_stateMachine.Controller = this;
			if (NewVersionAI)
			{
				_stateMachine.STopStateMashine();
			}
			_damageSystem = _gameObject.AddComponent<DamageSystem>();
			if (_pathFinder != null)
			{
				_moveTarget = _pathFinder.target;
			}
			_hitMarker = GetComponent<HitMarker>();
			if (_hitMarker == null)
			{
				_hitMarker = _gameObject.AddComponent<HitMarker>();
			}
			OnCreate();
		}

		private void Start()
		{
			if (_walkSound != null)
			{
				_walkSound.RegisterInSoundManager();
			}
			if (_hurtSound != null)
			{
				_hurtSound.RegisterInSoundManager();
			}
			if (_deathSound != null)
			{
				_deathSound.RegisterInSoundManager();
			}
			if (_viewArea != null)
			{
				_viewArea.SetRadius(Model.ViewRadius);
				_viewArea.SetAngle(Model.ViewAngle);
			}
			Model.OnDamageEvent += ModelOnDamage;
			OnStart();
		}

		private void Update()
		{
			OnUpdate();
		}

		private void OnDestroy()
		{
			OnReleaseResource();
			Model.OnDamageEvent -= ModelOnDamage;
		}

		protected virtual void RotateToAttackTarget()
		{
			RotateToAttackTarget(Animator.transform);
		}

		protected void RotateToAttackTarget(Transform rotateTransform)
		{
			if (_rotateToTarget && _attackTarget != null)
			{
				_rotateToCommand = MonoCommand.ExecuteOn<RotateToCommand>(gameObject, new object[3] { Transform, rotateTransform, _attackTarget });
				_rotateToCommand.AsyncToken.AddResponder(new Responder<RotateToCommand>(OnRotateToCommandFinish));
			}
			else if (_rotateToCommand != null)
			{
				_rotateToCommand.Finish();
				_rotateToCommand = null;
			}
		}

		private void OnRotateToCommandFinish(RotateToCommand command)
		{
			if (RotateToTarget)
			{
				RotateToTarget = false;
			}
			_rotateToCommand = null;
		}

		protected virtual void OnAttackTargetChanged()
		{
			RotateToAttackTarget();
		}

		public virtual void OnDamage(float damage, Attacker attackedUnit = null)
		{
			if (Model.IsDead() || attackedUnit == null || attackedUnit.transform == null || GameObject.layer == attackedUnit.transform.gameObject.layer)
			{
				return;
			}
			_damageSystem.OnDamage(attackedUnit.transform, damage);
			RaiseOnDamageRecievedEvent(damage, attackedUnit);
			if (!_social || damage <= 0f || !(_socialRadius > 0f))
			{
				return;
			}
			Collider[] array = PhysicsFacade.OverlapSphere(Transform.position, _socialRadius, 1 << _gameObject.layer);
			for (int i = 0; i < array.Length; i++)
			{
				UnitController unitController = array[i].GetComponent(GetType()) as UnitController;
				if (unitController != null && unitController.AttackTarget == null && unitController.GetType() == GetType())
				{
					unitController.AttackTarget = attackedUnit.transform;
				}
			}
		}

		public Renderer GetRenderer()
		{
			return _Renderer;
		}

		private void ModelOnDamage(float offset)
		{
			if (Model.IsDead())
			{
				PlayDeathSound();
			}
			else if (_nextPlayHurtSound < Time.time && PlayHurtSound() != null)
			{
				_nextPlayHurtSound = Time.time + 1f / offset + 0.6f;
			}
		}

		private void OnDrawGizmos()
		{
			if (_social)
			{
				Gizmos.color = new Color(0f, 1f, 1f, 0.2f);
				Gizmos.DrawSphere(transform.position, _socialRadius);
			}
		}

		public virtual void OnFallDown()
		{
			if (Model == null || IsFallDown)
			{
				return;
			}
			if (NewVersionAI)
			{
				MyBehavior.OnFallDown();
			}
			AnimatorStateInfo currentAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
			AnimatorStateInfo nextAnimatorStateInfo = Animator.GetNextAnimatorStateInfo(0);
			if (!currentAnimatorStateInfo.IsName("FallDown") && !nextAnimatorStateInfo.IsName("FallDown"))
			{
				if (AttackTarget != null)
				{
					AttackTarget = null;
				}
				Animator.SetBool("EnemyInSight", false);
				Animator.SetTrigger("OnFallDown");
				if ((bool)PathFinder)
				{
					PathFinder.enabled = false;
				}
				enabled = false;
				CanAttack = false;
				IsFallDown = true;
				if (StateMachine.CurrentState != null)
				{
					StateMachine.CurrentState.enabled = false;
				}
				StateMachine.enabled = false;
				Model.AddSpeedCoeficient(GetType(), 0f);
			}
		}

		public virtual void OnStandUp()
		{
			if (Model != null && !Model.IsDead() && IsFallDown)
			{
				if (NewVersionAI)
				{
					StartCoroutine(MyBehavior.OnStandUp(1.5f));
				}
				Animator.SetTrigger("OnStandUp");
				StartCoroutine(EnablePathFinder(1.5f));
			}
		}

		protected virtual void OnStandUpComplited()
		{
		}

		private IEnumerator EnablePathFinder(float time)
		{
			yield return new WaitForSeconds(time);
			if ((bool)PathFinder)
			{
				PathFinder.enabled = true;
			}
			if (StateMachine.CurrentState != null)
			{
				StateMachine.CurrentState.enabled = true;
			}
			StateMachine.enabled = true;
			CanAttack = true;
			IsFallDown = false;
			OnStandUpComplited();
			Model.RemoveSpeedCoeficient(GetType());
		}

		public void SetViewRadius(float radius)
		{
			_viewArea.SetRadius(radius);
		}

		public void Reset()
		{
			Transform parent = base.transform.parent;
			foreach (Transform item in parent)
			{
				if (item != transform)
				{
					item.position = transform.position;
				}
			}
		}

		public void SetAttackTarget(Transform target)
		{
			AttackTarget = target;
			if (NewVersionAI)
			{
				MyBehavior.AttackTarget(target);
			}
		}

		public void SetMaxHealth(float value)
		{
			_maxHealth = value;
		}

		public void SetLevel(int lvl)
		{
			_level = lvl;
		}

		public SoundItem PlayWalkSound()
		{
			return (!(_walkSound != null) || !IsLocal) ? null : _walkSound.Play();
		}

		public SoundItem PlayHurtSound()
		{
			return (!(_hurtSound != null) || !IsLocal) ? null : _hurtSound.Play();
		}

		public SoundItem PlayDeathSound()
		{
			return (!(_deathSound != null) || !IsLocal) ? null : _deathSound.Play();
		}

		public void HypnotizmEffect(UnitController attacker, float durationTime, GameObject effect)
		{
			ToAlly();
			if (attacker.AttackTarget == Transform)
			{
				attacker.AttackTarget = null;
			}
			Invoke("ToEnemy", durationTime);
		}

		public void ToAlly()
		{
			if (NewVersionAI && MyBehavior != null)
			{
				MyBehavior.ConvertToAlly();
			}
			else if (StateMachine.CurrentState != null)
			{
				StateMachine.CurrentState.Finish();
			}
			ViewArea.RemoveLayer("AllyLayer");
			ViewArea.AddLayer("EnemyLayer");
			gameObject.layer = LayerMask.NameToLayer("AllyLayer");
			ViewArea.SetBaseLayer(gameObject.layer);
			if (_progressBar != null)
			{
				_progressBar.Color = Color.green;
			}
			ViewArea.Items.Clear();
			DamageSystem.Clear();
			if (AttackTarget != null)
			{
				AttackTarget = null;
			}
			ViewArea.UpdateTargets();
		}

		public void ToEnemy()
		{
			if (NewVersionAI && MyBehavior != null)
			{
				MyBehavior.ConvertToEnemy();
			}
			else if (StateMachine.CurrentState != null)
			{
				StateMachine.CurrentState.Finish();
			}
			ViewArea.RemoveLayer("EnemyLayer");
			ViewArea.AddLayer("AllyLayer");
			gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
			ViewArea.SetBaseLayer(gameObject.layer);
			if (_progressBar != null)
			{
				_progressBar.Color = Color.red;
			}
			ViewArea.Items.Clear();
			DamageSystem.Clear();
			if (AttackTarget != null)
			{
				AttackTarget = null;
			}
			ViewArea.UpdateTargets();
		}

		public void RepaintProgressBarColor()
		{
			if ((bool)_progressBar)
			{
				ProgressBarController progressBar = _progressBar;
				Color color;
				if (LayerMask.LayerToName(GameObject.layer) == "AllyLayer")
				{
					color = Color.green;
				}
				else
				{
					color = ((!(LayerMask.LayerToName(GameObject.layer) == "NeutralLayer") && !(LayerMask.LayerToName(GameObject.layer) == "Default")) ? Color.red : Color.yellow);
				}
				progressBar.Color = color;
			}
		}

		protected void RaiseOnDamageRecievedEvent(float damage, Attacker attacker)
		{
			if (OnDamageRecievedEvent__BackingField != null)
			{
				OnDamageRecievedEvent__BackingField(damage, attacker);
			}
		}
	}
}
