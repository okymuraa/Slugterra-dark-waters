using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Utils;
using Slugterra.Game.Units.NetPlayer;
using UnityEngine;

namespace Slugterra.Game.Units.AreaTrigger
{
	public class ViewAreaTrigger : AreaTrigger
	{
		[SerializeField]
		private float _sectorAngle;

		[SerializeField]
		private float _baseAngleOffset;

		private float _radius;

		private List<Transform> _itemsInSector = new List<Transform>();

		private int _baseLayer;

		private SphereCollider _radiusCollider;

		public HitMarkers CurrentVisibleMarkers = new HitMarkers();

		private Transform _currentSelectedItem;

		private Transform _currentItem;

		private int _layerMask;

		public List<Transform> VisibleItems = new List<Transform>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Transform> OnViewTriggerEnterEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Transform> OnViewTriggerExitEvent__BackingField;

		public Transform CurrentItem
		{
			get
			{
				return _currentItem;
			}
			set
			{
				CurrentVisibleMarkers.Markers.Clear();
				_currentItem = value;
			}
		}

		public event Action<Transform> OnViewTriggerEnterEvent
		{
			add
			{
				Action<Transform> action = OnViewTriggerEnterEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnViewTriggerEnterEvent__BackingField, (Action<Transform>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Transform> action = OnViewTriggerEnterEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnViewTriggerEnterEvent__BackingField, (Action<Transform>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Transform> OnViewTriggerExitEvent
		{
			add
			{
				Action<Transform> action = OnViewTriggerExitEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnViewTriggerExitEvent__BackingField, (Action<Transform>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Transform> action = OnViewTriggerExitEvent__BackingField;
				Action<Transform> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnViewTriggerExitEvent__BackingField, (Action<Transform>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			_radiusCollider = (SphereCollider)GetComponent<Collider>();
			_radius = _radiusCollider.radius;
		}

		protected override bool IsTriggerItem(Transform item)
		{
			bool flag = IsVisible(item) && IsInsideSector(item);
			if (flag && !_itemsInSector.Contains(item))
			{
				_itemsInSector.Add(item);
			}
			else if (!flag && _itemsInSector.Contains(item))
			{
				_itemsInSector.Remove(item);
			}
			return flag;
		}

		protected override void OnTriggerExit(Collider item)
		{
			base.OnTriggerExit(item);
			if (_itemsInSector.Contains(item.transform))
			{
				_itemsInSector.Remove(item.transform);
			}
		}

		protected override void OnUpdate()
		{
			Items.RemoveAll((Transform t) => t == null);
			List<Transform> list = VisibleItems.FindAll((Transform t) => !Items.Contains(t));
			foreach (Transform item in list)
			{
				if (OnViewTriggerExitEvent__BackingField != null)
				{
					OnViewTriggerExitEvent__BackingField(item);
				}
			}
			VisibleItems.RemoveAll((Transform t) => !Items.Contains(t));
			foreach (Transform item2 in Items)
			{
				bool flag = IsTriggerItem(item2);
				if (flag && !VisibleItems.Contains(item2))
				{
					VisibleItems.Add(item2);
					if (OnViewTriggerEnterEvent__BackingField != null)
					{
						OnViewTriggerEnterEvent__BackingField(item2);
					}
				}
				if (!flag && VisibleItems.Contains(item2))
				{
					VisibleItems.Remove(item2);
					if (OnViewTriggerExitEvent__BackingField != null)
					{
						OnViewTriggerExitEvent__BackingField(item2);
					}
				}
				if (!_itemsInSector.Contains(item2))
				{
					if (flag)
					{
						TriggerEnter(item2);
					}
				}
				else if (!flag)
				{
					TriggerExit(item2);
				}
			}
			for (int num = 0; num < _itemsInSector.Count; num++)
			{
				Transform transform = _itemsInSector[num];
				if (transform == null)
				{
					_itemsInSector.RemoveAt(num);
					num--;
				}
			}
			CheckHitMarkers();
		}

		public bool Contains(Transform item)
		{
			if (_itemsInSector.Contains(item))
			{
				return true;
			}
			return false;
		}

		public Transform GetEnemy()
		{
			if (Items.Count == 0)
			{
				return null;
			}
			List<Transform> list = new List<Transform>();
			foreach (Transform item in Items)
			{
				if (!(item == null) && IsInsideSector(item))
				{
					list.Add(item);
				}
			}
			foreach (Transform item2 in list)
			{
				if (IsVisible(item2))
				{
					return item2;
				}
			}
			return null;
		}

		public void SetBaseLayer(int layer)
		{
			_baseLayer = layer;
			if (_baseLayer != 0)
			{
				int num = LayerMask.NameToLayer("TouchLayer");
				int num2 = LayerMask.NameToLayer("AllyConstraint");
				int num3 = LayerMask.NameToLayer("UnitsConstraints");
				_layerMask = 1 << _baseLayer;
				_layerMask |= 1 << num;
				_layerMask |= 1 << num2;
				_layerMask |= 1 << num3;
				_layerMask = ~_layerMask;
			}
			else
			{
				_layerMask = -5;
			}
		}

		public void SetRadius(float radius)
		{
			_radius = radius;
			_radiusCollider.radius = radius;
		}

		public void SetAngle(float angle)
		{
			_sectorAngle = angle;
		}

		private bool IsInsideSector(Transform item)
		{
			return MathUtil.IsInsideSector(baseTransform.position, item.position, baseTransform.eulerAngles.y + _baseAngleOffset, _sectorAngle, _radius);
		}

		private bool IsVisible(Transform item)
		{
			_currentSelectedItem = item;
			HitMarker component = item.GetComponent<HitMarker>();
			Vector3 vector = baseTransform.position + Vector3.up;
			Vector3 zero = Vector3.zero;
			Collider collider = null;
			if (component != null)
			{
				foreach (Transform marker in component.Markers)
				{
					if (!(marker == null))
					{
						zero = marker.position - vector;
						if (IsIntersectCollider(vector, zero, out collider))
						{
							return true;
						}
					}
				}
			}
			else
			{
				zero = item.position + Vector3.up - vector;
				if (IsIntersectCollider(vector, zero, out collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsIntersectCollider(Vector3 origin, Vector3 direction, out Collider collider)
		{
			collider = null;
			if (_currentSelectedItem != null)
			{
				NetPlayerController component = _currentSelectedItem.gameObject.GetComponent<NetPlayerController>();
				if (component != null && _currentSelectedItem.gameObject.layer == LayerMask.NameToLayer("EnemyLayer") && _currentSelectedItem.parent.name == "Player")
				{
					return true;
				}
			}
			Ray ray = new Ray(origin, direction);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, _layerMask);
			if (hitInfo.collider != null && IsCorrectLayer(hitInfo.collider.gameObject.layer))
			{
				collider = hitInfo.collider;
				return true;
			}
			return false;
		}

		private void CheckHitMarkers()
		{
			if (!(CurrentItem != null))
			{
				return;
			}
			HitMarker component = CurrentItem.GetComponent<HitMarker>();
			if (!(component != null))
			{
				return;
			}
			Vector3 vector = baseTransform.position + Vector3.up * 1.3f;
			foreach (Transform marker in component.Markers)
			{
				if (marker == null)
				{
					continue;
				}
				Vector3 direction = marker.position - vector;
				Collider collider;
				if (IsIntersectCollider(vector, direction, out collider))
				{
					if (!CurrentVisibleMarkers.Markers.Contains(marker))
					{
						CurrentVisibleMarkers.Markers.Add(marker);
					}
				}
				else if (CurrentVisibleMarkers.Markers.Contains(marker))
				{
					CurrentVisibleMarkers.Markers.Remove(marker);
				}
			}
		}

		public void UpdateTargets()
		{
			_radiusCollider.enabled = false;
			_radiusCollider.enabled = true;
		}
	}
}
