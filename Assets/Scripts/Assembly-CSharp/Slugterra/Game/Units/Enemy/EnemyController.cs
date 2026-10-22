using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using AppsMinistry.Core.State;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.ConfigStrategy;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	[Serializable]
	public abstract class EnemyController : HumanoidController, IObserver
	{
		public enum dropType
		{
			Array = 0,
			Single = 1
		}

		[Serializable]
		public class dropsSetup
		{
			[Serializable]
			public class items
			{
				public GameObject item;

				public byte roll;
			}

			public dropType DropType = dropType.Single;

			public float radiusDropMin = 0.5f;

			public float radiusDropMax = 1f;

			[HideInInspector]
			public int valueDrops;

			[HideInInspector]
			public byte i;

			public LayerMask LayerPhysicsMask;

			[HideInInspector]
			public RaycastHit hit;

			[HideInInspector]
			public Vector3 randomOffset;

			public items[] dropsItem;
		}

		[SerializeField]
		private PatrolPath _patrolPath;

		private int _movePathIndex;

		private int _runAwayCount;

		protected Humanoid _model;

		[SerializeField]
		private float _healthForHide = 0.6f;

		[SerializeField]
		private float _hideChance = 0.25f;

		[SerializeField]
		protected float _hideTime = 2f;

		private float _lastHide;

		[Range(0f, 1f)]
		[SerializeField]
		private float _moveToWarPointChance = 1f;

		[SerializeField]
		private float _waitInWarPointTime = 2f;

		[SerializeField]
		private float _preFireTime = 0.5f;

		[SerializeField]
		private GameObject _preFireEffect;

		[SerializeField]
		private bool _createProgressBar = true;

		public dropsSetup Drops = new dropsSetup();

		public virtual EnemyType mEnemyType
		{
			get
			{
				return EnemyType.None;
			}
		}

		public PatrolPath PatrolPath
		{
			get
			{
				return _patrolPath;
			}
		}

		public int MovePathIndex
		{
			get
			{
				return _movePathIndex;
			}
			set
			{
				_movePathIndex = value;
			}
		}

		public int RunAwayCount
		{
			get
			{
				return _runAwayCount;
			}
			set
			{
				_runAwayCount = value;
			}
		}

		public override Unit Model
		{
			get
			{
				return _model;
			}
		}

		public float MoveToWarPointChance
		{
			get
			{
				return _moveToWarPointChance;
			}
		}

		public float WaitInWarPointTime
		{
			get
			{
				return _waitInWarPointTime;
			}
			set
			{
				_waitInWarPointTime = value;
			}
		}

		public override Transform AttackTarget
		{
			get
			{
				return base.AttackTarget;
			}
			set
			{
				if (IsFallDown)
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
			}
		}

		private void DropItems()
		{
			if (Drops.DropType == dropType.Array)
			{
				Drops.i = 0;
				Drops.valueDrops = Drops.dropsItem.Length;
				while (Drops.valueDrops > 0)
				{
					if (UnityEngine.Random.Range(0, 100) < Drops.dropsItem[Drops.i].roll)
					{
						InstantiateItem();
					}
					Drops.i++;
					if (Drops.i == Drops.dropsItem.Length)
					{
						Drops.i = 0;
					}
					Drops.valueDrops--;
				}
			}
			if (Drops.DropType != dropType.Single)
			{
				return;
			}
			Drops.i = 0;
			Drops.valueDrops = Drops.dropsItem.Length;
			while (Drops.valueDrops > 0)
			{
				if (UnityEngine.Random.Range(0, 100) < Drops.dropsItem[Drops.i].roll)
				{
					InstantiateItem();
					break;
				}
				Drops.i++;
				if (Drops.i == Drops.dropsItem.Length)
				{
					Drops.i = 0;
				}
				Drops.valueDrops--;
			}
		}

		private void InstantiateItem()
		{
			Drops.randomOffset = Vector3.left * UnityEngine.Random.Range(Drops.radiusDropMin, Drops.radiusDropMax);
			Drops.randomOffset.y = Mathf.Abs(transform.position.y);
			Drops.randomOffset = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f) * Drops.randomOffset;
			Drops.randomOffset += transform.position;
			if (PhysicsFacade.Raycast(Drops.randomOffset + Vector3.up, -Vector3.up, out Drops.hit, (Mathf.Abs(transform.position.y) + 1f) * 2f, Drops.LayerPhysicsMask) && Drops.dropsItem[Drops.i].item != null)
			{
				UnityEngine.Object.Instantiate(Drops.dropsItem[Drops.i].item, Drops.hit.point, Quaternion.identity);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			SetAvailableSlugs();
			_model.AddObserver(this);
			_model.Clear(Time.time);
			_lastHide = Time.time;
			if (PathFinder != null)
			{
				PathFinder.repathRate = 0f;
				PathFinder.canSearch = false;
			}
			if (_createProgressBar)
			{
				CreateProgressBar();
			}
		}

		protected virtual void SetAvailableSlugs()
		{
			int num = 0;
			int num2 = 0;
			if (UseAdaptiveConfig)
			{
				GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
				IDictionary<SlugType, Slug> slugs = gameController.Model.Eli.Slugs;
				if (slugs != null)
				{
					foreach (KeyValuePair<SlugType, Slug> item in slugs)
					{
						if (num2 < item.Value.Level)
						{
							num2 = item.Value.Level;
						}
					}
				}
			}
			foreach (SlugType availableSlug in _availableSlugs)
			{
				int num3 = 1;
				if (_availableSlugsLevels.Count > num)
				{
					num3 = _availableSlugsLevels[num];
					if (UseAdaptiveConfig && num3 < num2)
					{
						num3 = num2;
					}
				}
				_model.AddSlug(new Slug(availableSlug, num3));
				num++;
			}
		}

		protected override void OnUseConfigStrategy()
		{
			EnemyConfigStrategy enemyConfigStrategy = new EnemyConfigStrategy(this);
			enemyConfigStrategy.LoadConfiguration(ref _model);
		}

		protected void CreateProgressBar()
		{
			GameObject original = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
			_progressBar = UnityEngine.Object.Instantiate(original).GetComponent<ProgressBarController>();
			_progressBar.transform.parent = Transform;
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

		protected override void OnStart()
		{
			base.OnStart();
			ViewArea.SetBaseTransform(Animator.transform);
			ViewArea.SetBaseLayer(gameObject.layer);
			_movePathIndex = 0;
			_runAwayCount = 0;
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			GameModel model = gameController.Model;
			RepaintProgressBarColor();
			model.AddObserver(this);
			OnGameModelChanged(model);
		}

		protected virtual void OnEnable()
		{
			ViewArea.OnTriggerEnterEvent += OnPlayerAreaEnter;
			ViewArea.OnTriggerExitEvent += OnPlayerAreaExit;
		}

		protected virtual void OnDisable()
		{
			ViewArea.OnTriggerEnterEvent -= OnPlayerAreaEnter;
			ViewArea.OnTriggerExitEvent -= OnPlayerAreaExit;
		}

		protected virtual void OnPlayerAreaEnter(Transform item)
		{
		}

		protected virtual void OnPlayerAreaExit(Transform item)
		{
		}

		protected override void OnAttackTargetChanged()
		{
			if (!(Animator == null))
			{
				if (AttackTarget == null)
				{
					Animator.SetBool("EnemyInSight", false);
					RotateToTarget = false;
				}
				else
				{
					Animator.SetBool("EnemyInSight", true);
					RotateToTarget = true;
				}
			}
		}

		public override void OnDamage(float damage, Attacker attackedUnit)
		{
			if (!Model.IsDead())
			{
				base.OnDamage(damage, attackedUnit);
				Model.Health -= damage;
				if (_progressBar != null)
				{
					_progressBar.Progress = _model.Health / _model.MaxHealth;
				}
				if (Model.IsDead())
				{
					MonoSingleton<MissionStatisticsController>.Instance.OnEnemyKilled(attackedUnit, this);
					OnKilled();
				}
			}
		}

		protected bool CanHide()
		{
			if (_lastHide > Time.time)
			{
				return false;
			}
			_lastHide = Time.time + _hideTime;
			if (Model.Health / Model.MaxHealth > _healthForHide)
			{
				return false;
			}
			if (UnityEngine.Random.Range(0f, 1f) <= _hideChance)
			{
				return true;
			}
			return false;
		}

		public virtual void OnKilled()
		{
			DropItems();
			if (_progressBar != null)
			{
				UnityEngine.Object.Destroy(_progressBar.gameObject);
			}
		}

		public virtual void OnFire()
		{
			if (!CanAttack || NextFire > Time.time || AttackTarget == null)
			{
				return;
			}
			Humanoid humanoid = Model as Humanoid;
			if (!((double)Vector3.SqrMagnitude(Transform.position - AttackTarget.position) > Math.Pow(humanoid.Blaster.Range, 2.0)))
			{
				SlugType shotSlug = GetShotSlug();
				Slug slug = humanoid.GetSlug(shotSlug);
				if (slug != null && IsCanUseSlug(slug) && slug.Available)
				{
					MonoCommand.ExecuteOn<FireCommand>(gameObject, new object[4] { this, slug, _preFireEffect, _preFireTime });
					UpdateNextFire();
				}
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Unit)
			{
				Unit unit = observable as Unit;
				OnModelChanged(unit);
				if (unit.IsDead())
				{
					AttackTarget = null;
				}
			}
			if (observable is GameModel)
			{
				GameModel model = observable as GameModel;
				OnGameModelChanged(model);
			}
		}

		protected virtual void OnModelChanged(Unit model)
		{
		}

		protected virtual void OnGameModelChanged(GameModel model)
		{
			if (model.State == GameState.CutScene)
			{
				ViewArea.SetRadius(0f);
				ViewArea.Items.Clear();
				if (AttackTarget != null)
				{
					AttackTarget = null;
				}
			}
			else if ((Model as Humanoid).Blaster != null)
			{
				ViewArea.SetRadius((Model as Humanoid).Blaster.Range + 1f);
			}
			else
			{
				ViewArea.SetRadius(Model.ViewRadius);
			}
		}

		public override void Next(MonoState previousState)
		{
		}

		protected override void OnReleaseResource()
		{
			if (Model != null)
			{
				Model.RemoveObserver(this);
			}
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			if (gameController != null)
			{
				GameModel model = gameController.Model;
				model.RemoveObserver(this);
			}
		}

		public void OnMove(float speed)
		{
			if (!NewVersionAI)
			{
				StateMachine.ChangeState<MoveState>(new object[2] { MoveTarget.position, speed });
			}
			else
			{
				MoveTo(MoveTarget, speed);
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if ((StateMachine.CurrentState is HideState || StateMachine.CurrentState is ChaseState) && AttackTarget != null && RotateToTarget)
			{
				OnFire();
			}
		}
	}
}
