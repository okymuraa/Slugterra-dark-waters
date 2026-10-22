using System;
using AppsMinistry.Scene;
using UnityEngine;

namespace AppsMinistry.Core
{
	public class LocalizationLevelName : LocalizationTextMesh
	{
		protected override string GetKey()
		{
			return GetCurrentSceneName();
		}

		private string GetCurrentSceneName()
		{
			string empty = string.Empty;
			SceneController sceneController = UnityEngine.Object.FindObjectOfType<SceneController>();
			if (sceneController != null)
			{
				Attribute[] customAttributes = Attribute.GetCustomAttributes(sceneController.GetType());
				Attribute attribute = Array.Find(customAttributes, (Attribute a) => a is SceneNameAttribute);
				Attribute attribute2 = Array.Find(customAttributes, (Attribute a) => a is SurvivalDescriptionAttribute);
				if (attribute != null)
				{
					empty = (attribute as SceneNameAttribute).Name;
				}
				if (attribute2 != null)
				{
					return (attribute2 as SurvivalDescriptionAttribute).TitleKey;
				}
			}
			return empty + "_title";
		}
	}
}
