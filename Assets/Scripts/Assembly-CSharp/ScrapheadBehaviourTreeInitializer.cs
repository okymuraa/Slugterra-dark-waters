using BehaviorDesigner.Runtime;
using Slugterra.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using UnityEngine;

public class ScrapheadBehaviourTreeInitializer : MonoBehaviour
{
	[SerializeField]
	private BehaviorTree _scrapheadTree;

	[SerializeField]
	private UnitController _unitController;

	[SerializeField]
	private PatrolPathHolder _pathHolder;

	[SerializeField]
	private ViewAreaTrigger _viewAreaTrigger;

	private void OnEnable()
	{
		if (_unitController == null || _pathHolder == null || _viewAreaTrigger == null)
		{
			Debug.Log(transform.name);
		}
		_scrapheadTree.EnableBehavior();
		_unitController.Animator.logWarnings = false;
		_scrapheadTree.SetVariableValue("unitController", _unitController);
		_scrapheadTree.SetVariableValue("patrolPathHolder", _pathHolder);
		_scrapheadTree.SetVariableValue("viewAreaTrigger", _viewAreaTrigger);
		_scrapheadTree.SetVariableValue("enemyTag", "Player");
		_scrapheadTree.SetVariableValue("alliesTag", "Enemy");
		_scrapheadTree.SetVariableValue("isRangeAttacker", true);
	}
}
