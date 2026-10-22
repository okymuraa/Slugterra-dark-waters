using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class SharedQuaternion : SharedVariable<Quaternion>
	{
		public static implicit operator SharedQuaternion(Quaternion value)
		{
			SharedQuaternion sharedQuaternion = new SharedQuaternion();
			sharedQuaternion.mValue = value;
			return sharedQuaternion;
		}
	}
}
