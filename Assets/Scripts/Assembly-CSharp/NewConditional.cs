using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Slugterra.Game.Units;
using UnityEngine;

public class NewConditional : Conditional
{
	public SharedFloat announceRadius;

	public SharedString alliesTag;

	public UnitControllerVar unitController;

	private Transform _me;

	public override TaskStatus OnUpdate()
	{
		_me = unitController.Value.transform;
		Ray ray = new Ray(_me.position - Vector3.up * announceRadius.Value * 2f, Vector3.up * announceRadius.Value * 2f);
		RaycastHit[] source = Physics.SphereCastAll(ray, announceRadius.Value, announceRadius.Value * 22f);
		IEnumerable<UnitController> enumerable = from obj2 in source.ToList().FindAll((RaycastHit obj) => obj.collider.tag == alliesTag.Value)
			select obj2.transform.GetComponentInParent<UnitController>();
		return TaskStatus.Success;
	}
}
