using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window.Conditions
{
	public class IWECEnabledElements : InputWindowElementCondition
	{
		[SerializeField]
		private List<GameObject> _objects;

		public override bool IsActive()
		{
			bool result = true;
			foreach (GameObject @object in _objects)
			{
				result = @object.activeSelf & @object.activeInHierarchy;
			}
			return result;
		}
	}
}
