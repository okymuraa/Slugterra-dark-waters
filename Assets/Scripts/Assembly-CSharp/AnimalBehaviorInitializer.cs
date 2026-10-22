using BehaviorDesigner.Runtime;
using Slugterra.Behavior;
using Slugterra.Game;
using Slugterra.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using UnityEngine;

public class AnimalBehaviorInitializer : MonoBehaviour
{
	[SerializeField]
	private BehaviorTree _scrapheadTree;

	[SerializeField]
	private UnitController _unitController;

	[SerializeField]
	private ViewAreaTrigger _viewAreaTrigger;

	[SerializeField]
	private PatrolPathHolder _patrolPathHolder;

	[SerializeField]
	private bool canMove = true;

	[SerializeField]
	private bool advancedSlugShooting;

	[SerializeField]
	private bool advancedMovement;

	[SerializeField]
	private CoverNode _coverNodes;

	[SerializeField]
	private bool stepBackAfterAttack;

	private BehaviorDataHolder dataHolder;

	private void Awake()
	{
		if (_unitController == null || _viewAreaTrigger == null)
		{
			Debug.Log(transform.name);
		}
		_scrapheadTree.EnableBehavior();
		_unitController.Animator.logWarnings = false;
		dataHolder = new BehaviorDataHolder();
		dataHolder.Me.Controller = _unitController;
		dataHolder.Me.ViewAreaTrigger = _viewAreaTrigger;
		dataHolder.Me.Root = _unitController.transform;
		if (_patrolPathHolder != null)
		{
			dataHolder.Me.MovementSettings.PatrolPath = _patrolPathHolder;
		}
		else
		{
			dataHolder.Me.MovementSettings.CanPatrol = false;
		}
		dataHolder.Me.MovementSettings.CanMove = canMove;
		dataHolder.Me.MovementSettings.AdvancedMovement = advancedMovement;
		dataHolder.Me.AttackSettings.AdvancedSlugShooting = advancedSlugShooting;
		dataHolder.Me.AttackSettings.IsRangeAttacker = false;
		dataHolder.Me.MovementSettings.StepBackAfterAttack = stepBackAfterAttack;
		if (stepBackAfterAttack)
		{
			dataHolder.Me.MovementSettings.StepBackChance = 1f;
		}
		dataHolder.Me.MovementSettings.ReloadRunChance = 700f;
		_scrapheadTree.SetVariableValue("DataHolder", dataHolder);
	}

	private void Start()
	{
		_viewAreaTrigger.UpdateTargets();
	}
}
