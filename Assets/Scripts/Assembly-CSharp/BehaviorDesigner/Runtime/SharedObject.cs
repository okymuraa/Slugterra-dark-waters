using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedObject : SharedVariable<UnityEngine.Object>
	{
		public static explicit operator SharedObject(UnityEngine.Object value)
		{
			SharedObject sharedObject = new SharedObject();
			sharedObject.mValue = value;
			return sharedObject;
		}
	}
}
