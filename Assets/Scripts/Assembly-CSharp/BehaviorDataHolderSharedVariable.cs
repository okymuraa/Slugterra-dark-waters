using System;
using BehaviorDesigner.Runtime;
using Slugterra.Behavior;
using UnityEngine;

[Serializable]
public class BehaviorDataHolderSharedVariable : SharedVariable
{
	[SerializeField]
	private BehaviorDataHolder mValue;

	public BehaviorDataHolder Value
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
		mValue = (BehaviorDataHolder)value;
	}

	public override string ToString()
	{
		return (mValue != null) ? mValue.ToString() : "null";
	}
}
