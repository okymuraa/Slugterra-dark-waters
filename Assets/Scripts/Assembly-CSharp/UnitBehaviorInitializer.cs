using BehaviorDesigner.Runtime;
using Slugterra.Behavior;
using Slugterra.Game;
using Slugterra.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using Slugterra.UI.Game;
using UnityEngine;

public class UnitBehaviorInitializer : MonoBehaviour
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

	private GameController _gameController;

	private BehaviorDataHolder dataHolder;

	private void Awake()
	{
		if (_unitController == null || _viewAreaTrigger == null)
		{
			Debug.Log(transform.name);
		}
		if (_unitController != null && _unitController.Animator != null)
		{
			_unitController.Animator.logWarnings = false;
		}
		dataHolder = new BehaviorDataHolder();
		if (_unitController != null)
		{
			dataHolder.Me.Controller = _unitController;
			dataHolder.Me.Root = _unitController.transform;
		}
		dataHolder.Me.ViewAreaTrigger = _viewAreaTrigger;
		if (_gameController == null)
		{
			_gameController = Object.FindObjectOfType<GameController>();
		}
		if (_gameController != null && _gameController.IsMultiplayer && _patrolPathHolder == null)
		{
			_patrolPathHolder = Object.FindObjectOfType<PatrolPathHolder>();
		}
		if (_patrolPathHolder != null && _patrolPathHolder.enabled && !_patrolPathHolder.IsEmpty)
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
		dataHolder.Me.AttackSettings.IsRangeAttacker = true;
		if (_unitController != null && !_unitController.NewVersionAI)
		{
			if (_scrapheadTree != null)
			{
				_scrapheadTree.enabled = false;
			}
			UnitBehavior component = GetComponent<UnitBehavior>();
			if (component != null)
			{
				component.enabled = false;
			}
			enabled = false;
			return;
		}
		try
		{
			if (_scrapheadTree != null)
			{
				_scrapheadTree.EnableBehavior();
				_scrapheadTree.SetVariableValue("DataHolder", dataHolder);
			}
		}
		catch (System.Exception)
		{
			if (_scrapheadTree != null)
			{
				_scrapheadTree.enabled = false;
			}
		}
	}

	private void Start()
	{
		if (_viewAreaTrigger != null)
		{
			_viewAreaTrigger.UpdateTargets();
		}
	}
}
