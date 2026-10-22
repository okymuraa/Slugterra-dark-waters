using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.State;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Environment;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.NetPlayer;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Slugs;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;

namespace Slugterra.Game.Duel
{
	public class DuelManager : MonoSingleton<DuelManager>, IObserver, IStateMachineController
	{
		private HumanoidController _bossController;

		private Transform _bossTransform;

		private PlayerController _playerController;

		private DuelSmoothFollow _duelSmoothFollow;

		private SlugSmoothFollow _slugSmoothFollow;

		private StateMachine _stateMachine;

		private Camera _uiCamera;

		private List<SlugController> _enemySlugs = new List<SlugController>();

		private List<SlugController> _allySlugs = new List<SlugController>();

		private GameObject _gameObject;

		private GameObject _slugCamera;

		public Transform BossTransform
		{
			get
			{
				return _bossTransform;
			}
		}

		public PlayerController PlayerController
		{
			get
			{
				return _playerController;
			}
		}

		public DuelSmoothFollow DuelSmothFollow
		{
			get
			{
				return _duelSmoothFollow;
			}
		}

		public SlugSmoothFollow SlugSmoothFollow
		{
			get
			{
				return _slugSmoothFollow;
			}
		}

		public StateMachine StateMachine
		{
			get
			{
				return _stateMachine;
			}
		}

		public Camera UICamera
		{
			get
			{
				return _uiCamera;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return _gameObject;
			}
		}

		public void AddAllySlug(SlugController slugController)
		{
			if (!_allySlugs.Contains(slugController))
			{
				_allySlugs.Add(slugController);
			}
		}

		public void RemoveAllySlug(SlugController slugController)
		{
			if (_allySlugs.Contains(slugController))
			{
				_allySlugs.Remove(slugController);
			}
		}

		public void SetActivateAllySlugs(bool active)
		{
			foreach (SlugController allySlug in _allySlugs)
			{
				allySlug.gameObject.SetActive(active);
			}
		}

		protected override void OnCreate()
		{
			_gameObject = base.gameObject;
			GameObject original = Resources.Load("Prefabs/UI/DuelSlugCamera") as GameObject;
			GameObject gameObject = Object.Instantiate(original);
			gameObject.SetActive(false);
			_slugSmoothFollow = gameObject.GetComponent<SlugSmoothFollow>();
			_stateMachine = GameObject.AddComponent<StateMachine>();
			_stateMachine.Controller = this;
			_stateMachine.OnStateChanged += OnStateChanged;
			Camera[] allCameras = Camera.allCameras;
			foreach (Camera camera in allCameras)
			{
				if (camera.gameObject.layer == LayerMask.NameToLayer("UI") && camera.gameObject.CompareTag("MainUICamera"))
				{
					_uiCamera = camera;
					break;
				}
			}
		}

		public void Next(MonoState state)
		{
			_bossController.AttackTarget = _playerController.Transform;
			_bossController.RotateToTarget = true;
		}

		public void Initialize(PlayerController player, HumanoidController boss)
		{
			_playerController = player;
			_bossController = boss;
			_bossTransform = boss.transform;
			_playerController.IsIgnorEnemy = false;
			_playerController.AttackTarget = _bossTransform;
			_playerController.Model.AddObserver(this);
			_bossController.AttackTarget = _playerController.Transform;
			_bossController.RotateToTarget = true;
			_bossController.Model.AddObserver(this);
			if (boss is NetPlayerController)
			{
				InitializeNetworkBoss(boss as NetPlayerController);
			}
			else
			{
				InitializeLocalBoss(boss);
			}
			_duelSmoothFollow = Object.FindObjectOfType<DuelSmoothFollow>();
			_duelSmoothFollow.OnDuel(_playerController.Transform, BossTransform);
			StateMachine.ChangeState<FightState>(new object[0]);
			ArenaInteractiveEnvironmant[] array = Object.FindObjectsOfType<ArenaInteractiveEnvironmant>();
			ArenaInteractiveEnvironmant[] array2 = array;
			foreach (ArenaInteractiveEnvironmant arenaInteractiveEnvironmant in array2)
			{
				if (!(Mathf.Abs(arenaInteractiveEnvironmant.transform.position.z - _playerController.Transform.position.z) < 12f))
				{
					continue;
				}
				Collider[] componentsInChildren = arenaInteractiveEnvironmant.GetComponentsInChildren<Collider>();
				Collider[] array3 = componentsInChildren;
				foreach (Collider collider in array3)
				{
					if (LayerMask.LayerToName(collider.gameObject.layer) == "TouchLayer")
					{
						Object.Destroy(collider);
					}
				}
				if (arenaInteractiveEnvironmant._glow != null)
				{
					Object.Destroy(arenaInteractiveEnvironmant._glow);
				}
				if (arenaInteractiveEnvironmant._glow2 != null)
				{
					Object.Destroy(arenaInteractiveEnvironmant._glow2);
				}
			}
		}

