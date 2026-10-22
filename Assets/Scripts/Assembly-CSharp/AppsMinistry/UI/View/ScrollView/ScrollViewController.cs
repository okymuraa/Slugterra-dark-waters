using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using UnityEngine;

namespace AppsMinistry.UI.View.ScrollView
{
	public abstract class ScrollViewController<TItem> : UIController<UIView> where TItem : MonoBehaviour
	{
		[SerializeField]
		private Alignment _alignment;

		[SerializeField]
		private float _visibleWidth;

		[SerializeField]
		private float _visibleHeigh;

		[SerializeField]
		private ScrollDirection _scrollDirection;

		[SerializeField]
		private UnityEngine.Camera _camera;

		[SerializeField]
		private float _itemWidth = 0.25f;

		[SerializeField]
		private float _itemHeight = 0.25f;

		[SerializeField]
		private GameObject _itemPrefab;

		[SerializeField]
		private bool _fixedScroll;

		private BoxCollider _scrollableArea;

		private bool _isPressed;

		private Vector3 _lastPosition;

		private float _currentOffset;

		private float _lastOffset;

		private float _swipeForce;

		private float _swipeVelocity;

		private List<TItem> _items;

		private int _currentIndex;

		public int CurrentIndex
		{
			get
			{
				return _currentIndex;
			}
			set
			{
				_currentIndex = value;
				int num = (int)(_visibleHeigh / _itemHeight);
				if (_currentIndex > Count - num)
				{
					_currentIndex = Count - num;
				}
				if (_currentIndex < 0)
				{
					_currentIndex = 0;
				}
			}
		}

		protected abstract int Count { get; }

		protected abstract void OnItemChanged(TItem item, int index);

		protected override void OnCreate()
		{
			base.OnCreate();
			_items = new List<TItem>();
			if (_camera == null)
			{
				_camera = UnityEngine.Camera.main;
			}
			if (_camera == null && UnityEngine.Camera.allCamerasCount > 0)
			{
				_camera = UnityEngine.Camera.allCameras[0];
			}
			_scrollableArea = gameObject.AddComponent<BoxCollider>();
			UpdateScrollableArea();
			_currentOffset = 0f;
			_currentIndex = 0;
			_isPressed = false;
		}

		protected override void _OnEnable()
		{
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart += OnTouchStart;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += OnMove;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnTouchEnd;
		}

		protected override void _OnDisable()
		{
			if (!(DonDestroyMonoSingleton<InputManager>.Instance == null))
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart += OnTouchStart;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += OnMove;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnTouchEnd;
			}
		}

