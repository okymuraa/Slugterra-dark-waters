using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedTransform : SharedVariable<Transform>
	{
		public static implicit operator SharedTransform(Transform value)
		{
			SharedTransform sharedTransform = new SharedTransform();
			sharedTransform.mValue = value;
			return sharedTransform;
		}
	}
}
