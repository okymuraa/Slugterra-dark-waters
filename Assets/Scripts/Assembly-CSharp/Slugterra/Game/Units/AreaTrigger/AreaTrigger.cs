using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Slugterra.Game.Units.AreaTrigger
{
	public class AreaTrigger : MonoBehaviour
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Transform> OnTriggerEnterEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Transform> OnTriggerExitEvent__BackingField;

		[SerializeField]
		protected List<string> layersName;

		private List<Transform> _items;

		private List<Transform> _ignorItems;

		private Transform _transform;

		private Transform _baseTransform;

		private const float UPDATE_INTERVAL = 0.1f;

		private float _nextUpdate;

		public List<Transform> Items
		{
			get
			{
				return _items;
			}
		}

		protected Transform baseTransform
		{
			get
			{
				if (_baseTransform != null)
				{
					return _baseTransform;
				}
				return _transform;
			}
		}

		public event Action<Transform> OnTriggerEnterEvent
		{
			add
			{
				Action<Transform> action = OnTriggerEnterEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerEnterEvent__BackingField, (Action<Transform>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Transform> action = OnTriggerEnterEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerEnterEvent__BackingField, (Action<Transform>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Transform> OnTriggerExitEvent
		{
			add
			{
				Action<Transform> action = OnTriggerExitEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerExitEvent__BackingField, (Action<Transform>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Transform> action = OnTriggerExitEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerExitEvent__BackingField, (Action<Transform>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			_transform = transform;
			_items = new List<Transform>();
			_ignorItems = new List<Transform>();
			gameObject.layer = LayerMask.NameToLayer("Area");
			if (layersName.Count == 0)
			{
				UnityEngine.Debug.LogWarning("AreaTrigger: does not have layer attribute");
			}
			_nextUpdate = Time.time + 0.1f;
			OnCreate();
		}

		protected virtual void OnStart()
		{
		}

		private void Start()
		{
			OnStart();
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
			if (!(_nextUpdate < Time.time))
			{
				return;
			}
			for (int i = 0; i < _items.Count; i++)
			{
				if (_items[i] == null || !_items[i].gameObject.activeSelf || _items[i].GetComponent<Collider>() == null || !_items[i].GetComponent<Collider>().enabled || !IsCorrectLayer(_items[i].gameObject.layer))
				{
					Transform item = _items[i];
					_items.RemoveAt(i);
					TriggerExit(item);
					i--;
				}
			}
			OnUpdate();
			_nextUpdate = Time.time + 0.1f;
		}

		protected void OnTriggerEnter(Collider item)
		{
			if (!(item == null) && !_ignorItems.Contains(item.transform) && IsCorrectLayer(item.gameObject.layer))
			{
				if (!_items.Contains(item.transform))
				{
					_items.Add(item.transform);
				}
				if (IsTriggerItem(item.transform))
				{
					TriggerEnter(item.transform);
				}
			}
		}

		protected void TriggerEnter(Transform item)
		{
			if (OnTriggerEnterEvent__BackingField != null)
			{
				OnTriggerEnterEvent__BackingField(item);
			}
		}

		protected bool IsCorrectLayer(int layer)
		{
			return layersName.Contains(LayerMask.LayerToName(layer));
		}

		public int GetLayerMask()
		{
			int result = 0;
			layersName.ForEach((string obj) =>
			{
				result |= 1 << LayerMask.NameToLayer(obj);
			});
			return result;
		}

		protected virtual bool IsTriggerItem(Transform item)
		{
			return true;
		}

		protected virtual void OnTriggerExit(Collider item)
		{
			if (IsCorrectLayer(item.gameObject.layer) && _items.Contains(item.transform))
			{
				_items.Remove(item.transform);
				if (IsTriggerItem(item.transform))
				{
					TriggerExit(item.transform);
				}
			}
		}

		protected void TriggerExit(Transform item)
		{
			if (OnTriggerExitEvent__BackingField != null)
			{
				OnTriggerExitEvent__BackingField(item);
			}
		}

		public bool IsEmpty()
		{
			return _items.Count == 0;
		}

		public void SetBaseTransform(Transform transform)
		{
			_baseTransform = transform;
		}

		public void AddIgnorObject(Transform item)
		{
			if (!_ignorItems.Contains(item))
			{
				_ignorItems.Add(item);
			}
		}

		public void ClearLayer()
		{
			layersName.Clear();
		}

		public void AddLayer(string name)
		{
			layersName.Add(name);
		}

		public void RemoveLayer(string name)
		{
			layersName.Remove(name);
		}
	}
}
