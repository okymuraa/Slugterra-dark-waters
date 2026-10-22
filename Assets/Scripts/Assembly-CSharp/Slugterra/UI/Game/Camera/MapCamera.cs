using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.UI.MapMenu;
using UnityEngine;

namespace Slugterra.UI.Game.Camera
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	internal class MapCamera : MonoBehaviour
	{
		private const float HEIGHT = 12f;

		[SerializeField]
		private float _sensitive = 0.016f;

		[SerializeField]
		private float _smoothTime = 0.05f;

		private Transform _cameraTransform;

		private Vector3 _prevMovePos;

		private Vector3 _movement;

		private Vector3 _offset;

		private Vector3 _basePosition;

		[SerializeField]
		private float _minZoom = 30f;

		[SerializeField]
		private float _maxZoom = 60f;

		private float _zoom;

		[SerializeField]
		private float _minHeight = 8f;

		[SerializeField]
		private float _maxHeight = 12f;

		private float _height;

		[SerializeField]
		private Vector4 _bounds = new Vector4(-8f, 1f, 18f, 7f);

		private bool isLocked;

		private Vector3 _velocity;

		private void Start()
		{
			_cameraTransform = transform;
			_zoom = _maxZoom;
			_height = _maxHeight;
			_cameraTransform.position = new Vector3(_bounds.x, _height, _bounds.y);
			Vector3 vector = Vector3.zero;
			LevelItemPlaceHolderView[] array = Object.FindObjectsOfType<LevelItemPlaceHolderView>();
			List<LevelItem> levels = MonoSingleton<AppController>.Instance.Model.Levels;
			LevelItem levelItem = levels.Find((LevelItem level) => level.Order == LevelItem.Current && level.Type == LevelType.Action);
			if (levelItem == null)
			{
				levelItem = levels.Find((LevelItem level) => level.Order == LevelItem.Current);
			}
			LevelItem levelItem2;
			if (LevelItem.Last > -1 && (levelItem2 = levels.Find((LevelItem level) => level.Order == LevelItem.Last)) != null)
			{
				isLocked = true;
				LevelItemPlaceHolderView[] array2 = array;
				foreach (LevelItemPlaceHolderView levelItemPlaceHolderView in array2)
				{
					if (levelItemPlaceHolderView.LevelName == levelItem2.Scene.Name)
					{
						vector = levelItemPlaceHolderView.transform.position;
					}
				}
				vector = new Vector3(vector.x + 2f, _height, vector.z - 12.4f);
				vector.x = Mathf.Clamp(vector.x, _bounds.x, _bounds.z);
				vector.z = Mathf.Clamp(vector.z, _bounds.y, _bounds.w);
				_cameraTransform.position = vector;
				_basePosition = _cameraTransform.position;
				LevelItemPlaceHolderView[] array3 = array;
				foreach (LevelItemPlaceHolderView levelItemPlaceHolderView2 in array3)
				{
					if (levelItemPlaceHolderView2.LevelName == levelItem.Scene.Name)
					{
						vector = levelItemPlaceHolderView2.transform.position;
					}
				}
				vector = new Vector3(vector.x + 2f, _height, vector.z - 12.4f);
				vector.x = Mathf.Clamp(vector.x, _bounds.x, _bounds.z);
				vector.z = Mathf.Clamp(vector.z, _bounds.y, _bounds.w);
				_cameraTransform.DOLocalMove(vector, 1f).SetDelay(1.6f).SetEase(Ease.InOutCubic)
					.OnComplete(() =>
					{
						_basePosition = _cameraTransform.position;
						isLocked = false;
					});
			}
			else
			{
				LevelItemPlaceHolderView[] array4 = array;
				foreach (LevelItemPlaceHolderView levelItemPlaceHolderView3 in array4)
				{
					if (levelItemPlaceHolderView3.LevelName == levelItem.Scene.Name)
					{
						vector = levelItemPlaceHolderView3.transform.position;
					}
				}
				vector = new Vector3(vector.x + 2f, _height, vector.z - 12.4f);
				vector.x = Mathf.Clamp(vector.x, _bounds.x, _bounds.z);
				vector.z = Mathf.Clamp(vector.z, _bounds.y, _bounds.w);
				_cameraTransform.position = vector;
				_basePosition = _cameraTransform.position;
			}
			_offset = Vector3.zero;
		}

		public void MoveTo(LevelItem item)
		{
			LevelItemPlaceHolderView[] array = Object.FindObjectsOfType<LevelItemPlaceHolderView>();
			List<LevelItem> levels = MonoSingleton<AppController>.Instance.Model.Levels;
			Vector3 vector = Vector3.zero;
			LevelItemPlaceHolderView[] array2 = array;
			foreach (LevelItemPlaceHolderView levelItemPlaceHolderView in array2)
			{
				if (levelItemPlaceHolderView.LevelName == item.Scene.Name)
				{
					vector = levelItemPlaceHolderView.transform.position;
				}
			}
			vector = new Vector3(vector.x + 2f, _height, vector.z - 12.4f);
			vector.x = Mathf.Clamp(vector.x, _bounds.x, _bounds.z);
			vector.z = Mathf.Clamp(vector.z, _bounds.y, _bounds.w);
			_basePosition = vector;
		}

		private void OnEnable()
		{
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouch += OnMove;
			DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd += OnTouchEnd;
			DonDestroyMonoSingleton<InputManager>.Instance.OnZoom += OnZoom;
		}

		private void OnDisable()
		{
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouch -= OnMove;
				DonDestroyMonoSingleton<InputManager>.Instance.OnTouchEnd -= OnTouchEnd;
				DonDestroyMonoSingleton<InputManager>.Instance.OnZoom -= OnZoom;
			}
		}

		private void OnZoom(float zoom)
		{
			if (!isLocked)
			{
				_zoom -= zoom;
				_zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
				_height -= zoom / 5f;
				_height = Mathf.Clamp(_height, _minHeight, _maxHeight);
			}
		}

		private void OnMove(Vector3 position, float actionTime)
		{
			if (!isLocked)
			{
				if (_prevMovePos == Vector3.zero)
				{
					_prevMovePos = position;
					_offset = Vector3.zero;
					_basePosition = _cameraTransform.position;
				}
				Vector3 vector = -(position - _prevMovePos);
				_movement = new Vector3(vector.x, 0f, vector.y);
				_offset += _movement * _sensitive;
				_prevMovePos = position;
			}
		}

		private void OnTouchEnd(Vector3 position, float actionTime)
		{
			if (!isLocked)
			{
				_movement = Vector3.zero;
				_prevMovePos = Vector3.zero;
			}
		}

		private void LateUpdate()
		{
			if (!isLocked)
			{
				Vector3 vector = _basePosition + _offset;
				vector.x = Mathf.Clamp(vector.x, _bounds.x, _bounds.z);
				vector.z = Mathf.Clamp(vector.z, _bounds.y, _bounds.w);
				float x = Mathf.SmoothDamp(_cameraTransform.position.x, vector.x, ref _velocity.x, _smoothTime);
				float z = Mathf.SmoothDamp(_cameraTransform.position.z, vector.z, ref _velocity.z, _smoothTime);
				float y = Mathf.SmoothDamp(_cameraTransform.position.y, _height, ref _velocity.y, 0.1f);
				_cameraTransform.position = new Vector3(x, y, z);
			}
		}
	}
}
