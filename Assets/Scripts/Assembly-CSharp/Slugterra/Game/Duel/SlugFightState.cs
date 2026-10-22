using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.Input;
using AppsMinistry.Domain.Observer;
using Slugterra.Domain;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Slugs;
using Slugterra.UI.Game;
using Slugterra.UI.Game.SlugFight;
using UnityEngine;

namespace Slugterra.Game.Duel
{
	[PersistentCommand]
	public class SlugFightState : DuelState, IObserver
	{
		private struct SlugSetting
		{
			public Vector3 Position;

			public Vector3 Rotation;
		}

		public const float FightTime = 5f;

		public const float EnemyAttackDelay = 0.7f;

		public static readonly Vector2 PlayerDamageRange = new Vector2(0.024f, 0.05f);

		public static readonly Vector2 EnemyDamageRange = new Vector2(0.04f, 0.15f);

		private InGameModel _inGameModel;

		private PlayerController _playerController;

		private float _finishBattle;

		private float _hitTime;

		private SlugController _playerSlug;

		private SlugController _enemySlug;

		private float _playerHealthProgress;

		private float _enemyHealthProgress;

		private SlugSetting _oldPlayerSlugSetting;

		private SlugSetting _oldEnemySlugSetting;

		private float _nextAttack;

		private bool _isFinish;

		private float _finishWaitTime;

		private InGameModel InGameModel
		{
			get
			{
				if (_inGameModel == null)
				{
					InGameController inGameController = Object.FindObjectOfType<InGameController>();
					if (inGameController != null)
					{
						_inGameModel = inGameController.Model;
					}
				}
				return _inGameModel;
			}
		}

		public float FinishBattleTime
		{
			get
			{
				return _finishBattle;
			}
		}

		public SlugController PlayerSlug
		{
			get
			{
				return _playerSlug;
			}
		}

		public SlugController EnemySlug
		{
			get
			{
				return _enemySlug;
			}
		}

		public float PlayerHealthProgress
		{
			get
			{
				return _playerHealthProgress;
			}
		}

		public float EnemyHealthProgress
		{
			get
			{
				return _enemyHealthProgress;
			}
		}

		public bool IsFinish
		{
			get
			{
				return _isFinish;
			}
		}

		protected override void OnStart(object[] args)
		{
			Transform transform = (Transform)args[0];
			if (transform == null)
			{
				StateMachine.NextState<FightState>(new object[0]);
				Finish();
				return;
			}
			_playerSlug = ((Transform)args[0]).GetComponent<SlugController>();
			Transform transform2 = (Transform)args[1];
			if (transform2 == null)
			{
				StateMachine.NextState<SlugFlyState>(new object[1] { _playerSlug });
				Finish();
				return;
			}
			_oldPlayerSlugSetting = default(SlugSetting);
			_oldPlayerSlugSetting.Position = _playerSlug.transform.position;
			_oldPlayerSlugSetting.Rotation = _playerSlug.transform.eulerAngles;
			_playerSlug.Transform.GetComponent<Collider>().enabled = false;
			_enemySlug = ((Transform)args[1]).GetComponent<SlugController>();
			_oldEnemySlugSetting = default(SlugSetting);
			_oldEnemySlugSetting.Position = _enemySlug.transform.position;
			_oldEnemySlugSetting.Rotation = _enemySlug.transform.eulerAngles;
			_enemySlug.Transform.GetComponent<Collider>().enabled = false;
			_playerController = PlayerManager.LocalPlayer;
			_playerController.gameObject.SetActive(false);
			Controller.BossTransform.gameObject.SetActive(false);
			_playerController.AttackPointLight.gameObject.SetActive(false);
			_finishBattle = Time.time + 5f;
			_hitTime = Time.time;
			_nextAttack = Time.time;
			Controller.UICamera.gameObject.SetActive(false);
			_playerHealthProgress = (_enemyHealthProgress = 1f);
			_isFinish = false;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart += OnPress;
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvent(InputEvent.Press);
			if (ManualMonoSingleton<SlugFightUICameraView>.ManualInstance != null)
			{
				ManualMonoSingleton<SlugFightUICameraView>.ManualInstance.Show(true);
				ManualMonoSingleton<SlugFightUICameraView>.ManualInstance.FightController.ApplyModel(this);
			}
			MonoSingleton<DuelManager>.Instance.RemoveAllySlug(_playerSlug);
			MonoSingleton<DuelManager>.Instance.SetActivateAllySlugs(false);
			if (InGameModel != null)
			{
				InGameModel.CurrentDuelState = this;
			}
			Object.FindObjectOfType<GameController>().Model.AddObserver(this);
			_playerSlug.Animator.SetTrigger("OnIdle");
		}

		private void OnPress(Vector3 position)
		{
			Attack();
		}

		public void Attack()
		{
			if (!_isFinish)
			{
				float enemyHealthProgress = _enemyHealthProgress;
				Vector2 playerDamageRange = PlayerDamageRange;
				float x = playerDamageRange.x;
				Vector2 playerDamageRange2 = PlayerDamageRange;
				_enemyHealthProgress = Mathf.Clamp01(enemyHealthProgress - Random.Range(x, playerDamageRange2.y));
				if (_nextAttack < Time.time)
				{
					_playerSlug.Animator.SetTrigger("OnAttack");
					_nextAttack = Time.time + 0.5f;
				}
			}
		}

