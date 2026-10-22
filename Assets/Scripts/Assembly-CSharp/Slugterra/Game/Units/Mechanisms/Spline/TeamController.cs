using AppsMinistry.Core;
using AppsMinistry.Core.State;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Mechanisms;
using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms.Spline
{
	public class TeamController : MonoBehaviour, IStateMachineController, IObserver, IDestructible
	{
		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private Animator _fernandoAnimator;

		private Transform _transform;

		[SerializeField]
		private Animation _animation;

		private StateMachine _stateMachine;

		private Mechanism _model;

		private GameObject _gameObject;

		[SerializeField]
		private float _health = 1000f;

		private AudioSource _engineAudioSource;

		private float _defaultEnginePith;

		private float _stopEnginePith;

		private float _defaultEngineVolume;

		private float _stopEngineVolume;

		private bool _startEngine;

		private bool _stopEngine;

		protected ProgressBarController _progressBar;

		private const float _brakeOffDelay = 0.5f;

		private float _brakePushTime;

		private MeshRenderer _meshRenderer;

		public Animator Animator
		{
			get
			{
				return _animator;
			}
		}

		public Animator FernandoAnimator
		{
			get
			{
				return _fernandoAnimator;
			}
		}

		public Transform Transform
		{
			get
			{
				return _transform;
			}
		}

		public Animation Animation
		{
			get
			{
				return _animation;
			}
		}

		public StateMachine StateMachine
		{
			get
			{
				return _stateMachine;
			}
		}

		public Mechanism Model
		{
			get
			{
				return _model;
			}
		}

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
			if (GameObject.layer == LayerMask.NameToLayer("AllyLayer"))
			{
				WagonConfig wagonConfig = MonoSingleton<GameConfiguration>.Instance.WagonConfig;
				_model = new Wagon(wagonConfig);
				_model.MaxHealth = _health;
				_model.Health = _health;
			}
			else if (GameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
			{
				EnemyWagonConfig enemyWagonConfig = MonoSingleton<GameConfiguration>.Instance.EnemyWagonConfig;
				_model = new Wagon(enemyWagonConfig);
			}
			_model.AddObserver(this);
			_engineAudioSource = GetComponent<AudioSource>();
			if (_engineAudioSource != null)
			{
				_defaultEnginePith = _engineAudioSource.pitch;
				_defaultEngineVolume = _engineAudioSource.volume;
				_stopEnginePith = (_stopEngineVolume = 0f);
			}
			StateMachine.OnStateChanged += OnStateChanged;
			StateMachine.NextState<IdleState>(new object[0]);
			CreateProgressBar();
		}

		protected void CreateProgressBar()
		{
			GameObject original = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
			_progressBar = Object.Instantiate(original).GetComponent<ProgressBarController>();
			_progressBar.transform.parent = Transform;
			_progressBar.transform.localPosition = new Vector3(0f, 4f, 1f);
			_progressBar.transform.localScale = new Vector3(2f, 3f, 1f);
			_progressBar.Progress = _model.Health / _model.MaxHealth;
			_progressBar.Color = Color.green;
		}

		private bool LerpEngineSound(float pith, float volume)
		{
			if (_engineAudioSource != null)
			{
				_engineAudioSource.pitch = Mathf.Lerp(_engineAudioSource.pitch, pith, 2f * Time.deltaTime);
				_engineAudioSource.volume = Mathf.Lerp(_engineAudioSource.volume, volume, 2f * Time.deltaTime);
				if (Mathf.Abs(pith - _engineAudioSource.pitch) < 0.001f && Mathf.Abs(volume - _engineAudioSource.volume) < 0.001f)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		private void OnStateChanged(MonoState state)
		{
			_startEngine = state is MoveState;
			_progressBar.Progress = _model.Health / _model.MaxHealth;
			_stopEngine = state is IdleState || state is DeathState;
		}

		private void Start()
		{
			OnStart();
		}

		private void Update()
		{
			OnUpdate();
			if (_startEngine)
			{
				_startEngine = LerpEngineSound(_defaultEnginePith, _defaultEngineVolume);
			}
			if (_stopEngine)
			{
				_stopEngine = LerpEngineSound(_stopEnginePith, _stopEngineVolume);
			}
			if (_brakePushTime + 0.5f < Time.time && Model.Brake)
			{
				Model.Brake = false;
				_startEngine = true;
				_stopEngine = false;
			}
		}

		private void OnDestroy()
		{
			OnReleaseResource();
		}

		public void Next(MonoState state)
		{
			if (!(state is IdleState))
			{
				StateMachine.NextState<IdleState>(new object[0]);
			}
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

		public void OnDamage(float damage, Attacker parent)
		{
			if (!_model.IsDead())
			{
				_model.Health -= damage;
				_progressBar.Progress = _model.Health / _model.MaxHealth;
				if (_model.IsDead())
				{
					StateMachine.ChangeState<DeathState>(new object[0]);
				}
			}
		}

		public Renderer GetRenderer()
		{
			return null;
		}

		public void OnMove()
		{
			if (!Model.IsDead())
			{
				StateMachine.ChangeState<MoveState>(new object[0]);
			}
		}

		public void OnStop()
		{
			if (!Model.IsDead())
			{
				StateMachine.ChangeState<IdleState>(new object[0]);
			}
		}

		public void SetSpeed(float speed, float speedAnimationTrailer, float speedAnimationFandango)
		{
			_model.Speed = speed;
			_model.SpeedAnimationTrailer = speedAnimationTrailer;
			_model.SpeedAnimationFandango = speedAnimationFandango;
		}

		public void PushBrake()
		{
			if (StateMachine.CurrentState is MoveState)
			{
				if (!Model.Brake)
				{
					Model.Brake = true;
					_startEngine = false;
					_stopEngine = true;
				}
				_brakePushTime = Time.time;
			}
		}
	}
}
