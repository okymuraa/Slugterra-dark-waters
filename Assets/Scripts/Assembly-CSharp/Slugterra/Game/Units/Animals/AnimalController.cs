using System;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.Game.Units;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.ConfigStrategy;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Animals
{
	[Serializable]
	public abstract class AnimalController : UnitController, IObserver
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

		[SerializeField]
		private Aggression _aggression;

		public dropsSetup Drops = new dropsSetup();

		private Animal _model;

		protected float _attackDistance;

		public virtual AnimalType mAnimalType
		{
			get
			{
				return AnimalType.None;
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

		public Aggression Aggresion
		{
			get
			{
				return _aggression;
			}
		}

		public override Unit Model
		{
			get
			{
				return _model;
			}
		}

		public float AttackDistance
		{
			get
			{
				return _attackDistance;
			}
		}

		private void DropItems()
		{
			Drops.i = 0;
			Drops.valueDrops = Drops.dropsItem.Length;
			while (Drops.valueDrops > 0)
			{
				if (UnityEngine.Random.Range(0, 100) < Drops.dropsItem[Drops.i].roll)
				{
					InstantiateItem();
					if (Drops.DropType == dropType.Single)
					{
						return;
					}
				}
				Drops.i++;
				if (Drops.i == Drops.dropsItem.Length)
				{
					Drops.i = 0;
				}
				Drops.valueDrops--;
			}
			Drops.dropsItem = new dropsSetup.items[0];
		}

		private void InstantiateItem()
		{
			Drops.randomOffset = Vector3.left * UnityEngine.Random.Range(Drops.radiusDropMin, Drops.radiusDropMax);
			Drops.randomOffset.y = Mathf.Abs(transform.position.y);
			Drops.randomOffset = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f) * Drops.randomOffset;
			Drops.randomOffset += transform.position;
			if (Physics.Raycast(Drops.randomOffset + Vector3.up, -Vector3.up, out Drops.hit, (Mathf.Abs(transform.position.y) + 1f) * 2f, Drops.LayerPhysicsMask) && Drops.dropsItem[Drops.i].item != null)
			{
				UnityEngine.Object.Instantiate(Drops.dropsItem[Drops.i].item, Drops.hit.point, Quaternion.identity);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			AnimalConfigStrategy animalConfigStrategy = new AnimalConfigStrategy(this);
			animalConfigStrategy.LoadConfiguration(ref _model);
			PathFinder.endReachedDistance = 0.4f;
			PathFinder.repathRate = 0f;
			PathFinder.canSearch = false;
			CreateProgressBar();
		}

		protected void CreateProgressBar()
		{
			GameObject original = Resources.Load("Prefabs/UI/LineProgressBar") as GameObject;
			_progressBar = UnityEngine.Object.Instantiate(original).GetComponent<ProgressBarController>();
			_progressBar.transform.parent = Transform;
			_progressBar.transform.localPosition = new Vector3(0f, 1.2f, 0f);
		}

		protected override void OnStart()
		{
			base.OnStart();
			ViewArea.SetBaseTransform(Animator.transform);
			ViewArea.SetBaseLayer(gameObject.layer);
			_model.AddObserver(this);
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			GameModel model = gameController.Model;
			model.AddObserver(this);
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Unit)
			{
				OnModelChanged(observable as Unit);
			}
			if (observable is GameModel)
			{
				GameModel model = observable as GameModel;
				OnGameModelChanged(model);
			}
		}

		protected virtual void OnModelChanged(Unit model)
		{
			if (_model.Equals(model))
			{
				if (_model.IsDead())
				{
					OnKilled();
				}
				else
				{
					_progressBar.Progress = _model.Health / _model.MaxHealth;
				}
			}
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
			else
			{
				ViewArea.SetRadius(Model.ViewRadius);
			}
		}

		public override void OnDamage(float damage, Attacker attackedUnit = null)
		{
			base.OnDamage(damage, attackedUnit);
			if (Model.IsDead())
			{
				MonoSingleton<MissionStatisticsController>.Instance.OnEnemyKilled(attackedUnit, this);
			}
		}

		public virtual void OnKilled()
		{
			DropItems();
			if (_progressBar != null)
			{
				UnityEngine.Object.Destroy(_progressBar.gameObject);
				_progressBar = null;
			}
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (_model != null)
			{
				_model.RemoveObserver(this);
			}
			GameController gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			if (gameController != null)
			{
				GameModel model = gameController.Model;
				model.RemoveObserver(this);
			}
		}
	}
}
