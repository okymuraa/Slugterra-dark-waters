using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

[Serializable]
public class MoveTargetTypeSharedVariable : SharedVariable
{
	[SerializeField]
	private SelectPointToMove.SelectMoveTargetType mValue;

	public SelectPointToMove.SelectMoveTargetType Value
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
		mValue = (SelectPointToMove.SelectMoveTargetType)value;
	}

	public override string ToString()
	{
		return mValue.ToString();
	}
}
