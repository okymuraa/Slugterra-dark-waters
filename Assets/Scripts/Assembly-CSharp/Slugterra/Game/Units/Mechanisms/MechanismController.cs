using AppsMinistry.Core.State;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Units.Mechanisms;
using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms
{
	public abstract class MechanismController : MonoBehaviour, IStateMachineController, IObserver, IDestructible
	{
		[SerializeField]
		private Animator _animator;

		private Transform _transform;

		private UnitPathFinder _pathFinder;

		private StateMachine _stateMachine;

		private GameObject _gameObject;

		public Animator Animator
		{
			get
			{
				return _animator;
			}
		}

		public Transform Transform
		{
			get
			{
				return _transform;
			}
		}

		public UnitPathFinder PathFinder
		{
			get
			{
				return _pathFinder;
			}
		}

		public StateMachine StateMachine
		{
			get
			{
				return _stateMachine;
			}
		}

		public abstract Mechanism Model { get; }

		public GameObject GameObject
		{
			get
			{
				return _gameObject;
			}
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

		private void Awake()
		{
			_gameObject = gameObject;
			_transform = base.transform;
			_pathFinder = GetComponent<UnitPathFinder>();
			_stateMachine = _gameObject.AddComponent<StateMachine>();
			_stateMachine.Controller = this;
			HitMarker component = GetComponent<HitMarker>();
			if (component == null)
			{
				component = gameObject.AddComponent<HitMarker>();
				Transform transform = new GameObject("HitMarker").transform;
				transform.parent = base.transform;
				transform.localPosition = Vector3.up * 0.3f;
				component.Markers.Add(transform);
			}
			OnCreate();
		}

		private void Start()
		{
			OnStart();
		}

		private void Update()
		{
			OnUpdate();
		}

		private void OnDestroy()
		{
			OnReleaseResource();
		}

		public virtual void Next(MonoState state)
		{
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Mechanism)
			{
				OnModelChanged(observable as Mechanism);
			}
		}

		protected virtual void OnModelChanged(Mechanism model)
		{
		}

		public virtual void OnDamage(float damage, Attacker parent)
		{
		}

		public Renderer GetRenderer()
		{
			return null;
		}
	}
}
