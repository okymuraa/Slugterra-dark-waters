using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public abstract class tk2dUILayoutContainer : tk2dUILayout
{
	protected Vector2 innerSize = Vector2.zero;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnChangeContent__BackingField;

	public event Action OnChangeContent
	{
		add
		{
			Action action = OnChangeContent__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChangeContent__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnChangeContent__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnChangeContent__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	public Vector2 GetInnerSize()
	{
		return innerSize;
	}

	protected abstract void DoChildLayout();

	public override void Reshape(Vector3 dMin, Vector3 dMax, bool updateChildren)
	{
		bMin += dMin;
		bMax += dMax;
		Vector3 vector = new Vector3(bMin.x, bMax.y);
		transform.position += vector;
		bMin -= vector;
		bMax -= vector;
		DoChildLayout();
		if (OnChangeContent__BackingField != null)
		{
			OnChangeContent__BackingField();
		}
	}

	public void AddLayout(tk2dUILayout layout, tk2dUILayoutItem item)
	{
		item.gameObj = layout.gameObject;
		item.layout = layout;
		layoutItems.Add(item);
		layout.gameObject.transform.parent = transform;
		Refresh();
	}

	public void AddLayoutAtIndex(tk2dUILayout layout, tk2dUILayoutItem item, int index)
	{
		item.gameObj = layout.gameObject;
		item.layout = layout;
		layoutItems.Insert(index, item);
		layout.gameObject.transform.parent = transform;
		Refresh();
	}

	public void RemoveLayout(tk2dUILayout layout)
	{
		foreach (tk2dUILayoutItem layoutItem in layoutItems)
		{
			if (layoutItem.layout == layout)
			{
				layoutItems.Remove(layoutItem);
				layout.gameObject.transform.parent = null;
				break;
			}
		}
		Refresh();
	}
}