		protected override void OnUpdate()
		{
			if (_isFinish)
			{
				if (_finishWaitTime < Time.time)
				{
					FinishFight();
				}
			}
			else if (_finishBattle < Time.time || _playerHealthProgress <= 0f || _enemyHealthProgress <= 0f)
			{
				_isFinish = true;
				_enemySlug.Animator.ResetTrigger("OnAttack");
				_playerSlug.Animator.ResetTrigger("OnAttack");
				_finishWaitTime = Time.time + 2.1f;
				if (ManualMonoSingleton<SlugFightUICameraView>.ManualInstance != null)
				{
					ManualMonoSingleton<SlugFightUICameraView>.ManualInstance.Show(false);
				}
				switch (GetResult())
				{
				case SlugFightResult.Draw:
					_enemySlug.Animator.SetTrigger("OnDamage");
					_playerSlug.Animator.SetTrigger("OnDamage");
					break;
				case SlugFightResult.Player:
					_enemySlug.Animator.SetTrigger("OnDamage");
					_enemySlug.ParticleLoop(false);
					_playerSlug.Animator.SetTrigger("OnTaunt");
					_playerSlug.ParticleLoop(false);
					break;
				default:
					_enemySlug.Animator.SetTrigger("OnTaunt");
					_enemySlug.ParticleLoop(false);
					_playerSlug.Animator.SetTrigger("OnDamage");
					_playerSlug.ParticleLoop(false);
					break;
				}
			}
			else if (_hitTime < Time.time)
			{
				float playerHealthProgress = _playerHealthProgress;
				Vector2 enemyDamageRange = EnemyDamageRange;
				float x = enemyDamageRange.x;
				Vector2 enemyDamageRange2 = EnemyDamageRange;
				_playerHealthProgress = Mathf.Clamp01(playerHealthProgress - Random.Range(x, enemyDamageRange2.y));
				_enemySlug.Animator.SetTrigger("OnAttack");
				_hitTime = Time.time + 0.7f;
			}
		}

		private SlugFightResult GetResult()
		{
			if (_enemyHealthProgress <= 0f)
			{
				return SlugFightResult.Player;
			}
			if (_playerHealthProgress <= 0f)
			{
				return SlugFightResult.Enemy;
			}
			if (Mathf.Abs(_playerHealthProgress - _enemyHealthProgress) <= 0.07f)
			{
				return SlugFightResult.Draw;
			}
			if (_enemyHealthProgress > _playerHealthProgress)
			{
				return SlugFightResult.Enemy;
			}
			return SlugFightResult.Player;
		}

		private void FinishFight()
		{
			switch (GetResult())
			{
			case SlugFightResult.Draw:
				_playerSlug.SelfDestruction();
				_enemySlug.SelfDestruction();
				StateMachine.NextState<FightState>(new object[0]);
				break;
			case SlugFightResult.Player:
				_playerSlug.enabled = true;
				_playerSlug.transform.position = _oldPlayerSlugSetting.Position;
				_playerSlug.transform.eulerAngles = _oldPlayerSlugSetting.Rotation;
				_enemySlug.SelfDestruction();
				StateMachine.NextState<SlugFlyState>(new object[1] { _playerSlug });
				_playerSlug.Transform.GetComponent<Collider>().enabled = true;
				MonoSingleton<DuelManager>.Instance.AddAllySlug(_playerSlug);
				break;
			default:
				_enemySlug.enabled = true;
				_enemySlug.transform.position = _oldEnemySlugSetting.Position;
				_enemySlug.transform.eulerAngles = _oldEnemySlugSetting.Rotation;
				_playerSlug.SelfDestruction();
				StateMachine.NextState<FightState>(new object[0]);
				_enemySlug.Transform.GetComponent<Collider>().enabled = true;
				break;
			}
			Finish();
		}

		protected override void OnFinish()
		{
			if (DonDestroyMonoSingleton<InputManager>.Instance != null)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvent(InputEvent.Press);
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart -= OnPress;
			}
			MonoSingleton<DuelManager>.Instance.SetActivateAllySlugs(true);
			if (Controller != null)
			{
				Controller.UICamera.gameObject.SetActive(true);
				Controller.BossTransform.gameObject.SetActive(true);
				if (_playerController != null)
				{
					_playerController.gameObject.SetActive(true);
					_playerController.AttackPointLight.gameObject.SetActive(true);
					_playerController.AttackTarget = Controller.BossTransform;
				}
				Controller.SetBossTarget();
			}
			if (InGameModel != null)
			{
				InGameModel.CurrentDuelState = null;
			}
			GameController gameController = Object.FindObjectOfType<GameController>();
			if (gameController != null)
			{
				gameController.Model.RemoveObserver(this);
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is GameModel)
			{
				GameModel gameModel = observable as GameModel;
				if (gameModel.State == GameState.Complete || gameModel.State == GameState.Defeat)
				{
					Finish();
				}
			}
		}
	}
}
