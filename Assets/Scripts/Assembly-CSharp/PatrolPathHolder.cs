using System.Collections.Generic;
using UnityEngine;

public class PatrolPathHolder : MonoBehaviour
{
	[SerializeField]
	private List<Transform> _pathNodes;

	private Queue<Transform> _pathNodesStack = new Queue<Transform>();

	private bool endlessPath = true;

	public bool EndlessPath
	{
		set
		{
			endlessPath = value;
		}
	}

	public bool IsEmpty
	{
		get
		{
			return _pathNodes.Count == 0 || !gameObject.activeSelf;
		}
	}

	private void OnEnable()
	{
		RecalculatePath();
	}

	public Transform GetNextPathPoint()
	{
		Transform result = null;
		if (_pathNodesStack.Count == 0)
		{
			Debug.Log("<color=red>PatrolPathHolder not enabled or have 0 nodes</color>");
			return result;
		}
		if (!endlessPath)
		{
			result = ((_pathNodesStack.Count != 1) ? _pathNodesStack.Dequeue() : _pathNodesStack.Peek());
		}
		else
		{
			result = _pathNodesStack.Dequeue();
			_pathNodesStack.Enqueue(result);
		}
		return result;
	}

	private void RecalculatePath()
	{
		if (_pathNodes == null || _pathNodes.Count == 0)
		{
			return;
		}
		_pathNodesStack.Clear();
		_pathNodes.ForEach((Transform obj) =>
		{
			if (obj != null)
			{
				_pathNodesStack.Enqueue(obj);
			}
		});
	}

	public void SetPath(List<Transform> newPath)
	{
		_pathNodes = newPath;
		RecalculatePath();
	}
}
