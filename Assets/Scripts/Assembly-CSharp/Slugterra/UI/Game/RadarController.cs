using System;
using System.Collections.Generic;
using System.Linq;
using AppsMinistry.UI;
using Slugterra.Game.Units.AreaTrigger;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class RadarController : UIController<RadarView>
	{
		[SerializeField]
		private GameObject _itemPrefab;

		[SerializeField]
		private GameObject _enemyMarkerPrefab;

		[SerializeField]
		private float _updateTime = 4f;

		[SerializeField]
		private AreaTrigger _radarArea;

		private Vector3 _itemOffset;

		private Transform _baseTransform;

		private float _currentAngle;

		private float _offsetAngle;

		private IDictionary<Transform, RadarItemController> _items;

		private Dictionary<Transform, RadarItemController> _enemyes;

		private GameObject _obj;

		protected override void _OnEnable()
		{
			_radarArea.OnTriggerEnterEvent += OnRadarAreaEnter;
			_radarArea.OnTriggerExitEvent += OnRadarAreaExit;
			if (_radarArea == null || _radarArea.Items == null)
			{
				return;
			}
			foreach (Transform item in _radarArea.Items)
			{
				AddEnemy(item, _enemyMarkerPrefab);
			}
		}

		protected override void _OnDisable()
		{
			_radarArea.OnTriggerEnterEvent -= OnRadarAreaEnter;
			_radarArea.OnTriggerExitEvent -= OnRadarAreaExit;
			if (_radarArea.Items != null)
			{
				foreach (Transform item in _radarArea.Items)
				{
					RemoveEnemy(item);
				}
			}
			_enemyes.Clear();
		}

		private void OnRadarAreaEnter(Transform transform)
		{
			AddEnemy(transform, _enemyMarkerPrefab);
		}

		private void OnRadarAreaExit(Transform item)
		{
			RemoveEnemy(item);
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_offsetAngle = 360f / _updateTime;
			_itemOffset = Vector3.up * 0.035f + Vector3.forward * -0.5f;
			_items = new Dictionary<Transform, RadarItemController>();
			_enemyes = new Dictionary<Transform, RadarItemController>();
			if (!(_radarArea == null))
			{
				return;
			}
			AreaTrigger[] array = UnityEngine.Object.FindObjectsOfType<AreaTrigger>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name == "RadarArea")
				{
					_radarArea = array[i];
					break;
				}
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			foreach (Transform item in _radarArea.Items)
			{
				AddEnemy(item, _enemyMarkerPrefab);
			}
		}

		protected override void OnUpdate()
		{
			_currentAngle -= _offsetAngle * Time.deltaTime;
			if (_currentAngle < 0f)
			{
				_currentAngle += 360f;
			}
			Vector3 vector = new Vector3(Mathf.Cos((_currentAngle + 90f) * ((float)Math.PI / 180f)), Mathf.Sin((_currentAngle + 90f) * ((float)Math.PI / 180f)), 0f);
			foreach (Transform key in _items.Keys)
			{
				RadarItemController radarItemController = _items[key];
				Vector3 vector2 = Vector3.ClampMagnitude(key.position - _baseTransform.position, 25f);
				vector2 = new Vector3(vector2.x, vector2.z, 0f);
				if (Vector3.SqrMagnitude(vector - vector2.normalized) < 0.01f)
				{
					radarItemController.UpdatePosition(vector2 / 25f * 0.44f + _itemOffset, _updateTime);
				}
			}
			foreach (KeyValuePair<Transform, RadarItemController> item in _enemyes.Where((KeyValuePair<Transform, RadarItemController> kvp) => kvp.Key == null).ToList())
			{
				_enemyes.Remove(item.Key);
				if (item.Value != null)
				{
					UnityEngine.Object.Destroy(item.Value.gameObject);
				}
			}
			foreach (Transform key2 in _enemyes.Keys)
			{
				if (!(key2 == null))
				{
					RadarItemController radarItemController2 = _enemyes[key2];
					Vector3 vector3 = Vector3.ClampMagnitude(key2.position - _baseTransform.position, 25f);
					vector3 = new Vector3(vector3.x, vector3.z, 0f);
					if (Vector3.SqrMagnitude(vector - vector3.normalized) < 0.01f)
					{
						radarItemController2.UpdatePosition(vector3 / 25f * 0.44f + _itemOffset, _updateTime);
					}
				}
			}
			View.ApplyModel(_currentAngle);
		}

		public void AddItem(Transform transform, GameObject marker)
		{
			if (!_items.ContainsKey(transform))
			{
				RadarItemController radarItemController = CreateItem(marker);
				_items[transform] = radarItemController;
				Vector3 vector = Vector3.ClampMagnitude(transform.position - _baseTransform.position, 25f);
				radarItemController.UpdatePosition(new Vector3(vector.x, vector.z, 0f) / 25f * 0.44f + _itemOffset, _updateTime);
			}
		}

		private void AddEnemy(Transform transform, GameObject marker)
		{
			if (!(transform == null) && !_enemyes.ContainsKey(transform))
			{
				RadarItemController radarItemController = CreateItem(marker);
				_enemyes[transform] = radarItemController;
				Vector3 vector = Vector3.ClampMagnitude(transform.position - _baseTransform.position, 25f);
				radarItemController.UpdatePosition(new Vector3(vector.x, vector.z, 0f) / 25f * 0.44f + _itemOffset, _updateTime);
			}
		}

		private RadarItemController CreateItem(GameObject prefab)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab, transform.position, Quaternion.identity);
			gameObject.transform.parent = transform;
			return gameObject.GetComponent<RadarItemController>();
		}

		public void RemoveItem(Transform transform)
		{
			if (_items.ContainsKey(transform))
			{
				UnityEngine.Object.Destroy(_items[transform].gameObject);
				_items.Remove(transform);
			}
		}

		private void RemoveEnemy(Transform transform)
		{
			if (!(transform == null) && _enemyes.ContainsKey(transform))
			{
				UnityEngine.Object.Destroy(_enemyes[transform].gameObject);
				_enemyes.Remove(transform);
			}
		}

		public void SetBaseTransform(Transform transform)
		{
			_baseTransform = transform;
		}
	}
}
