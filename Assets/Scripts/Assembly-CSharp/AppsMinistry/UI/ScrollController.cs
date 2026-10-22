using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using UnityEngine;

namespace AppsMinistry.UI
{
	public class ScrollController : MonoBehaviour
	{
		public const float StretchValue = 0.12f;

		public const float MaxSwipeSpeed = 20f;

		public bool allowInput = true;

		private Vector2 smoothDragOffset = new Vector2(0.2f, 0.2f);

		[SerializeField]
		private UnityEngine.Camera _camera;

		[SerializeField]
		private Vector2 _pixelsBetweenValues = Vector2.one * 100f;

		private Vector2 _maxValue = Vector2.one;

		private Vector2 _currentValue = Vector2.zero;

		private Vector2 _smoothValue = Vector2.zero;

		private Vector2 _smoothStartPos = Vector2.zero;

		private Vector2 _scaleFactor = Vector2.one;

		private Vector2 _swipeVelocity = Vector2.one;

		private Vector2[] prevSmoothValues = new Vector2[5];

		private Collider _scrollArea;

		private bool _touched;

		public Vector2 PixelsBetweenValues
		{
			get
			{
				return _pixelsBetweenValues;
			}
			set
			{
				SetPixelsBetweenValues(value);
			}
		}

		public Vector2 MaxValue
		{
			get
			{
				return _maxValue;
			}
			set
			{
				_maxValue = value;
			}
		}

		public Vector2 CurrentValue
		{
			get
			{
				return _currentValue;
			}
			set
			{
				_currentValue = value;
			}
		}

		public Vector2 SmoothValue
		{
			get
			{
				return _smoothValue;
			}
		}

		private void Awake()
		{
			PixelsBetweenValues = _pixelsBetweenValues;
			_scrollArea = GetComponent<Collider>();
		}

		private void OnEnable()
		{
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart += OnTouchStart;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += OnMove;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnTouchEnd;
		}

		private void OnDisable()
		{
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchStart -= OnTouchStart;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouch -= OnMove;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd -= OnTouchEnd;
			}
		}

		public void Shift(float x, float y)
		{
			_currentValue.x = Mathf.Clamp(_currentValue.x + x, 0f, _maxValue.x);
			_currentValue.y = Mathf.Clamp(_currentValue.y + y, 0f, _maxValue.y);
		}

		public void FastGoto(Vector2 value)
		{
			value.x = Mathf.Clamp(value.x, 0f, _maxValue.x);
			value.y = Mathf.Clamp(value.y, 0f, _maxValue.y);
			_currentValue = value;
			_smoothValue = value;
		}

		public void SmoothGoto(Vector2 value)
		{
			value.x = Mathf.Clamp(value.x, 0f, _maxValue.x);
			value.y = Mathf.Clamp(value.y, 0f, _maxValue.y);
			_currentValue = value;
			StopCoroutine("CheckCompleteSmoothTransitionCoroutine");
			StartCoroutine("CheckCompleteSmoothTransitionCoroutine");
		}

		private IEnumerator CheckCompleteSmoothTransitionCoroutine()
		{
			allowInput = false;
			_touched = false;
			_scrollArea.enabled = false;
			yield return new WaitWhile(() => (_smoothValue - _currentValue).sqrMagnitude > 0.05f);
			allowInput = true;
			_scrollArea.enabled = true;
		}

		private void SetPixelsBetweenValues(Vector2 value)
		{
			_pixelsBetweenValues = value;
			_scaleFactor.x = 1f / _pixelsBetweenValues.x;
			_scaleFactor.y = 1f / _pixelsBetweenValues.y;
		}

		private static Vector2 GetAverageValue(Vector2[] arr)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < arr.Length; i++)
			{
				num += arr[i].x;
				num2 += arr[i].y;
			}
			return new Vector2(num / (float)arr.Length, num2 / (float)arr.Length);
		}

		private static void FillArrayWithValue(Vector2[] arr, Vector2 value)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = value;
			}
		}

		private static Vector2 TransformScreenPosition(Vector3 position)
		{
			position.y = (float)Screen.height - position.y;
			return position;
		}

		private void CalculateCurrentValueAndVelocity()
		{
			Vector2 averageValue = GetAverageValue(prevSmoothValues);
			Vector2 vector = averageValue - _currentValue;
			if (vector.x > smoothDragOffset.x || vector.x < 0f - smoothDragOffset.x)
			{
				_currentValue.x = Mathf.Clamp(Mathf.Round(2f * _smoothValue.x - averageValue.x), 0f, _maxValue.x);
				_swipeVelocity.x = _smoothValue.x - averageValue.x;
			}
			if (vector.y > smoothDragOffset.y || vector.y < 0f - smoothDragOffset.y)
			{
				_currentValue.y = Mathf.Clamp(Mathf.Round(2f * _smoothValue.y - averageValue.y), 0f, _maxValue.y);
				_swipeVelocity.y = _smoothValue.y - averageValue.y;
			}
		}

		private void AddSmooth(Vector2 smooth)
		{
			for (int i = 1; i < prevSmoothValues.Length; i++)
			{
				prevSmoothValues[i] = prevSmoothValues[i - 1];
			}
			prevSmoothValues[0] = smooth;
		}

		private bool InScrollArea(Vector3 position)
		{
			if (_scrollArea == null || _camera == null)
			{
				return true;
			}
			RaycastHit hitInfo;
			return _scrollArea.Raycast(_camera.ScreenPointToRay(position), out hitInfo, _camera.farClipPlane);
		}

		private void OnTouchStart(Vector3 position)
		{
			if (allowInput)
			{
				_touched = InScrollArea(position);
				if (_touched)
				{
					_smoothStartPos = _smoothValue + Vector2.Scale(TransformScreenPosition(position), _scaleFactor);
					FillArrayWithValue(prevSmoothValues, _smoothValue);
				}
			}
		}

		private void OnMove(Vector3 position, float actionTime)
		{
			if (allowInput && _touched)
			{
				Vector2 a = TransformScreenPosition(position);
				_smoothValue = _smoothStartPos - Vector2.Scale(a, _scaleFactor);
				_smoothValue.x = Mathf.Clamp(_smoothValue.x, -0.12f, _maxValue.x + 0.12f);
				_smoothValue.y = Mathf.Clamp(_smoothValue.y, -0.12f, _maxValue.y + 0.12f);
				AddSmooth(_smoothValue);
			}
		}

		private void OnTouchEnd(Vector3 position, float actionTime)
		{
			if (allowInput && _touched)
			{
				CalculateCurrentValueAndVelocity();
			}
			_touched = false;
		}

		private void Update()
		{
			if (!_touched)
			{
				_smoothValue = Vector2.SmoothDamp(_smoothValue, _currentValue, ref _swipeVelocity, 0.4f, 20f, Time.unscaledDeltaTime);
			}
		}
	}
}