		private void OnDrawGizmos()
		{
			Vector3 vector = transform.position + GetOffset();
			Vector3 vector2 = vector + Vector3.right * _visibleWidth;
			Vector3 vector3 = vector - Vector3.up * _visibleHeigh;
			Vector3 vector4 = vector2 - Vector3.up * _visibleHeigh;
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector4);
			Gizmos.DrawLine(vector4, vector3);
			Gizmos.DrawLine(vector3, vector);
		}

		private void OnTouchStart(Vector3 position)
		{
			if (!(_camera != null))
			{
				return;
			}
			Ray ray = _camera.ScreenPointToRay(position);
			RaycastHit[] array = Physics.RaycastAll(ray);
			RaycastHit[] array2 = array;
			foreach (RaycastHit raycastHit in array2)
			{
				if (raycastHit.collider == _scrollableArea)
				{
					_isPressed = true;
					_lastPosition = _camera.ScreenToWorldPoint(position);
					_swipeForce = 0f;
					break;
				}
			}
		}

		private void OnTouchEnd(Vector3 position, float actionTime)
		{
			_isPressed = false;
			CalculateCurrentIndex();
		}

		private void CalculateCurrentIndex()
		{
			CurrentIndex = Mathf.RoundToInt(_currentOffset / _itemHeight);
		}

		private void OnMove(Vector3 position, float actionTime)
		{
			if (_isPressed)
			{
				_swipeForce = _lastOffset * 10f;
				Vector3 lastPosition = _camera.ScreenToWorldPoint(position);
				float num = 0f;
				Move(_lastOffset = ((_scrollDirection != ScrollDirection.Vertical) ? (lastPosition.x - _lastPosition.x) : (lastPosition.y - _lastPosition.y)));
				OnUpdateItemPosition();
				_lastPosition = lastPosition;
			}
		}

		private void Move(float offset)
		{
			_currentOffset += offset;
			if (_currentOffset < 0f)
			{
				_currentOffset = 0f;
			}
			if (_itemHeight * (float)Count > _visibleHeigh && _itemHeight * (float)Count - _visibleHeigh < _currentOffset)
			{
				_currentOffset = _itemHeight * (float)Count - _visibleHeigh;
			}
			else if (_itemHeight * (float)Count < _visibleHeigh)
			{
				_currentOffset = 0f;
			}
		}

		private Vector3 GetOffset()
		{
			Vector3 result = new Vector3((0f - _visibleWidth) / 2f, _visibleHeigh / 2f, 0f);
			if (_alignment == Alignment.TopLeft)
			{
				result = new Vector3(0f, 0f, 0f);
			}
			else if (_alignment == Alignment.TopCenter)
			{
				result = new Vector3((0f - _visibleWidth) / 2f, 0f, 0f);
			}
			else if (_alignment == Alignment.TopRight)
			{
				result = new Vector3(0f - _visibleWidth, 0f, 0f);
			}
			else if (_alignment == Alignment.MiddleLeft)
			{
				result = new Vector3(0f, _visibleHeigh / 2f, 0f);
			}
			else if (_alignment == Alignment.MiddleCenter)
			{
				result = new Vector3((0f - _visibleWidth) / 2f, _visibleHeigh / 2f, 0f);
			}
			else if (_alignment == Alignment.MiddleRight)
			{
				result = new Vector3(0f - _visibleWidth, _visibleHeigh / 2f, 0f);
			}
			else if (_alignment == Alignment.BottomLeft)
			{
				result = new Vector3(0f, _visibleHeigh, 0f);
			}
			else if (_alignment == Alignment.BottomCenter)
			{
				result = new Vector3((0f - _visibleWidth) / 2f, _visibleHeigh, 0f);
			}
			else if (_alignment == Alignment.BottomRight)
			{
				result = new Vector3(0f - _visibleWidth, _visibleHeigh, 0f);
			}
			return result;
		}

		public void SetVisibleWidth(float width)
		{
			_visibleWidth = width;
			UpdateScrollableArea();
			Rebuild();
		}

		public void SetVisibleHeight(float height)
		{
			_visibleHeigh = height;
			UpdateScrollableArea();
			Rebuild();
		}

		public void SetCurrentItem(int index)
		{
			_currentOffset = _itemHeight * (float)index;
		}

		private void UpdateScrollableArea()
		{
			_scrollableArea.size = new Vector3(_visibleWidth, _visibleHeigh, 1f);
			_scrollableArea.center = GetOffset() + new Vector3(_visibleWidth / 2f, (0f - _visibleHeigh) / 2f, 1f);
			GameObject gameObject = null;
			int num = 0;
			for (float num2 = 0f; num2 <= _visibleHeigh; num2 += _itemHeight)
			{
				if (_items.Count <= num)
				{
					gameObject = Object.Instantiate(_itemPrefab);
					gameObject.transform.parent = transform;
					_items.Add((TItem)gameObject.GetComponent(typeof(TItem)));
				}
				num++;
			}
			while (_items.Count < num)
			{
				_items.RemoveAt(num);
			}
			gameObject = Object.Instantiate(_itemPrefab);
			gameObject.transform.parent = transform;
			_items.Add((TItem)gameObject.GetComponent(typeof(TItem)));
		}

		private void OnUpdateItemPosition()
		{
			if (Count == 0)
			{
				for (int i = 0; i < _items.Count; i++)
				{
					TItem val = _items[i];
					val.gameObject.SetActive(false);
				}
				return;
			}
			int b = (int)(_currentOffset / _itemHeight);
			b = Mathf.Min(Mathf.Max(0, b), Count - 1);
			float num = _currentOffset - (float)b * _itemHeight;
			Vector3 vector = transform.position + GetOffset();
			int num2 = 0;
			while (num2 < _items.Count)
			{
				TItem val2 = _items[num2];
				val2.gameObject.SetActive(true);
				TItem val3 = _items[num2];
				val3.transform.position = vector + new Vector3(_itemWidth / 2f, num - (float)num2 * _itemHeight);
				if (b < Count)
				{
					OnItemChanged(_items[num2], b);
				}
				else
				{
					TItem val4 = _items[num2];
					val4.gameObject.SetActive(false);
				}
				num2++;
				b++;
			}
		}

		private void Update()
		{
			if (Mathf.Abs(_swipeForce) > 0.01f)
			{
				float swipeForce = _swipeForce;
				_swipeForce = Mathf.SmoothDamp(_swipeForce, 0f, ref _swipeVelocity, 0.5f);
				Move(swipeForce - _swipeForce);
				OnUpdateItemPosition();
				if (_fixedScroll && Mathf.Abs(_swipeForce) < 0.2f)
				{
					CalculateCurrentIndex();
					_swipeForce = 0f;
				}
			}
			else if (_fixedScroll && _currentOffset != (float)_currentIndex * _itemHeight)
			{
				_currentOffset = Mathf.SmoothDamp(_currentOffset, (float)_currentIndex * _itemHeight, ref _swipeVelocity, 0.3f);
				OnUpdateItemPosition();
			}
			else
			{
				_swipeForce = 0f;
			}
		}

		public void Rebuild()
		{
			_swipeForce = 0f;
			OnUpdateItemPosition();
		}
	}
}