		private void InitializeLocalBoss(HumanoidController boss)
		{
			boss.StateMachine.ChangeState<BossIdleState>(new object[1] { (boss as EnemyController).WaitInWarPointTime });
		}

		private void InitializeNetworkBoss(NetPlayerController boss)
		{
		}

		public void AddEnemySlug(SlugController slugController)
		{
			if (!_enemySlugs.Contains(slugController))
			{
				_enemySlugs.Add(slugController);
			}
		}

		public Transform IsDetectCollision(Transform playerSlug)
		{
			if (_stateMachine.CurrentState is SlugFightState)
			{
				return null;
			}
			for (int i = 0; i < _enemySlugs.Count; i++)
			{
				if (_enemySlugs[i] == null)
				{
					_enemySlugs.RemoveAt(i);
					i--;
					continue;
				}
				Transform transform = _enemySlugs[i].Transform;
				if (Vector3.SqrMagnitude(playerSlug.position - transform.position) <= 2.25f)
				{
					return transform;
				}
			}
			return null;
		}

		public void SetBossTarget()
		{
			_bossController.AttackTarget = _playerController.Transform;
		}

		public void OnObjectChanged(object observable)
		{
			if (!(observable is Unit) || !(observable as Unit).IsDead())
			{
				return;
			}
			StateMachine.ChangeState<FightState>(new object[0]);
			foreach (SlugController allySlug in _allySlugs)
			{
				if (allySlug != null)
				{
					allySlug.SelfDestruction();
				}
			}
			foreach (SlugController enemySlug in _enemySlugs)
			{
				if (enemySlug != null)
				{
					enemySlug.SelfDestruction();
				}
			}
			if (_bossController.Model.IsDead())
			{
				Object.FindObjectOfType<GameController>().OnFinishGame();
			}
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (_playerController != null && _playerController.Model != null)
			{
				_playerController.Model.RemoveObserver(this);
			}
			if (_bossController != null && _bossController.Model != null)
			{
				_bossController.Model.RemoveObserver(this);
			}
			if (_stateMachine != null)
			{
				_stateMachine.OnStateChanged -= OnStateChanged;
			}
		}

		private void OnStateChanged(MonoState nextState)
		{
			if (nextState is SlugFlyState)
			{
				SlugSmoothFollow.gameObject.SetActive(true);
				DuelSmothFollow.gameObject.SetActive(false);
				if (_slugCamera != null)
				{
					Object.Destroy(_slugCamera);
				}
			}
			else if (nextState is FightState)
			{
				SlugSmoothFollow.gameObject.SetActive(false);
				DuelSmothFollow.gameObject.SetActive(true);
				if (_slugCamera != null)
				{
					Object.Destroy(_slugCamera);
				}
			}
			else if (nextState is SlugFightState)
			{
				SlugSmoothFollow.gameObject.SetActive(false);
				DuelSmothFollow.gameObject.SetActive(false);
				if (_slugCamera != null)
				{
					_slugCamera.SetActive(true);
				}
			}
		}

		public void CreateSlugCamera(SlugController _playerSlug, SlugController _enemySlug)
		{
			Vector3 vector = new Vector3(_enemySlug.transform.position.x, _playerSlug.transform.position.y, _enemySlug.transform.position.z);
			Vector3 normalized = (vector - _playerSlug.transform.position).normalized;
			Vector3 position = _playerSlug.transform.position;
			_enemySlug.transform.position = position + normalized * 0.8f;
			_playerSlug.transform.position -= normalized * 0.8f;
			_enemySlug.transform.LookAt(_playerSlug.transform.position);
			_playerSlug.transform.LookAt(_enemySlug.transform.position);
			_enemySlug.enabled = false;
			_playerSlug.enabled = false;
			GameObject original = Resources.Load("Prefabs/UI/SlugFightCamera") as GameObject;
			_slugCamera = Object.Instantiate(original, position, Quaternion.identity);
			_slugCamera.transform.position -= new Vector3((0f - normalized.z) * 3f, -1f, normalized.x * 3f);
			_slugCamera.transform.LookAt(position);
		}
	}
}
