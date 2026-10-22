using System;
using BehaviorDesigner.Runtime;
using Slugterra.Game;
using UnityEngine;

[Serializable]
public class IDestructibleSharedVariable : SharedVariable
{
	[SerializeField]
	private IDestructible mValue;

	public IDestructible Value
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
		mValue = (IDestructible)value;
	}

	public override string ToString()
	{
		return (mValue != null) ? mValue.ToString() : "null";
	}
}
