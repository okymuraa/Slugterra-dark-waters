using System;
using System.Collections.Generic;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Player;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class PlayerAndAssistantTarget : ITarget
	{
		public void Apply(Type typeOfBoost, ConsumableProperties config)
		{
			PlayerController localPlayer = PlayerManager.LocalPlayer;
			if (localPlayer != null)
			{
				if (typeOfBoost == typeof(FirstAidKit))
				{
					ConsumableProperties consumableProperties = new ConsumableProperties();
					consumableProperties.Health = localPlayer.Model.MaxHealth / 4f;
					MonoCommand.ExecuteOn(typeOfBoost, localPlayer.GameObject, localPlayer, consumableProperties);
				}
				else
				{
					MonoCommand.ExecuteOn(typeOfBoost, localPlayer.GameObject, localPlayer, config);
				}
			}
			AssistantController assistantController = UnityEngine.Object.FindObjectOfType<AssistantController>();
			if (assistantController != null)
			{
				if (typeOfBoost == typeof(FirstAidKit))
				{
					ConsumableProperties consumableProperties2 = new ConsumableProperties();
					consumableProperties2.Health = assistantController.Model.MaxHealth / 4f;
					MonoCommand.ExecuteOn(typeOfBoost, assistantController.GameObject, assistantController, consumableProperties2);
				}
				else
				{
					MonoCommand.ExecuteOn(typeOfBoost, assistantController.GameObject, assistantController, config);
				}
			}
		}

		public object FindTarget(ConsumableProperties config)
		{
			List<UnitController> list = new List<UnitController>();
			PlayerController localPlayer = PlayerManager.LocalPlayer;
			AssistantController assistantController = UnityEngine.Object.FindObjectOfType<AssistantController>();
			if (localPlayer != null)
			{
				list.Add(localPlayer);
			}
			if (assistantController != null)
			{
				list.Add(assistantController);
			}
			return list;
		}
	}
}
