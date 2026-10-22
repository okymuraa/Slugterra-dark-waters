using Slugterra.Game.Units;
using Slugterra.Game.Units.AreaTrigger;
using UnityEngine;

namespace Slugterra.Behavior.Data
{
	public class Unit
	{
		public ViewAreaTrigger ViewAreaTrigger;

		public UnitController Controller;

		public Transform Root;

		public UnitSettings UnitSettings = new UnitSettings();

		public Effects Effects = new Effects();

		public Movement Movement = new Movement();

		public MovementSettings MovementSettings = new MovementSettings();

		public Attack Attack = new Attack();

		public AttackSettings AttackSettings = new AttackSettings();

		public bool IsMovingInProrgess;
	}
}
