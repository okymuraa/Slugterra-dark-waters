using System;
using BehaviorDesigner.Runtime;
using Slugterra.Game;
using UnityEngine;

[Serializable]
public class CoverNodesVar : SharedVariable
{
	[SerializeField]
	private CoverNode mValue;

	public CoverNode Value
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
		mValue = (CoverNode)value;
	}

	public override string ToString()
	{
		return (!(mValue == null)) ? mValue.ToString() : "null";
	}
}
