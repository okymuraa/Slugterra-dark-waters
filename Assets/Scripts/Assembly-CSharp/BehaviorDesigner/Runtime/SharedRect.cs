using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedRect : SharedVariable<Rect>
	{
		public static implicit operator SharedRect(Rect value)
		{
			SharedRect sharedRect = new SharedRect();
			sharedRect.mValue = value;
			return sharedRect;
		}
	}
}
