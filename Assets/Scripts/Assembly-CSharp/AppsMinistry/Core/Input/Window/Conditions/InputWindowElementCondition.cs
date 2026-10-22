using UnityEngine;

namespace AppsMinistry.Core.Input.Window.Conditions
{
	public class InputWindowElementCondition : MonoBehaviour
	{
		public virtual bool IsActive()
		{
			return true;
		}
	}
}
