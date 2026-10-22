using BehaviorDesigner.Runtime;
using Slugterra.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using UnityEngine;

public class EliBehaviourTreeInitializer : MonoBehaviour
{
	[SerializeField]
	private BehaviorTree _eliTree;

	[SerializeField]
	private UnitController _unitController;

	[SerializeField]
	private ViewAreaTrigger _viewAreaTrigger;

	private void OnEnable()
	{
		_eliTree.EnableBehavior();
		_eliTree.SetVariableValue("unitController", _unitController);
		_eliTree.SetVariableValue("viewAreaTrigger", _viewAreaTrigger);
	}
}
