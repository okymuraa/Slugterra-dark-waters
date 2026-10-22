using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Units.Player;
using Slugterra.Scene.Game;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	public class AssistantController : FollowerController, IObserver
	{
		public PlayerController PlayerController { get; set; }

		public override Transform AttackTarget
		{
			get
			{
				return base.AttackTarget;
			}
			set
			{
				if (base.AttackTarget != null)
				{
					UnitController component = base.AttackTarget.GetComponent<UnitController>();
					if (component != null)
					{
						component.Model.RemoveObserver(this);
					}
				}
				base.AttackTarget = value;
				if (base.AttackTarget != null)
				{
					UnitController component2 = base.AttackTarget.GetComponent<UnitController>();
					if (component2 != null)
					{
						component2.Model.AddObserver(this);
					}
				}
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Unit)
			{
				Unit unit = observable as Unit;
				if (unit.IsDead())
				{
					AttackTarget = null;
				}
			}
		}

		protected override void OnCreate()
		{
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			int level = missionSetting.Assistant.Level;
			HumanoidLevels humanoidLevels = ((MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ShaneTutor_0_1)) ? instance.UnitConfigurationManager.GetAlliesConfigurationLevel(Type, level) : instance.UnitConfigurationManager.GetAlliesConfigurationLevel(Type, 0));
			_model = new Humanoid(humanoidLevels);
			_model.SetArmor(instance.ArmorConfigManager.GetArmorParams(humanoidLevels.ArmorType, humanoidLevels.ArmorLevel));
			_model.SetBlaster(instance.BlasterConfigManager.GetBlasterParams(humanoidLevels.BlasterType, humanoidLevels.BlasterLevel), humanoidLevels.BlasterType);
			if (typeof(ProntoController) != GetType())
			{
				GameObject blasterPrefab = GameFactory.GetBlasterPrefab(_model.Blaster.Type);
				GameObject gameObject = Object.Instantiate(blasterPrefab, ShotPoint.position, Quaternion.identity);
				gameObject.transform.parent = ShotPoint;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localEulerAngles = Vector3.zero;
			}
			_gameController = Object.FindObjectOfType<GameController>();
			_gameController.Model.Assistant = _model;
			CreateAssistantObserver();
		}

		protected override void OnStart()
		{
			base.OnStart();
			PlayerController = _playerTransform.GetComponent<PlayerController>();
			if (PlayerController == null)
			{
				PlayerController = PlayerManager.LocalPlayer;
				_playerTransform = PlayerController.Transform;
				Debug.LogWarning("ProntoController: PlayerController is equal null");
			}
			if (_gameController.AssistantSpawnPoint != null)
			{
				Vector3 eulerAngles = _gameController.AssistantSpawnPoint.eulerAngles;
				Transform.eulerAngles = eulerAngles;
			}
			MonoCommand.ExecuteOn<ShieldCommand>(GameObject, new object[2] { Model, 3f });
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			ViewArea.OnTriggerExitEvent += OnViewAreaExit;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			ViewArea.OnTriggerExitEvent -= OnViewAreaExit;
		}

		private void OnViewAreaExit(Transform target)
		{
			if (AttackTarget != null && AttackTarget.Equals(target))
			{
				AttackTarget = null;
			}
		}

		protected override void OnAttackTargetChanged()
		{
			if (AttackTarget == null)
			{
				Animator.SetBool("EnemyInSight", false);
				RotateToTarget = false;
				LinkCommand component = GameObject.GetComponent<LinkCommand>();
				if (component != null)
				{
					component.Finish();
				}
			}
			else
			{
				Animator.SetBool("EnemyInSight", true);
				RotateToTarget = true;
				LinkCommand linkCommand = MonoCommand.ExecuteOn<LinkCommand>(GameObject, new object[1] { AttackTarget });
				linkCommand.AsyncToken.AddResponder(new Responder<LinkCommand>(OnLinkFinish));
			}
		}

		private void OnLinkFinish(LinkCommand command)
		{
			AttackTarget = null;
		}

		private void CreateAssistantObserver()
		{
			if (MonoSingleton<AppController>.Instance.Model.MissionSetting.Assistant.Type == Type)
			{
				string text = string.Format("{0}StateObserver", Type.ToString());
				GameObject gameObject = new GameObject(text);
				AssistantStateObserver assistantStateObserver = gameObject.AddComponent<AssistantStateObserver>();
				assistantStateObserver.SetAssistant(this);
			}
		}

		protected override void OnUpdate()
		{
			if (AttackTarget != null && LayerMask.LayerToName(AttackTarget.gameObject.layer) == "AllyLayer")
			{
				AttackTarget = null;
			}
		}
	}
}
