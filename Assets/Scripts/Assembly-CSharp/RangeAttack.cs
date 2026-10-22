using System.Collections.Generic;
using System.Linq;
using AppsMinistry.Core;
using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Configuration;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Game;
using Slugterra.Game.Mission;
using Slugterra.Game.Units;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.Slugs;
using Slugterra.UI.Game;
using UnityEngine;

[TaskCategory("Slugterra/Units")]
public class RangeAttack : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private Slug _slug;

	private int flopperShotCount;

	private TaskStatus attackStatus;

	private bool isAttackInProgress;

	private bool useFlopper = true;

	private GameController gameController;

	private IKControl _ikControl;

	public override void OnAwake()
	{
		if (gameController == null)
		{
			gameController = Object.FindObjectOfType<GameController>();
		}
		useFlopper = true;
	}

	public override void OnStart()
	{
		attackStatus = TaskStatus.Running;
		isAttackInProgress = false;
		_ikControl = dataHolder.Value.Me.Controller.Animator.GetComponent<IKControl>();
	}

	private void Shoot()
	{
		Transform shotPoint = (dataHolder.Value.Me.Controller as EnemyController).ShotPoint;
		if (!useFlopper)
		{
			float num = 1f;
			if (dataHolder.Value.Me.AttackSettings.AdvancedSlugShooting)
			{
				List<SlugType> eliFiresAll = MonoSingleton<MissionStatisticsController>.Instance.EliFiresAll;
				float num2 = eliFiresAll.FindAll((SlugType obj) => obj == SlugType.Flopper).Count;
				float num3 = eliFiresAll.FindAll((SlugType obj) => obj != SlugType.Flopper).Count;
				int num4 = eliFiresAll.FindLastIndex((SlugType obj) => obj != SlugType.Flopper);
				if (num2 == 0f)
				{
					num2 = 1f;
				}
				num = num3 / num2;
				if (num4 >= 0 && eliFiresAll.Count - num4 < 5)
				{
					num += 0.5f;
				}
			}
			_slug = null;
			if (Random.Range(0f, 1f) < num && flopperShotCount > (dataHolder.Value.Me.Controller as HumanoidController).IntensitySpecialSlug)
			{
				List<Slug> list = (from obj in (dataHolder.Value.Me.Controller as EnemyController).AvailableSlugs
					select (dataHolder.Value.Me.Controller.Model as Humanoid).GetSlug(obj) into obj2
					where obj2.Available && obj2.NextFire < Time.time
					select obj2).ToList();
				if (list.Count == 0)
				{
					_slug = (dataHolder.Value.Me.Controller.Model as Humanoid).GetSlug(SlugType.Flopper);
					flopperShotCount++;
				}
				else
				{
					_slug = list[Random.Range(0, list.Count)];
					flopperShotCount = 0;
					useFlopper = true;
				}
			}
			else
			{
				_slug = (dataHolder.Value.Me.Controller.Model as Humanoid).GetSlug(SlugType.Flopper);
				flopperShotCount++;
			}
		}
		else
		{
			_slug = (dataHolder.Value.Me.Controller.Model as Humanoid).GetSlug(SlugType.Flopper);
			flopperShotCount++;
			useFlopper = false;
		}
		if (_ikControl != null)
		{
			_ikControl.ResetWeight();
		}
		Vector3 normalized = (dataHolder.Value.Enemy.AttackTargetPoint.position - shotPoint.position).normalized;
		Quaternion rotation = Quaternion.LookRotation(normalized);
		GameObject slugPrefab = GameFactory.GetSlugPrefab(_slug.Type);
		GameObject gameObject = Object.Instantiate(slugPrefab, shotPoint.position, rotation);
		SlugController component = gameObject.GetComponent<SlugController>();
		SlugStrategy strategy = null;
		component.SetVelocity(normalized);
		component.SetSlug(_slug);
		_slug.Available = false;
		if (LayerMask.LayerToName(dataHolder.Value.Me.Controller.GameObject.layer) == "EnemyLayer")
		{
			gameObject.layer = LayerMask.NameToLayer("EnemySlugLayer");
			int num5 = LayerMask.NameToLayer("AllyLayer");
			int num6 = LayerMask.NameToLayer("NeutralLayer");
			int num7 = LayerMask.NameToLayer("PassiveLayer");
			strategy = new EnemySlugStrategy(component, new int[3] { num5, num6, num7 });
		}
		else if (LayerMask.LayerToName(dataHolder.Value.Me.Controller.GameObject.layer) == "AllyLayer")
		{
			gameObject.layer = LayerMask.NameToLayer("AllySlugLayer");
			int num8 = LayerMask.NameToLayer("EnemyLayer");
			int num9 = LayerMask.NameToLayer("NeutralLayer");
			int num10 = LayerMask.NameToLayer("PassiveLayer");
			strategy = new AllySlugStrategy(component, new int[3] { num8, num9, num10 });
		}
		component.SetStrategy(strategy);
		component.Parent = dataHolder.Value.Me.Controller as HumanoidController;
		component.Target = dataHolder.Value.Enemy.AttackTargetPoint;
		if (_slug.Type == SlugType.Flopper)
		{
			component.SelfDestruction(5f);
		}
		else
		{
			component.SelfDestruction(15f);
		}
		dataHolder.Value.Me.Controller.Animator.SetTrigger("OnFire");
		(dataHolder.Value.Me.Controller as EnemyController).PlaySlugShotSound(_slug.Type);
		if (_slug.Type == SlugType.Flopper)
		{
			(dataHolder.Value.Me.Controller as EnemyController).ReloadSlug(_slug);
		}
		attackStatus = TaskStatus.Success;
	}

	public override TaskStatus OnUpdate()
	{
		if (!isAttackInProgress)
		{
			if (!dataHolder.Value.Enemy.Exist)
			{
				return TaskStatus.Failure;
			}
			Shoot();
			isAttackInProgress = true;
		}
		if (!dataHolder.Value.Enemy.Exist || !dataHolder.Value.Me.AttackSettings.CanShoot)
		{
			return TaskStatus.Failure;
		}
		if (gameController.IsPlayCutScene && !dataHolder.Value.Me.UnitSettings.CutSceneUnlock)
		{
			return TaskStatus.Failure;
		}
		float sqrMagnitude = (dataHolder.Value.Enemy.Root.position - dataHolder.Value.Me.Root.position).sqrMagnitude;
		if (sqrMagnitude > dataHolder.Value.Me.Attack.AttackDistanceSQR)
		{
			return TaskStatus.Failure;
		}
		return attackStatus;
	}
}
