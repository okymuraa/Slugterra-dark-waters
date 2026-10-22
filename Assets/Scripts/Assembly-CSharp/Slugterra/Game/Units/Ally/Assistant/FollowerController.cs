using AppsMinistry.Core.State;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Units.ConfigStrategy;
using Slugterra.Game.Units.Player;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class FollowerController : HumanoidController
	{
		[SerializeField]
		private Slugterra.Game.Units.AreaTrigger.AreaTrigger _allyArea;

		[SerializeField]
		protected Transform _playerTransform;

		protected Humanoid _model;

		protected GameController _gameController;

		public AllyType Type { get; protected set; }

		public Slugterra.Game.Units.AreaTrigger.AreaTrigger AllyArea
		{
			get
			{
				return _allyArea;
			}
		}

		public Transform PlayerTransform
		{
			get
			{
				return _playerTransform;
			}
		}

		public override Unit Model
		{
			get
			{
				return _model;
			}
		}

		protected override void OnCreate()
		{
			Type = AllyType.Follower;
			base.OnCreate();
			_gameController = Object.FindObjectOfType<GameController>();
			CreateProgressBar();
		}

		protected override void OnUseConfigStrategy()
		{
			FollowerConfigStrategy followerConfigStrategy = new FollowerConfigStrategy(this);
			followerConfigStrategy.LoadConfiguration(ref _model);
		}

		protected void CreateProgressBar()
		{
			GameObject original = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
			_progressBar = Object.Instantiate(original).GetComponent<ProgressBarController>();
			_progressBar.transform.parent = Transform;
			_progressBar.transform.localPosition = new Vector3(0f, 2.2f, 0f);
			_progressBar.Progress = _model.Health / _model.MaxHealth;
			_progressBar.Color = Color.green;
		}

		protected override void OnStart()
		{
			_model.Clear(Time.time);
			PathFinder.repathRate = 0f;
			PathFinder.canSearch = false;
			ViewArea.SetBaseTransform(Animator.transform);
			ViewArea.SetBaseLayer(gameObject.layer);
			_allyArea.AddIgnorObject(Transform);
			_playerTransform = PlayerManager.LocalPlayer.transform;
		}

		protected virtual void OnEnable()
		{
			_allyArea.OnTriggerExitEvent += OnAllyAreaExit;
			_allyArea.OnTriggerEnterEvent += OnAllyAreaEnter;
			_gameController.OnPlayCutSceneEvent += OnPlayCutScene;
		}

		protected virtual void OnDisable()
		{
			_allyArea.OnTriggerExitEvent -= OnAllyAreaExit;
			_allyArea.OnTriggerEnterEvent -= OnAllyAreaEnter;
			_gameController.OnPlayCutSceneEvent -= OnPlayCutScene;
		}

		private void OnPlayCutScene(bool isPlay)
		{
			if (isPlay)
			{
				Model.Invulnerability = true;
			}
			else
			{
				Model.Invulnerability = false;
			}
		}

		protected virtual void OnAllyAreaEnter(Transform item)
		{
		}

		protected virtual void OnAllyAreaExit(Transform item)
		{
			if (!Model.IsDead() && item == _playerTransform)
			{
				StateMachine.ChangeState<FollowState>(new object[1] { PlayerTransform });
			}
		}

		public override void Next(MonoState prevState)
		{
			if (!(StateMachine == null) && !(_allyArea == null))
			{
				if (_allyArea.gameObject.activeSelf && _allyArea.enabled && !_allyArea.Items.Contains(_playerTransform))
				{
					StateMachine.NextState<FollowState>(new object[1] { _playerTransform });
				}
				else if (!(prevState is IdleState))
				{
					StateMachine.NextState<IdleState>(new object[0]);
				}
			}
		}

		public override void OnDamage(float damage, Attacker attackedUnit)
		{
			if (!Model.IsDead())
			{
				base.OnDamage(damage, attackedUnit);
				Model.Health -= damage;
				if (Model.IsDead() && !(StateMachine.CurrentState is DeathState))
				{
					StateMachine.ChangeState<DeathState>(new object[0]);
				}
				if (_progressBar != null)
				{
					_progressBar.Progress = _model.Health / _model.MaxHealth;
				}
			}
		}

		public void OnRespawn()
		{
			if (_progressBar != null)
			{
				_progressBar.Progress = _model.Health / _model.MaxHealth;
			}
		}

		public void OnMove(float speed)
		{
			StateMachine.ChangeState<MoveState>(new object[3] { speed, 0.5f, MoveTarget });
		}
	}
}
