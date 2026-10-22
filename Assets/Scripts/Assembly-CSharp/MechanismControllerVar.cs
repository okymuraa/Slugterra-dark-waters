using System;
using BehaviorDesigner.Runtime;
using Slugterra.Game.Units.Mechanisms;
using UnityEngine;

[Serializable]
public class MechanismControllerVar : SharedVariable
{
	[SerializeField]
	private MechanismController mValue;

	public MechanismController Value
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
		mValue = (MechanismController)value;
	}

	public override string ToString()
	{
		return (!(mValue == null)) ? mValue.ToString() : "null";
	}
}
