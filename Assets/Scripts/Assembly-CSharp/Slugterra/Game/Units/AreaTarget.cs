using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class AreaTarget : ITarget
	{
		public void Apply(Type typeOfBoost, ConsumableProperties config)
		{
			object obj = FindTarget(config);
			if (!(obj is List<UnitController>))
			{
				return;
			}
			List<UnitController> list = obj as List<UnitController>;
			foreach (UnitController item in list)
			{
				MonoCommand.ExecuteOn(typeOfBoost, item.GameObject, item, config);
			}
		}

		public object FindTarget(ConsumableProperties config)
		{
			List<UnitController> list = new List<UnitController>();
			int num = LayerMask.NameToLayer("AllyLayer");
			PlayerController localPlayer = PlayerManager.LocalPlayer;
			Collider[] array = PhysicsFacade.OverlapSphere(localPlayer.Transform.position, config.Radius, 1 << num);
			for (int i = 0; i < array.Length; i++)
			{
				UnitController component = array[i].GetComponent<UnitController>();
				if (component != null)
				{
					list.Add(component);
				}
			}
			return list;
		}
	}
}
