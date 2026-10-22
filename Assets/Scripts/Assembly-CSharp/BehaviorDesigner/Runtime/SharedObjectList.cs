using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedObjectList : SharedVariable<List<UnityEngine.Object>>
	{
		public static implicit operator SharedObjectList(List<UnityEngine.Object> value)
		{
			SharedObjectList sharedObjectList = new SharedObjectList();
			sharedObjectList.mValue = value;
			return sharedObjectList;
		}
	}
}
