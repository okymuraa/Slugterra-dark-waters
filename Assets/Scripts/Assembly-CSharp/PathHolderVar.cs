using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

[Serializable]
public class PathHolderVar : SharedVariable
{
	[SerializeField]
	private PatrolPathHolder mValue;

	public PatrolPathHolder Value
	{
		get
		{
			return mValue;
		}
		set
		{
			mValue = value;
		}
	}

	public override object GetValue()
	{
		return mValue;
	}

	public override void SetValue(object value)
	{
		mValue = (PatrolPathHolder)value;
	}

	public override string ToString()
	{
		return (!(mValue == null)) ? mValue.ToString() : "null";
	}
}
