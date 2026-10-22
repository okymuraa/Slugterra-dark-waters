using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.Shops;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Duel;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;
using Slugterra.Game.Units.Slugs;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Ally
{
	public class EliController : PlayerController
	{
		[SerializeField]
		private Transform _customLookPoint;

		public GameObject _playerDiamond;

		protected override void OnCreate()
		{
			base.OnCreate();
			if (_customLookPoint != null && PathFinder != null)
			{
				PathFinder.CustomLookPoint = _customLookPoint;
				PathFinder.MovementType = UnitMovementType.CustomLookPoint;
			}
			if ((bool)StateMachine)
			{
				StateMachine.ChangeState<Slugterra.Game.Units.Player.IdleState>(new object[0]);
			}
			GameObject blasterPrefab = GameFactory.GetBlasterPrefab(_model.Blaster.Type);
			GameController gameController = Object.FindObjectOfType<GameController>();
			if (gameController.IsMultiplayer)
			{
				blasterPrefab = GameFactory.GetBlasterPrefab(BlasterType.Default);
				_model.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(BlasterType.LeaveBlaster, 6), BlasterType.LeaveBlaster);
				BlasterType = BlasterType.LeaveBlaster;
				_model.SetArmor(MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(ArmorType.Default, 0));
			}
			GameObject gameObject = Object.Instantiate(blasterPrefab, ShotPoint.position, Quaternion.identity);
			gameObject.transform.parent = ShotPoint;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			ArmorItem armor = MonoSingleton<AppController>.Instance.Model.MissionSetting.Armor;
			if (gameController.IsMultiplayer)
			{
				SetArmorView(ArmorType.Default);
			}
			else
			{
				SetArmorView(armor.Type);
			}
			IsIgnorEnemy = true;
			GameController gameController2 = Object.FindObjectOfType<GameController>();
			if (gameController2 != null)
			{
				gameController2.Model.AddObserver(this);
			}
			if (!IsTPCMode)
			{
				return;
			}
			IconInventory.AddPlayer(Transform);
			InGameController inGameController = Object.FindObjectOfType<InGameController>();
			InGameModel inGameModel = new InGameModel(LevelType.Action);
			inGameModel.AttackSlug = new Slug(SlugType.Infurnus);
			inGameModel.DefenceSlug = new Slug(SlugType.Arachnet);
			inGameModel.SpecialSlug = new Slug(SlugType.Geoshard);
			if (inGameController != null)
			{
				inGameController.SlugBarControllerAction.OnApplyModel(inGameModel);
				if (AttackTarget != null)
				{
					inGameController.NetPlayerAvatarInfo.OnApplyModel(AttackTarget.GetComponent<BillyController>().Model);
				}
			}
		}

		protected override Humanoid CreateModel()
		{
			return GameController.Model.Eli;
		}

		protected override IInputStrategy CreateInputStrategy()
		{
			if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Type == LevelType.Arena)
			{
				return new DuelSplineInputStrategy(this);
			}
			if (IsTPCMode)
			{
				return new TPActionInputStrategy(this);
			}
			return new ActionInputStrategy(this);
		}

		protected override IBehaviorStrategy CreateBehaviorStrategy()
		{
			if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Type == LevelType.Arena)
			{
				return new DuelBehaviourStrategy(this);
			}
			return new ActionBehaviorStrategy(this);
		}

		public override void OnObjectChanged(object observable)
		{
			base.OnObjectChanged(observable);
			if (observable is GameModel)
			{
				OnGameModelChanged(observable as GameModel);
			}
		}

		protected virtual void OnGameModelChanged(GameModel model)
		{
			if (model.State == GameState.Pause || model.State == GameState.Minimap || model.State == GameState.CutScene)
			{
				DisableControl(typeof(GameModel));
				UseAutoattack = false;
			}
			else if (model.State == GameState.Play)
			{
				EnableControl(typeof(GameModel));
				UseAutoattack = true;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			GameController.OnPlayCutSceneEvent += OnPlayCutScene;
			if (BehaviorStrategy is DuelBehaviourStrategy && MonoSingleton<DuelManager>.Instance != null && AttackTarget != MonoSingleton<DuelManager>.Instance.BossTransform)
			{
				AttackTarget = MonoSingleton<DuelManager>.Instance.BossTransform;
			}
		}

		protected override void OnDisable()
		{
			if (GameController != null)
			{
				GameController.OnPlayCutSceneEvent -= OnPlayCutScene;
			}
			base.OnDisable();
		}

		private void OnPlayCutScene(bool isPlay)
		{
			if (isPlay)
			{
				Model.Invulnerability = true;
				UnitEffect[] components = GetComponents<UnitEffect>();
				UnitEffect[] array = components;
				foreach (UnitEffect unitEffect in array)
				{
					unitEffect.Finish();
				}
			}
			else
			{
				Model.Invulnerability = false;
			}
			InputStrategy.OnRelease(Vector3.zero, 0f);
		}

		public override void OnDamage(float damage, Attacker attackedUnit)
		{
			base.OnDamage(damage, attackedUnit);
			float num = damage * Model.CurrentDamageCoeficient();
			if (!Model.IsDead())
			{
				if (GameController != null && GameController.InGameController != null && num > 0f)
				{
					GameController.InGameController.View.DamageController.Damage();
				}
				MonoSingleton<MissionStatisticsController>.Instance.Model.DamageEliTook += num;
				return;
			}
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel.Scene == typeof(DesertBase1Multiplayer) || currentGameLevel.Scene == typeof(EastSet1Multiplayer))
			{
				GameController gameController = Object.FindObjectOfType<GameController>();
				gameController.OnResultmultiplayerFail();
			}
		}

		public void OnMove(float speed)
		{
			Model.CurrentSpeed = speed;
			StateMachine.ChangeState<Slugterra.Game.Units.Player.MoveState>(new object[1] { 0.5f });
		}

		public void JoystickOnMove(Vector2 axies)
		{
			Debug.Log(axies);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_customLookPoint != null)
			{
				_customLookPoint.position = Transform.position + Vector3.forward * 10f;
			}
		}
	}
}
