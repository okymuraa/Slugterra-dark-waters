using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedColor : SharedVariable<Color>
	{
		public static implicit operator SharedColor(Color value)
		{
			SharedColor sharedColor = new SharedColor();
			sharedColor.mValue = value;
			return sharedColor;
		}
	}
}
