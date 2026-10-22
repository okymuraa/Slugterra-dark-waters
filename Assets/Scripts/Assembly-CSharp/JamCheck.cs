using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class JamCheck : Action
{
	public BehaviorDataHolderSharedVariable dataHolder;

	private bool isMovingInProgress;

	private float minDelta = 0.1f;

	private float minTime = 0.4f;

	private float curDelta;

	private float curTime;

	private Vector3 oldPos;

	public override TaskStatus OnUpdate()
	{
		if (!dataHolder.Value.Me.UnitSettings.IsALive || dataHolder.Value.Me.Controller == null)
		{
			return TaskStatus.Running;
		}
		if (isMovingInProgress != dataHolder.Value.Me.IsMovingInProrgess)
		{
			curDelta = 0f;
			curTime = 0f;
			isMovingInProgress = dataHolder.Value.Me.IsMovingInProrgess;
			oldPos = dataHolder.Value.Me.Controller.Transform.position;
			return TaskStatus.Running;
		}
		if (dataHolder.Value.Me.IsMovingInProrgess)
		{
			curTime += Time.deltaTime;
			curDelta += (dataHolder.Value.Me.Controller.Transform.position - oldPos).magnitude;
			oldPos = dataHolder.Value.Me.Controller.Transform.position;
			if (curTime >= minTime)
			{
				if (curDelta < minDelta)
				{
					curTime = 0f;
					curDelta = 0f;
					return TaskStatus.Success;
				}
				curTime = 0f;
				curDelta = 0f;
			}
		}
		return TaskStatus.Running;
	}
}
