using BehaviorDesigner.Runtime;
using Slugterra.Game.Units;
using UnityEngine;

public class EliBehaviour : MonoBehaviour
{
	[SerializeField]
	private BehaviorTree eliBehaviourTree;

	[SerializeField]
	private UnitController unitController;

	public void MoveTo(Transform newPoint, float moveSpeed = 0f)
	{
		eliBehaviourTree.SetVariableValue("moveSpeed", (moveSpeed != 0f) ? moveSpeed : unitController.Model.WalkSpeed);
		eliBehaviourTree.SetVariableValue("nextPathPoint", newPoint);
		eliBehaviourTree.SetVariableValue("recalculatePath", true);
	}

	public void ShootAt(UnitController newTarget)
	{
		eliBehaviourTree.SetVariableValue("attackTargetController", newTarget);
		eliBehaviourTree.SetVariableValue("attackTarget", newTarget.HitMarker.GetRandomMarker());
	}
}
