using System;
using AppsMinistry.Scene;
using Slugterra.Configuration;
using UnityEngine;

namespace AppsMinistry.Core
{
	public class LocalizationMapName : LocalizationTextMesh
	{
		protected override string GetKey()
		{
			return GetLevelDesc();
		}

		private string GetLevelDesc()
		{
			string empty = string.Empty;
			SceneController sceneController = UnityEngine.Object.FindObjectOfType<SceneController>();
			if (sceneController != null)
			{
				Attribute[] customAttributes = Attribute.GetCustomAttributes(sceneController.GetType());
				Attribute attribute = Array.Find(customAttributes, (Attribute a) => a is SurvivalDescriptionAttribute);
				if (attribute != null)
				{
					return (attribute as SurvivalDescriptionAttribute).TitleKey;
				}
				return MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelDesc(sceneController.GetType().Name);
			}
			return string.Empty;
		}
	}
}
