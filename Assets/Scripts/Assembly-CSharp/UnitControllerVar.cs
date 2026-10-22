using System;
using BehaviorDesigner.Runtime;
using Slugterra.Game.Units;
using UnityEngine;

[Serializable]
public class UnitControllerVar : SharedVariable
{
	[SerializeField]
	private UnitController mValue;

	public UnitController Value
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
		mValue = (UnitController)value;
	}

	public override string ToString()
	{
		return (!(mValue == null)) ? mValue.ToString() : "null";
	}
}
