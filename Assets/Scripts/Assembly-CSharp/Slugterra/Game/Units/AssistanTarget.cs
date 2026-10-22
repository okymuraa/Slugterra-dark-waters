using System;
using AppsMinistry.Core.Command;
using Slugterra.Configuration;
using Slugterra.Game.Units.Ally.Assistant;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class AssistanTarget : ITarget
	{
		public void Apply(Type typeOfBoost, ConsumableProperties config)
		{
			AssistantController assistantController = FindTarget(config) as AssistantController;
			if (assistantController != null)
			{
				MonoCommand.ExecuteOn(typeOfBoost, assistantController.GameObject, assistantController, config);
			}
		}

		public object FindTarget(ConsumableProperties config)
		{
			return UnityEngine.Object.FindObjectOfType<AssistantController>();
		}
	}
}
