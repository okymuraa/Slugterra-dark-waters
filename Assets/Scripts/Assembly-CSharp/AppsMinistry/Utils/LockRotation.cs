using UnityEngine;

namespace AppsMinistry.Utils
{
	public class LockRotation : MonoBehaviour
	{
		[SerializeField]
		private bool _lockCameraAngle;

		[SerializeField]
		private Vector3 _lockAngles;

		private Transform _cachedTransform;

		private Transform _cachedCameraTransform;

		private Transform _cameraTransform
		{
			get
			{
				if (_cachedCameraTransform == null && Camera.main != null)
				{
					_cachedCameraTransform = Camera.main.transform;
				}
				return _cachedCameraTransform;
			}
		}

		private void Start()
		{
			_cachedTransform = transform;
		}

		private void Update()
		{
			if (!(_cameraTransform == null))
			{
				if (_lockCameraAngle)
				{
					_cachedTransform.eulerAngles = _cameraTransform.eulerAngles;
				}
				else
				{
					_cachedTransform.eulerAngles = _lockAngles;
				}
			}
		}
	}
}
