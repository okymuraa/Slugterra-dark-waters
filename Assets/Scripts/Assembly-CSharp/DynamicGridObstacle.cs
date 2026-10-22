using System;
using Pathfinding;
using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_dynamic_grid_obstacle.php")]
[RequireComponent(typeof(Collider))]
public class DynamicGridObstacle : MonoBehaviour
{
	private Collider col;

	private Transform tr;

	public float updateError = 1f;

	public float checkTime = 0.2f;

	private Bounds prevBounds;

	private Quaternion prevRotation;

	private bool prevEnabled;

	private float lastCheckTime = -9999f;

	private void Start()
	{
		col = GetComponent<Collider>();
		tr = transform;
		if (col == null)
		{
			throw new Exception("A collider must be attached to the GameObject for the DynamicGridObstacle to work");
		}
		prevBounds = col.bounds;
		prevEnabled = col.enabled;
		prevRotation = tr.rotation;
	}

	private void Update()
	{
		if (!col)
		{
			Debug.LogError("Removed collider from DynamicGridObstacle", this);
			enabled = false;
		}
		else if (AstarPath.active == null || AstarPath.active.isScanning)
		{
			lastCheckTime = Time.realtimeSinceStartup;
		}
		else
		{
			if (Time.realtimeSinceStartup - lastCheckTime < checkTime)
			{
				return;
			}
			if (col.enabled)
			{
				Bounds bounds = col.bounds;
				Quaternion rotation = tr.rotation;
				Vector3 vector = prevBounds.min - bounds.min;
				Vector3 vector2 = prevBounds.max - bounds.max;
				float magnitude = bounds.extents.magnitude;
				float num = magnitude * Quaternion.Angle(prevRotation, rotation) * ((float)Math.PI / 180f);
				if (vector.sqrMagnitude > updateError * updateError || vector2.sqrMagnitude > updateError * updateError || num > updateError || !prevEnabled)
				{
					DoUpdateGraphs();
				}
			}
			else if (prevEnabled)
			{
				DoUpdateGraphs();
			}
		}
	}

	private void OnDestroy()
	{
		if (AstarPath.active != null)
		{
			GraphUpdateObject ob = new GraphUpdateObject(prevBounds);
			AstarPath.active.UpdateGraphs(ob);
		}
	}

	public void DoUpdateGraphs()
	{
		if (col == null)
		{
			return;
		}
		if (!col.enabled)
		{
			AstarPath.active.UpdateGraphs(prevBounds);
		}
		else
		{
			Bounds bounds = col.bounds;
			Bounds bounds2 = bounds;
			bounds2.Encapsulate(prevBounds);
			if (BoundsVolume(bounds2) < BoundsVolume(bounds) + BoundsVolume(prevBounds))
			{
				AstarPath.active.UpdateGraphs(bounds2);
			}
			else
			{
				AstarPath.active.UpdateGraphs(prevBounds);
				AstarPath.active.UpdateGraphs(bounds);
			}
			prevBounds = bounds;
		}
		prevEnabled = col.enabled;
		prevRotation = tr.rotation;
		lastCheckTime = Time.realtimeSinceStartup;
	}

	private static float BoundsVolume(Bounds b)
	{
		return Math.Abs(b.size.x * b.size.y * b.size.z);
	}
}
