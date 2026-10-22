using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedVector3 : SharedVariable<Vector3>
	{
		public static implicit operator SharedVector3(Vector3 value)
		{
			SharedVector3 sharedVector = new SharedVector3();
			sharedVector.mValue = value;
			return sharedVector;
		}
	}
}
