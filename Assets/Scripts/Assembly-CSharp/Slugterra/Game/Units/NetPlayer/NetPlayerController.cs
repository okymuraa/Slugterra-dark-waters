using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.Shops;
using Slugterra.Domain.UI.Game;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.NetPlayer.Strategy;
using Slugterra.Game.Units.Player;
using Slugterra.Game.Units.Player.Strategy;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.NetPlayer
{
	public class NetPlayerController : PlayerController
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float> OnDamageEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<SlugType> OnSlugShotEvent__BackingField;

		[SerializeField]
		private Transform _customLookPoint;

		[SerializeField]
		private PhotonView _photonView;

		[SerializeField]
		private int _oldHealth;

		[SerializeField]
		private float _rank;

		private InventoryItemView inventoryItemView;

		public float Rank
		{
			get
			{
				return _rank;
			}
			set
			{
				_rank = value;
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

		public event Action<SlugType> OnSlugShotEvent
		{
			add
			{
				Action<SlugType> action = OnSlugShotEvent__BackingField;
				Action<SlugType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSlugShotEvent__BackingField, (Action<SlugType>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<SlugType> action = OnSlugShotEvent__BackingField;
				Action<SlugType> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSlugShotEvent__BackingField, (Action<SlugType>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			IsLocal = _photonView.isMine;
			base.OnCreate();
			if (!IsLocal)
			{
				base.gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
				AttackPointLight.gameObject.SetActive(false);
				base.gameObject.name = _photonView.name;
			}
			if (IsLocal)
			{
				base.gameObject.tag = "Player";
				IconInventory.AddPlayer(Transform);
				if (_customLookPoint != null && PathFinder != null)
				{
					PathFinder.CustomLookPoint = _customLookPoint;
					PathFinder.MovementType = UnitMovementType.CustomLookPoint;
				}
				if ((bool)StateMachine)
				{
					StateMachine.ChangeState<IdleState>(new object[0]);
				}
				Rank = MonoSingleton<AppController>.Instance.Model.DuelSetting.Rank;
			}
			_model.AddSlug(new Slug(SlugType.Infurnus, 1));
			_model.AddSlug(new Slug(SlugType.Arachnet, 1));
			_model.AddSlug(new Slug(SlugType.Geoshard, 1));
			_model.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(BlasterType.LeaveBlaster, 6), BlasterType.LeaveBlaster);
			BlasterType = BlasterType.LeaveBlaster;
			_model.SetArmor(MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(ArmorType.Default, 0));
			InGameController inGameController = UnityEngine.Object.FindObjectOfType<InGameController>();
			InGameModel inGameModel = new InGameModel(LevelType.Action);
			inGameModel.AttackSlug = new Slug(SlugType.Infurnus);
			inGameModel.DefenceSlug = new Slug(SlugType.Arachnet);
			inGameModel.SpecialSlug = new Slug(SlugType.Geoshard);
			if (inGameController != null)
			{
				inGameController.SlugBarControllerAction.OnApplyModel(inGameModel);
				if (AttackTarget != null)
				{
					inGameController.NetPlayerAvatarInfo.OnApplyModel(AttackTarget.GetComponent<NetPlayerController>().Model);
				}
			}
			GameObject blasterPrefab = GameFactory.GetBlasterPrefab(BlasterType.Default);
			GameObject gameObject = UnityEngine.Object.Instantiate(blasterPrefab, ShotPoint.position, Quaternion.identity);
			gameObject.transform.parent = ShotPoint;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			ArmorItem armorItem = new ArmorItem(ArmorType.Default, 0, false, true);
			if (armorItem != null)
			{
				SetArmorView(armorItem.Type);
			}
			IsIgnorEnemy = true;
			if (!IsLocal)
			{
				EliView componentInChildren = Transform.GetComponentInChildren<EliView>();
				if (componentInChildren != null)
				{
					componentInChildren.UseMaskGhouled();
				}
			}
			UseAutoattack = false;
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			if (gameController != null)
			{
				gameController.Model.AddObserver(this);
			}
			_model.AddObserver(this);
		}

		protected void CreateProgressBar()
		{
			GameObject original = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
			_progressBar = UnityEngine.Object.Instantiate(original).GetComponent<ProgressBarController>();
			_progressBar.transform.parent = Transform;
			_progressBar.ShowText = true;
			_progressBar.SetText = Transform.parent.name;
			if ((bool)Transform.GetComponent<CharacterController>())
			{
				_progressBar.transform.localPosition = new Vector3(0f, Transform.GetComponent<CharacterController>().height + 0.333f, 0f);
			}
			else
			{
				_progressBar.transform.localPosition = new Vector3(0f, 2.2f, 0f);
			}
			_progressBar.Progress = _model.Health / _model.MaxHealth;
			_progressBar.Color = Color.red;
		}

		public override void OnReloadSlug(SlugType slugType)
		{
			if (CanAttack)
			{
				if (ShootCounter == 0)
				{
					int viewID = GetComponent<PhotonView>().viewID;
					PhotonNetwork.networkingPeer.SendReloadEvent(viewID, true);
				}
				base.OnReloadSlug(slugType);
			}
		}

		public void OnReloadFromMaster()
		{
			ShootCounter = 0;
			base.OnReloadSlug(SlugType.Infurnus);
		}

		protected override void OnAttackTargetChanged()
		{
			base.OnAttackTargetChanged();
			if (AttackTarget != null)
			{
				NetPlayerController component = AttackTarget.GetComponent<NetPlayerController>();
				if ((bool)component)
				{
					GameController.InGameController.NetPlayerAvatarInfo.OnApplyModel(component.Model);
					GameController.View.MultiplayerWinView.RivalNickName = component.gameObject.transform.root.name;
					GameController.View.MultiplayerLoseView.RivalNickName = component.gameObject.transform.root.name;
					GameController.View.MultiplayerWinView.RivalRank = component.Rank;
					GameController.View.MultiplayerLoseView.RivalRank = component.Rank;
				}
			}
		}

		public void UpdateProgressBar()
		{
			if ((bool)_progressBar)
			{
				_progressBar.SetText = Transform.parent.name;
			}
		}

		protected override Humanoid CreateModel()
		{
			if (IsLocal)
			{
				return GameController.Model.Eli;
			}
			return GameController.Model.P1Eli;
		}

		public override void OnStandUp()
		{
			if (IsLocal)
			{
				int viewID = GetComponent<PhotonView>().viewID;
				PhotonNetwork.networkingPeer.SendStandUpEvent(viewID, true);
				PhotonNetwork.networkingPeer.SendHPEvent(Model.Health, viewID, true);
			}
			base.OnStandUp();
		}

		public void OnStandUpFromNet()
		{
			IsFallDown = false;
			if (NewVersionAI)
			{
				StartCoroutine(MyBehavior.OnStandUp(1f));
			}
			Animator.SetTrigger("OnStandUp");
		}

		protected override IInputStrategy CreateInputStrategy()
		{
			if (IsLocal)
			{
				return new NetLocalTPActionInputStrategy(this);
			}
			return new NetRemoteTPActionInputStrategy(this);
		}

		protected override IBehaviorStrategy CreateBehaviorStrategy()
		{
			if (IsLocal)
			{
				return new NetLocalActionBehaviorStrategy(this);
			}
			return new NetRemoteActionBehaviorStrategy(this);
		}

		public override void OnUnitChanged(Humanoid unit)
		{
			if (_progressBar != null && _oldHealth != (int)_model.Health)
			{
				_progressBar.Progress = _model.Health / _model.MaxHealth;
			}
			if (IsLocal && _oldHealth != (int)_model.Health)
			{
				int viewID = GetComponent<PhotonView>().viewID;
				PhotonNetwork.networkingPeer.SendHPEvent(Model.Health, viewID, true);
				_oldHealth = (int)_model.Health;
			}
		}

		public override void OnBeginAttack()
		{
			ShootWithFlopper = true;
			if (AttackTarget != null)
			{
				AttackTarget = null;
				return;
			}
			List<Transform> list = BuildTargetsPriorityList();
			List<Transform> list2 = list.FindAll(CheckTargetInFOV);
			if (list2.Count > 0)
			{
				AttackTarget = list2[0];
			}
		}

		public void OnUpdateHPFromMaster(float health)
		{
			_model.Health = health;
			if (_progressBar != null)
			{
				_progressBar.Progress = _model.Health / _model.MaxHealth;
			}
			GameController.InGameController.NetPlayerAvatarInfo.OnUpdateHP();
		}

		public override void OnDamage(float damage, Attacker attackedUnit)
		{
			if (attackedUnit == null || attackedUnit.hitTransform == null)
			{
				UnityEngine.Debug.Log("attackedUnit.hitTransform = null!!!!");
			}
			else
			{
				if (attackedUnit.hitTransform.parent.name != transform.parent.name)
				{
					return;
				}
				if (!IsLocal && PhotonNetwork.isMasterClient)
				{
					Model.Health -= damage;
					if (_progressBar != null)
					{
						_progressBar.Progress = _model.Health / _model.MaxHealth;
					}
					if (Model.IsDead())
					{
						int viewID = GetComponent<PhotonView>().viewID;
						PhotonNetwork.networkingPeer.SendDieEvent(viewID, true);
					}
					else
					{
						PhotonNetwork.networkingPeer.SendDmgEvent(damage, attackedUnit, true);
					}
				}
				else
				{
					if (IsLocal && !PhotonNetwork.isMasterClient)
					{
						return;
					}
					base.OnDamage(damage, attackedUnit);
					float num = damage * Model.CurrentDamageCoeficient();
					if (!Model.IsDead())
					{
						if (GameController != null && GameController.InGameController != null && num > 0f && IsLocal)
						{
							GameController.InGameController.View.DamageController.Damage();
						}
						MonoSingleton<MissionStatisticsController>.Instance.Model.DamageEliTook += num;
					}
					if (_progressBar != null)
					{
						_progressBar.Progress = _model.Health / _model.MaxHealth;
					}
				}
			}
		}

		public void OnDamageFromMaster(float damage, Attacker attackedUnit, string hitTransformName)
		{
			if (hitTransformName != transform.parent.name || PhotonNetwork.isMasterClient || !IsLocal)
			{
				return;
			}
			base.OnDamage(damage, attackedUnit);
			float num = damage * Model.CurrentDamageCoeficient();
			if (!Model.IsDead())
			{
				if (GameController != null && GameController.InGameController != null && num > 0f)
				{
					GameController.InGameController.View.DamageController.Damage();
				}
				MonoSingleton<MissionStatisticsController>.Instance.Model.DamageEliTook += num;
			}
			if (_progressBar != null)
			{
				_progressBar.Progress = _model.Health / _model.MaxHealth;
			}
		}

		public void OnDeadFromMaster()
		{
			if (!PhotonNetwork.isMasterClient && IsLocal)
			{
				Model.Health = 0f;
				base.OnDead();
				if (_progressBar != null)
				{
					_progressBar.Progress = _model.Health / _model.MaxHealth;
				}
			}
		}

		public override void OnDead()
		{
			if (!PhotonNetwork.isMasterClient)
			{
				Model.Health = 0f;
			}
			else
			{
				base.OnDead();
			}
		}

		public void TakeDamage(float damage)
		{
		}

		public void RiseFireEvent(SlugType slugType)
		{
			if (OnSlugShotEvent__BackingField != null)
			{
				OnSlugShotEvent__BackingField(slugType);
			}
		}

		public void OnMove(float speed)
		{
			Model.CurrentSpeed = speed;
			StateMachine.ChangeState<MoveState>(new object[1] { 0.5f });
		}

		public void JoystickOnMove(Vector2 axies)
		{
			UnityEngine.Debug.Log(axies);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_customLookPoint != null)
			{
				_customLookPoint.position = Transform.position + Vector3.forward * 10f;
			}
		}

		protected override void RotateToAttackTarget()
		{
		}
	}
}
