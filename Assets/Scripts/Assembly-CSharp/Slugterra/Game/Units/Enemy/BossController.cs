using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.State;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game.Duel;
using Slugterra.Game.Environment;
using Slugterra.Game.Units.ConfigStrategy;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy
{
	public class BossController : EnemyController
	{
		private GameObject _bossLight;

		private Slug _lastUsedSlug;

		public DuelFireCommand FireCommand;

		private List<ArenaInteractiveEnvironmant> _interactiveObjects;

		public List<ArenaInteractiveEnvironmant> InteractiveObjects
		{
			get
			{
				return _interactiveObjects;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			GameObject original = Resources.Load("Prefabs/Game/BossLight") as GameObject;
			_bossLight = Object.Instantiate(original, Animator.transform.position, Quaternion.identity);
			_bossLight.transform.parent = Animator.transform;
			_bossLight.SetActive(false);
			if (_progressBar != null)
			{
				Object.Destroy(_progressBar.gameObject);
			}
			Object.FindObjectOfType<GameController>().Model.Boss = _model;
			_intensitySpecialSlug = 1;
		}

		protected override void OnUseConfigStrategy()
		{
			BossConfigStrategy bossConfigStrategy = new BossConfigStrategy(this);
			bossConfigStrategy.LoadConfiguration(ref _model);
		}

		public void SetSlugs(List<UnitSlugConfigContainer> slugs)
		{
			if (slugs == null || slugs.Count < 1)
			{
				GameController gameController = Object.FindObjectOfType<GameController>();
				if (gameController == null)
				{
					return;
				}
				IDictionary<SlugType, Slug> slugs2 = gameController.Model.Eli.Slugs;
				int num = 1;
				foreach (KeyValuePair<SlugType, Slug> item in slugs2)
				{
					if (num < item.Value.Level)
					{
						num = item.Value.Level;
					}
				}
				{
					foreach (SlugType availableSlug in _availableSlugs)
					{
						_model.AddSlug(new Slug(availableSlug, num));
					}
					return;
				}
			}
			_availableSlugs = new List<SlugType>();
			_availableSlugsLevels = new List<int>();
			foreach (UnitSlugConfigContainer slug in slugs)
			{
				_model.AddSlug(new Slug(slug.slugType, slug.level));
				_availableSlugs.Add(slug.slugType);
				_availableSlugsLevels.Add(slug.level);
			}
		}

		protected override void SetAvailableSlugs()
		{
		}

		protected override void OnStart()
		{
			_interactiveObjects = new List<ArenaInteractiveEnvironmant>();
			ArenaInteractiveEnvironmant[] array = Object.FindObjectsOfType<ArenaInteractiveEnvironmant>();
			ArenaInteractiveEnvironmant[] array2 = array;
			foreach (ArenaInteractiveEnvironmant arenaInteractiveEnvironmant in array2)
			{
				if (Mathf.Abs(arenaInteractiveEnvironmant.transform.position.z - Transform.position.z) > 10f)
				{
					_interactiveObjects.Add(arenaInteractiveEnvironmant);
				}
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			MonoSingleton<DuelManager>.Instance.StateMachine.OnStateChanged += OnDuelStateChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (MonoSingleton<DuelManager>.Instance != null)
			{
				MonoSingleton<DuelManager>.Instance.StateMachine.OnStateChanged -= OnDuelStateChanged;
			}
		}

		private void OnDuelStateChanged(MonoState state)
		{
			if (state is SlugFlyState)
			{
				_bossLight.SetActive(true);
			}
			else
			{
				_bossLight.SetActive(false);
			}
		}

		public void SetUsedSlug(Slug slug)
		{
			_lastUsedSlug = slug;
			_lastUsedSlug.Available = false;
		}

		public bool IsCanUseSlug()
		{
			if (_lastUsedSlug == null)
			{
				return true;
			}
			return _lastUsedSlug.Available;
		}

		protected override void OnStandUpComplited()
		{
			base.OnStandUpComplited();
			AttackTarget = MonoSingleton<DuelManager>.Instance.PlayerController.Transform;
		}

		public override void OnKilled()
		{
			if (!(StateMachine.CurrentState is DeathState))
			{
				StateMachine.ChangeState<DeathState>(new object[0]);
			}
		}

		protected override void OnUpdate()
		{
			if (StateMachine.CurrentState == null && StateMachine.GetNextState == null)
			{
				StateMachine.ChangeState<BossIdleState>(new object[1] { WaitInWarPointTime });
			}
		}

		public void OnFireCommandFinish(FireCommand command)
		{
			AttackTarget = MonoSingleton<DuelManager>.Instance.PlayerController.Transform;
		}
	}
}
