using Slugterra.Game;
using Slugterra.Game.Units;
using UnityEngine;

public class ScrapheadBehavior : IBehavior
{
	public override void OnAnnounce(UnitController enemy)
	{
		MyBehaviorTree.SetVariableValue("enemyController", enemy);
		MyBehaviorTree.SetVariableValue("attackTarget", enemy.HitMarker.GetRandomMarker());
		MyBehaviorTree.SetVariableValue("isTargetFinded", true);
		Debug.Log(string.Format("{0} is heard announce about {1} enemy", transform.parent.name, enemy.transform.parent.name));
	}

	public override void OnAnnounce(Vector3 enemyPos)
	{
		MyBehaviorTree.SetVariableValue("isRecieveAnnounce", true);
	}

	public override void OnDamage(float damage, Attacker attacker)
	{
	}
}
