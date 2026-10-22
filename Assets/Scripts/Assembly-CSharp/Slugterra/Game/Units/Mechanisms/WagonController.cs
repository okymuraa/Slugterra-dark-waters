using AppsMinistry.Core;
using AppsMinistry.Core.State;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Mechanisms;
using Slugterra.Game.Wave;
using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms
{
	public class WagonController : MechanismController
	{
		[SerializeField]
		private Animator _fernandoAnimator;

		[SerializeField]
		private float _health = 1000f;

		private Wagon _model;

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

		public Animator FernandoAnimator
		{
			get
			{
				return _fernandoAnimator;
			}
		}

		public override Mechanism Model
		{
			get
			{
				return _model;
			}
		}

		protected override void OnCreate()
		{
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
			_progressBar.transform.localPosition = new Vector3(0f, 4f, -1f);
			_progressBar.transform.localScale = new Vector3(2f, 3f, 1f);
			_progressBar.Progress = _model.Health / _model.MaxHealth;
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

		protected override void OnUpdate()
		{
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

		private void OnStateChanged(MonoState state)
		{
			_startEngine = state is MoveState;
			_stopEngine = state is IdleState;
		}

		public override void Next(MonoState state)
		{
			if (!(state is IdleState))
			{
				StateMachine.NextState<IdleState>(new object[0]);
			}
		}

		public override void OnDamage(float damage, Attacker parent)
		{
			if (!_model.IsDead())
			{
				_model.Health -= damage;
				_progressBar.Progress = _model.Health / _model.MaxHealth;
				if (_model.Health <= 0f)
				{
					StateMachine.ChangeState<DeathState>(new object[0]);
				}
			}
		}

		public void OnMove(MovePath path)
		{
			StateMachine.ChangeState<MoveState>(new object[1] { path.Points });
		}

		public void SetSpeed(float speed)
		{
			_model.Speed = speed;
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
