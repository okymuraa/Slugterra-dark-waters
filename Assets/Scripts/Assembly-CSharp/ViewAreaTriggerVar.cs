using System;
using BehaviorDesigner.Runtime;
using Slugterra.Game.Units.AreaTrigger;
using UnityEngine;

[Serializable]
public class ViewAreaTriggerVar : SharedVariable
{
	[SerializeField]
	private ViewAreaTrigger mValue;

	public ViewAreaTrigger Value
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
		mValue = (ViewAreaTrigger)value;
	}

	public override string ToString()
	{
		return (!(mValue == null)) ? mValue.ToString() : "null";
	}
}
