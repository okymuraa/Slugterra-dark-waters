using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.Input;
using AppsMinistry.Core.Sound;
using AppsMinistry.Core.State;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Blasters;
using Slugterra.Game.Duel;
using Slugterra.Game.Environment;
using Slugterra.Game.Item;
using Slugterra.Game.Units.Animals;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.Game.Units.Enemy.Deadweed;
using Slugterra.Game.Units.NetPlayer;
using Slugterra.Game.Units.Player.Strategy;
using Slugterra.Game.Units.Slugs;
using Slugterra.Multiplayer.Player;
using Slugterra.Scene.Game;
using Slugterra.UI.Game;
using Slugterra.UI.Game.Camera;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Slugterra.Game.Units.Player
{
	public abstract class PlayerController : HumanoidController, IObserver
	{
		[SerializeField]
		private string _atackerName = "none";

		[SerializeField]
		private bool _isTPCMode;

		[SerializeField]
		protected PlayerView _view;

		[SerializeField]
		private PickUpAreaTrigger _pickUpArea;

		[SerializeField]
		private PlayerWaypoint _playerWaypoint;

		[SerializeField]
		private Transform _attackPointLight;

		[SerializeField]
		private Transform _aimPointLight;

		[SerializeField]
		private SoundContainer _slugLoadSound;

		[SerializeField]
		private SoundContainer _cheerSound;

		public List<SlugController> RealisedSlugs = new List<SlugController>();

		[SerializeField]
		private bool _canSwipe = true;

		[SerializeField]
		private bool _useAutoattack;

		[SerializeField]
		private NetworkPlayerRoot _networkRoot;

		[SerializeField]
		protected Humanoid _model;

		private bool _isIgnorEnemy;

		private Slug _nextSlug;

		private ReloadCommand _reloadCommand;

		private List<Type> _controllDisablers = new List<Type>();

		private bool _isControlEnabled;

		private Vector3 _swipeStartPosition = Vector3.zero;

		private int _shootCounter;

		[SerializeField]
		private ETCButton _button;

		private float _lastPlaySound;

		public Vector2 MoveAxises;

		private Vector3 _prevPosition;

		private float _nextReaim;

		private float _reaimTimeDelay = 0.25f;

		private Transform _aim;

		public Shader shader1;

		public Shader shader2;

		private Camera _camera;

		private CharacterController _characterController;

		[SerializeField]
		private AutoCam _cam;

		protected GameController _gameController;

		private Vector3 moveDir = Vector3.zero;

		private const float speedTPCModeMultipliy = 1.3f;

		public string AtackerName
		{
			get
			{
				return _atackerName;
			}
		}

		public bool IsTPCMode
		{
			get
			{
				return _isTPCMode;
			}
		}

		public PickUpAreaTrigger PickUpArea
		{
			get
			{
				return _pickUpArea;
			}
		}

		public PlayerWaypoint Waypoint
		{
			get
			{
				return _playerWaypoint;
			}
		}

		public Transform AttackPointLight
		{
			get
			{
				return _attackPointLight;
			}
		}

		public bool CanSwipe
		{
			get
			{
				return _canSwipe;
			}
			set
			{
				_canSwipe = value;
			}
		}

		public bool UseAutoattack
		{
			get
			{
				return _useAutoattack;
			}
			set
			{
				_useAutoattack = value;
			}
		}

		public NetworkPlayerRoot NetworkRoot
		{
			get
			{
				return _networkRoot;
			}
			set
			{
				_networkRoot = value;
			}
		}

		public override Unit Model
		{
			get
			{
				return _model;
			}
		}

		public IInputStrategy InputStrategy { get; protected set; }

		public IBehaviorStrategy BehaviorStrategy { get; protected set; }

		public bool WasAttackBegun { get; set; }

		public override Transform AttackTarget
		{
			get
			{
				return base.AttackTarget;
			}
			set
			{
				if (IsFallDown && (bool)value)
				{
					return;
				}
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
				BehaviorStrategy.OnAttackTargetChanged(value);
			}
		}

		public bool IsIgnorEnemy
		{
			get
			{
				return _isIgnorEnemy;
			}
			set
			{
				_isIgnorEnemy = value;
				if (AttackTarget != null)
				{
					if (_isIgnorEnemy)
					{
						RotateToTarget = false;
						Animator.SetBool("EnemyInSight", false);
					}
					else
					{
						RotateToTarget = true;
						Animator.SetBool("EnemyInSight", true);
					}
				}
			}
		}

		public Slug NextSlug
		{
			get
			{
				return _nextSlug;
			}
		}

		public ReloadCommand ReloadCommand
		{
			get
			{
				return _reloadCommand;
			}
		}

		public bool IsReloadSlug
		{
			get
			{
				return _reloadCommand != null;
			}
		}

		public FireCommand FireCommand { get; set; }

		public int ShootCounter
		{
			get
			{
				return _shootCounter;
			}
			set
			{
				if (0 <= value && value <= (Model as Humanoid).Blaster.MagazineCapacity)
				{
					_shootCounter = value;
				}
			}
		}

		public bool ShootWithFlopper { get; set; }

		public float NextReaim
		{
			get
			{
				return _nextReaim;
			}
		}

		public GameController GameController
		{
			get
			{
				if (_gameController == null)
				{
					_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
				}
				return _gameController;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			UseAutoattack = true;
			_model = CreateModel();
			_model.Clear(Time.time);
			BehaviorStrategy = CreateBehaviorStrategy();
			InputStrategy = CreateInputStrategy();
			PlayerManager.Add(this);
			GameModel model = GameController.Model;
			SetBlasterType(model.Eli.Blaster.Type);
			CreateBlasterObserver();
			CreateSlugObserver();
			shader1 = Shader.Find("Mobile/Diffuse_projector");
			shader2 = Shader.Find("Mobile/Diffuse_projectorOutline");
			_camera = Camera.main;
		}

		protected override void OnStart()
		{
			base.OnStart();
			_model.Clear(Time.time);
			if (_slugLoadSound != null)
			{
				_slugLoadSound.RegisterInSoundManager();
			}
			if (_cheerSound != null)
			{
				_cheerSound.RegisterInSoundManager();
			}
			ViewArea.SetBaseTransform(Animator.transform);
			ViewArea.SetBaseLayer(gameObject.layer);
			if (_isTPCMode)
			{
				ViewArea.SetAngle(30f);
				ViewArea.SetRadius((Model as Humanoid).Blaster.Range - 1f);
				DamageSystem.enabled = false;
			}
			else
			{
				ViewArea.SetAngle(360f);
			}
			_pickUpArea.SetBaseTransform(Animator.transform);
			EnableControl(GetType());
			_characterController = GetComponent<CharacterController>();
			Model.CurrentSpeed = Model.RunSpeed;
		}

		protected virtual void OnPlayerAreaEnter(Transform item)
		{
			if (UseAutoattack)
			{
				if (AttackTarget != null && AttackTarget.GetComponent<HumanoidController>() == null && item.GetComponent<HumanoidController>() != null)
				{
					AttackTarget = item;
				}
				if (AttackTarget == null && item != null)
				{
					AttackTarget = item;
				}
			}
		}

		protected virtual void OnPlayerAreaExit(Transform item)
		{
			if (UseAutoattack)
			{
				ViewArea.VisibleItems.Remove(item);
				if (AttackTarget != null && AttackTarget.Equals(item))
				{
					AttackTarget = null;
				}
			}
		}

		protected virtual void OnEnable()
		{
			if ((bool)StateMachine)
			{
				StateMachine.OnStateChanged += OnStateChanged;
			}
		}

		protected virtual void OnDisable()
		{
			if ((bool)StateMachine)
			{
				StateMachine.OnStateChanged -= OnStateChanged;
			}
			if (AttackTarget != null)
			{
				AttackTarget = null;
			}
		}

		protected override void OnUpdate()
		{
			BehaviorStrategy.OnUpdate();
			InputStrategy.OnUpdate();
			RealisedSlugs.RemoveAll((SlugController s) => s == null);
			if (_isTPCMode)
			{
				StartCoroutine(UpdateControl());
			}
			if (!_isTPCMode || !IsLocal)
			{
				return;
			}
			if ((bool)_aim)
			{
				_aimPointLight.position = _aim.position;
			}
			if (_nextReaim < Time.time)
			{
				if (AttackTarget == null)
				{
					List<Transform> list = BuildTargetsPriorityList();
					if (list.Count > 0)
					{
						Transform transform = list[0];
						AttackTarget = list[0];
						if (transform != _aim)
						{
							_aim = transform;
						}
						if (AttackTarget == null)
						{
							_aimPointLight.gameObject.SetActive(true);
						}
					}
					else if (_aim != null)
					{
						_aim = null;
						AttackTarget = null;
						_aimPointLight.gameObject.SetActive(false);
					}
					_nextReaim += _reaimTimeDelay;
				}
				else
				{
					List<Transform> list2 = BuildTargetsPriorityList();
					if (list2.Count == 0)
					{
						AttackTarget = null;
					}
					else
					{
						Transform transform2 = list2[0];
						if (AttackTarget != _aim)
						{
							AttackTarget = list2[0];
						}
					}
					_nextReaim += _reaimTimeDelay + 0.05f;
				}
			}
			if (AttackTarget != null)
			{
				if (RotateToTarget && Vector3.SqrMagnitude(AttackTarget.position - Transform.position) < 4f)
				{
					RotateToTarget = false;
				}
				else if (!RotateToTarget)
				{
					RotateToTarget = true;
				}
			}
		}

		protected abstract Humanoid CreateModel();

		protected abstract IInputStrategy CreateInputStrategy();

		protected abstract IBehaviorStrategy CreateBehaviorStrategy();

		public virtual void OnObjectChanged(object observable)
		{
			if (observable is Unit && (observable as Unit).IsDead())
			{
				BehaviorStrategy.OnEnemyKilled();
				AttackTarget = null;
			}
			if (observable is Humanoid)
			{
				OnUnitChanged(observable as Humanoid);
			}
		}

		public virtual void OnUnitChanged(Humanoid unit)
		{
		}

		private void OnStateChanged(MonoState state)
		{
			BehaviorStrategy.OnStateChanged(state as PlayerState);
		}

		protected override void OnAttackTargetChanged()
		{
			if (AttackTarget == null || IsIgnorEnemy)
			{
				Animator.SetBool("EnemyInSight", false);
				RotateToTarget = false;
				if (_attackPointLight.gameObject.activeSelf)
				{
					LinkCommand component = _attackPointLight.GetComponent<LinkCommand>();
					if (component != null)
					{
						component.Finish();
					}
				}
			}
			else
			{
				if (!IsFallDown)
				{
					Animator.SetBool("EnemyInSight", true);
				}
				if (!IsIgnorEnemy)
				{
					RotateToTarget = true;
				}
				LinkCommand linkCommand = MonoCommand.ExecuteOn<LinkCommand>(_attackPointLight.gameObject, new object[1] { AttackTarget });
				linkCommand.AsyncToken.AddResponder(new Responder<LinkCommand>(OnLinkFinish));
				_attackPointLight.gameObject.SetActive(true);
				if (_isTPCMode)
				{
					_aimPointLight.gameObject.SetActive(false);
				}
			}
			WasAttackBegun = false;
		}

		protected override void RotateToAttackTarget()
		{
			if (!_isTPCMode)
			{
				RotateToAttackTarget(Animator.transform);
			}
		}

		private void ChangeAttackTarget()
		{
			if (!IsIgnorEnemy)
			{
				Transform enemy = ViewArea.GetEnemy();
				if (enemy != null)
				{
					AttackTarget = enemy;
				}
			}
		}

		public void SetCanAttack(bool value)
		{
			CanAttack = value;
		}

		public void SetNextSlug(SlugType slugType)
		{
			if (_nextSlug == null || _nextSlug.Type != slugType)
			{
				_nextSlug = null;
				if (slugType != SlugType.None)
				{
					Slug slug = _model.GetSlug(slugType);
					_nextSlug = slug;
				}
			}
		}

		public void EnableControl(Type type)
		{
			if (!IsLocal)
			{
				return;
			}
			if (_controllDisablers.Contains(type))
			{
				_controllDisablers.Remove(type);
			}
			if (_controllDisablers.Count == 0 && !_isControlEnabled)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnTap;
				DonDestroyMonoSingleton<InputManager>.Instance.OnSwipeStart += OnSwipeStart;
				DonDestroyMonoSingleton<InputManager>.Instance.OnSwipeEnd += OnSwipeEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnRoll += OnRoll;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart += OnPress;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnRelease;
				DonDestroyMonoSingleton<InputManager>.Instance.OnDoubleTap += OnDoubleTap;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_movement += OnMove;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMoveEnd_movement += OnMoveEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_rotation += OnRotate;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMoveEnd_rotation += OnRotateEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnNextTarget += OnNextTarget;
				DonDestroyMonoSingleton<InputManager>.Instance.OnPrevTarget += OnPrevTarget;
				DonDestroyMonoSingleton<InputManager>.Instance.OnBeginAttack += OnBeginAttack;
				DonDestroyMonoSingleton<InputManager>.Instance.NextSlugEvent += OnNextSlug;
				DonDestroyMonoSingleton<InputManager>.Instance.PreviousSlugEvent += OnPreviousSlug;
				if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(DesertBase_1Test))
				{
					DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += OnTouch;
				}
				if ((bool)_button)
				{
					_button.onDown.AddListener(OnBeginAttack);
				}
				_isControlEnabled = true;
			}
		}

		public void DisableControl(Type type)
		{
			if (!IsLocal)
			{
				return;
			}
			if (DonDestroyMonoSingleton<InputManager>.Instance != null)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd -= OnTap;
				DonDestroyMonoSingleton<InputManager>.Instance.OnSwipeStart -= OnSwipeStart;
				DonDestroyMonoSingleton<InputManager>.Instance.OnSwipeEnd -= OnSwipeEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnRoll -= OnRoll;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart -= OnPress;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd -= OnRelease;
				DonDestroyMonoSingleton<InputManager>.Instance.OnDoubleTap -= OnDoubleTap;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_movement -= OnMove;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMoveEnd_movement -= OnMoveEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMove_rotation -= OnRotate;
				DonDestroyMonoSingleton<InputManager>.Instance.OnJoysticOnMoveEnd_rotation -= OnRotateEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnNextTarget -= OnNextTarget;
				DonDestroyMonoSingleton<InputManager>.Instance.OnPrevTarget -= OnPrevTarget;
				DonDestroyMonoSingleton<InputManager>.Instance.OnBeginAttack -= OnBeginAttack;
				DonDestroyMonoSingleton<InputManager>.Instance.NextSlugEvent -= OnNextSlug;
				DonDestroyMonoSingleton<InputManager>.Instance.PreviousSlugEvent -= OnPreviousSlug;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouch -= OnTouch;
				if ((bool)_button)
				{
					_button.onDown.RemoveListener(OnBeginAttack);
				}
				_isControlEnabled = false;
			}
			if (!_controllDisablers.Contains(type))
			{
				_controllDisablers.Add(type);
			}
			OnRelease(Vector3.zero, 0f);
		}

		public virtual void OnReloadSlug(SlugType slugType)
		{
			if (CanAttack)
			{
				if (_shootCounter == 0)
				{
					_reloadCommand = MonoCommand.ExecuteOn<ReloadCommand>(GameObject, new object[2] { this, slugType });
					_reloadCommand.AsyncToken.AddResponder(new Responder<ReloadCommand>(OnReloadFinish));
					_shootCounter = (Model as Humanoid).Blaster.MagazineCapacity;
				}
				else
				{
					SetNextSlug(slugType);
				}
			}
		}

		private void OnReloadFinish(ReloadCommand command)
		{
			_reloadCommand = null;
		}

		public void OnPickUp()
		{
			Transform transform = PickUpArea.Pop();
			ItemController componentInChildren = transform.GetComponentInChildren<ItemController>();
			componentInChildren.PickUp(_model);
		}

		public void SetArmorView(ArmorType type)
		{
			if (_view != null)
			{
				_view.SetArmor(type);
			}
		}

		private void OnPress(Vector3 mousePosition)
		{
			InputStrategy.OnPress(mousePosition);
		}

		private void OnRelease(Vector3 mousePosition, float actionTime)
		{
			InputStrategy.OnRelease(mousePosition, actionTime);
		}

		private void OnTap(Vector3 mousePosition, float actionTime)
		{
			InputStrategy.OnTap(mousePosition, actionTime);
		}

		private void OnSwipeStart(Vector3 startPosition)
		{
			_swipeStartPosition = startPosition;
		}

		private void OnSwipeClick(Vector2 position)
		{
			InputStrategy.OnSwipe(position);
		}

		private void OnSwipeEnd(Vector3 endPosition)
		{
			Vector3 vector = endPosition - _swipeStartPosition;
			float num = (float)Screen.width * 0.07f;
			if (!(vector.magnitude < (float)Screen.width * 0.07f))
			{
				InputStrategy.OnSwipe(vector);
			}
		}

		private void OnRoll(Vector2 direction, Vector2 move)
		{
			InputStrategy.OnRoll(direction, move);
		}

		private void OnDoubleTap(Vector3 mousePosition)
		{
			InputStrategy.OnDoubleTap(mousePosition);
		}

		public virtual void OnUseSlug(SlugType slugType)
		{
			InputStrategy.OnUseSlug(slugType);
		}

		public virtual void OnMove(Vector2 axies)
		{
			InputStrategy.OnMove(axies);
		}

		public virtual void OnMoveEnd()
		{
			InputStrategy.OnMoveEnd();
		}

		public virtual void OnRotate(Vector2 axies)
		{
			InputStrategy.OnRotate(axies);
		}

		public virtual void OnRotateEnd()
		{
			InputStrategy.OnRotateEnd();
		}

		public virtual void OnNextTarget()
		{
			InputStrategy.OnNextTarget();
		}

		public virtual void OnPrevTarget()
		{
			InputStrategy.OnPrevTarget();
		}

		public virtual void OnTouch(Vector3 position, float actionTime)
		{
			InputStrategy.OnTouch(position, actionTime);
		}

		public virtual void OnBeginAttack()
		{
			ShootWithFlopper = true;
			UseAutoattack = true;
			IsIgnorEnemy = false;
			RotateToTarget = true;
			if (AttackTarget == null || !AttackTarget.gameObject.activeSelf)
			{
				List<Transform> list = BuildTargetsPriorityList();
				List<Transform> list2 = list.FindAll(CheckTargetInFOV);
				if (list2.Count > 0)
				{
					AttackTarget = list2[0];
				}
				else if (list.Count > 0)
				{
					AttackTarget = list[0];
				}
			}
		}

		public void OnNextSlug()
		{
			CycleSlug(true);
		}

		public void OnPreviousSlug()
		{
			CycleSlug(false);
		}

		public void SelectSlugByIndex(int index)
		{
			Humanoid humanoid = Model as Humanoid;
			if (humanoid == null || humanoid.Slugs == null)
			{
				return;
			}
			List<SlugType> available = new List<SlugType>();
			foreach (KeyValuePair<SlugType, Slug> pair in humanoid.Slugs)
			{
				if (pair.Key != SlugType.Flopper && pair.Key != SlugType.None && pair.Value != null && pair.Value.Available)
				{
					available.Add(pair.Key);
				}
			}
			if (index >= 0 && index < available.Count)
			{
				OnUseSlug(available[index]);
			}
		}

		private void CycleSlug(bool forward)
		{
			Humanoid humanoid = Model as Humanoid;
			if (humanoid == null || humanoid.Slugs == null || humanoid.Slugs.Count == 0)
			{
				return;
			}
			List<SlugType> available = new List<SlugType>();
			foreach (KeyValuePair<SlugType, Slug> pair in humanoid.Slugs)
			{
				if (pair.Key != SlugType.Flopper && pair.Key != SlugType.None && pair.Value != null && pair.Value.Available)
				{
					available.Add(pair.Key);
				}
			}
			if (available.Count == 0)
			{
				return;
			}
			int currentIndex = -1;
			if (NextSlug != null)
			{
				currentIndex = available.IndexOf(NextSlug.Type);
			}
			int targetIndex;
			if (forward)
			{
				targetIndex = (currentIndex + 1) % available.Count;
			}
			else
			{
				targetIndex = (currentIndex - 1 + available.Count) % available.Count;
			}
			OnUseSlug(available[targetIndex]);
		}

		private void OnLinkFinish(LinkCommand command)
		{
			_attackPointLight.gameObject.SetActive(false);
			RotateToTarget = false;
			if (BehaviorStrategy is DuelBehaviourStrategy)
			{
				if (enabled && GameObject.activeSelf && MonoSingleton<DuelManager>.Instance != null)
				{
					AttackTarget = MonoSingleton<DuelManager>.Instance.BossTransform;
				}
			}
			else
			{
				AttackTarget = null;
			}
		}

		public override void OnDamage(float damage, Attacker attackedUnit)
		{
			if (!Model.IsDead())
			{
				base.OnDamage(damage, attackedUnit);
				_model.Health -= damage;
				if (Model.IsDead())
				{
					OnDead();
				}
			}
		}

		public virtual void OnDead()
		{
			if (StateMachine.CurrentState != null)
			{
				StateMachine.CurrentState.Finish();
			}
			OnFallDown();
			AttackTarget = null;
		}

		public override void OnFallDown()
		{
			base.OnFallDown();
			DisableControl(GetType());
		}

		public override void OnStandUp()
		{
			base.OnStandUp();
			if (Model != null && Model.IsDead())
			{
			}
		}

		protected override void OnStandUpComplited()
		{
			base.OnStandUpComplited();
			EnableControl(GetType());
			BehaviorStrategy.OnStandUp();
		}

		public SoundItem PlaySlugLoadSound()
		{
			return (!(_slugLoadSound != null) || !IsLocal) ? null : _slugLoadSound.Play();
		}

		public SoundItem PlayCheerSound()
		{
			return (!(_cheerSound != null) || !IsLocal) ? null : _cheerSound.Play();
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			DisableControl(GetType());
			PlayerManager.Remove(this);
		}

		protected void CreateBlasterObserver()
		{
			if (BlasterType != BlasterType.None)
			{
				string text = string.Format("{0}StateObserver", BlasterType.ToString());
				GameObject gameObject = new GameObject(text);
				BlasterStateObserver blasterStateObserver = gameObject.AddComponent<BlasterStateObserver>();
				blasterStateObserver.Activate(this);
			}
		}

		protected void CreateSlugObserver()
		{
			string text = "SlugStateObserver";
			GameObject gameObject = new GameObject(text);
			SlugStateObserver slugStateObserver = gameObject.AddComponent<SlugStateObserver>();
		}

		public List<Transform> BuildTargetsPriorityList()
		{
			List<Transform> list = new List<Transform>();
			ViewArea.VisibleItems.RemoveAll((Transform t) => t == null);
			list.AddRange(from t in ViewArea.VisibleItems.FindAll((Transform t) => t.GetComponent<HumanoidController>() != null && t.GetComponent<DeadweedController>() == null)
				orderby (t.position - Transform.position).magnitude
				select t);
			list.AddRange(from t in ViewArea.VisibleItems.FindAll((Transform t) => t.GetComponent<AnimalController>() != null)
				orderby (t.position - Transform.position).magnitude
				select t);
			list.AddRange(from t in ViewArea.VisibleItems.FindAll((Transform t) => t.GetComponent<ExplosionEnvironmentController>() != null)
				orderby (t.position - Transform.position).magnitude
				select t);
			list.AddRange(from t in ViewArea.VisibleItems.FindAll((Transform t) => t.GetComponent<ArenaInteractiveEnvironmant>() != null)
				orderby (t.position - Transform.position).magnitude
				select t);
			foreach (Transform visibleItem in ViewArea.VisibleItems)
			{
				if (!list.Contains(visibleItem))
				{
					list.Add(visibleItem);
				}
			}
			return list;
		}

		public bool CheckTargetInFOV(Transform target)
		{
			if (_camera == null)
			{
				return false;
			}
			if (target != null)
			{
				Vector3 vector = _camera.WorldToViewportPoint(target.position);
				if (vector.x < -0.01f || 1.01f < vector.x || vector.y < -0.01f || 1.01f < vector.y)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		private IEnumerator UpdateControl()
		{
			yield return new WaitForEndOfFrame();
			moveDir = Vector3.zero;
			Vector3 desiredMove = Transform.forward * MoveAxises.y + Transform.right * MoveAxises.x;
			moveDir.x = desiredMove.x;
			moveDir.y = 0f;
			moveDir.z = desiredMove.z;
			moveDir += Physics.gravity * 4f * Time.deltaTime;
			moveDir *= Model.CurrentSpeed * 1.3f;
			_characterController.Move(moveDir * Time.deltaTime);
			ProcessWalkAnimation(moveDir.magnitude);
			if (moveDir.x != 0f || moveDir.z != 0f)
			{
				ProcessWalkSound();
			}
			if ((bool)_cam)
			{
				_cam.ManualUpdate();
			}
		}

		private void ProcessWalkAnimation(float speed)
		{
			Vector3 direction = (Transform.position - _prevPosition) / Time.deltaTime;
			direction = Transform.InverseTransformDirection(direction);
			Vector2 vector = new Vector2(direction.x, direction.z);
			if (RotateToCommand != null)
			{
				if (RotateToTarget)
				{
					vector = MathUtil.Rotate(vector, RotateToCommand.Angle);
				}
				else if (!(this is NetPlayerController))
				{
					Animator.transform.localEulerAngles = Vector3.zero;
				}
			}
			Animator.SetFloat("VelocityX", vector.x);
			Animator.SetFloat("VelocityY", vector.y);
			_prevPosition = Transform.position;
		}

		private void ProcessWalkSound()
		{
			if (!((double)(Time.time - _lastPlaySound) < (double)(1f / Model.CurrentSpeed) * 1.5))
			{
				SoundItem soundItem = PlayWalkSound();
				if (soundItem != null)
				{
					soundItem.transform.parent = Transform;
					soundItem.transform.localPosition = Vector3.zero;
					_lastPlaySound = Time.time;
				}
			}
		}
	}
}
