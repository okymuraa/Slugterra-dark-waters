using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

[AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdate")]
[HelpURL("http://arongranberg.com/astar/docs/class_recast_tile_update.php")]
public class RecastTileUpdate : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<Bounds> OnNeedUpdates__BackingField;

	public static event Action<Bounds> OnNeedUpdates
	{
		add
		{
			Action<Bounds> action = OnNeedUpdates__BackingField;
			Action<Bounds> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnNeedUpdates__BackingField, (Action<Bounds>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<Bounds> action = OnNeedUpdates__BackingField;
			Action<Bounds> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnNeedUpdates__BackingField, (Action<Bounds>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private void Start()
	{
		ScheduleUpdate();
	}

	private void OnDestroy()
	{
		ScheduleUpdate();
	}

	public void ScheduleUpdate()
	{
		Collider component = GetComponent<Collider>();
		if (component != null)
		{
			if (OnNeedUpdates__BackingField != null)
			{
				OnNeedUpdates__BackingField(component.bounds);
			}
		}
		else if (OnNeedUpdates__BackingField != null)
		{
			OnNeedUpdates__BackingField(new Bounds(transform.position, Vector3.zero));
		}
	}
}
