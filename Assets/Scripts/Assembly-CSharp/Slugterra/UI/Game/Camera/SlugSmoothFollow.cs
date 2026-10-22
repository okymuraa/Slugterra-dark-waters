using System;
using UnityEngine;

namespace Slugterra.UI.Game.Camera
{
	public class SlugSmoothFollow : MonoBehaviour
	{
		private const float SMOOTH_TIME = 0.15f;

		[SerializeField]
		private Transform _target;

		[SerializeField]
		private Vector3 _duelRotation;

		[SerializeField]
		private Vector3 _duelOffset;

		private Transform _transform;

		private Vector3 _moveVelocity;

		private Vector3 _rotationVelocity;

		private float _maxRotateZ;

		private float _rotateZ;

		private void Awake()
		{
			_transform = transform;
			_transform.eulerAngles = _duelRotation;
			_moveVelocity = Vector3.zero;
			_rotationVelocity = Vector3.zero;
			_maxRotateZ = 15f;
			_rotateZ = 0f;
		}

		private void OnEnable()
		{
			SetRotateZ(0f);
			_transform.eulerAngles = new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, 0f);
		}

		private void Update()
		{
			if (_target != null && _target.gameObject.activeSelf)
			{
				float y = _target.eulerAngles.y;
				float f = y / 180f * (float)Math.PI;
				float z = _duelOffset.z * Mathf.Cos(f);
				float x = _duelOffset.z * Mathf.Sin(f);
				Vector3 vector = new Vector3(x, _duelOffset.y, z);
				_transform.position = Vector3.SmoothDamp(_transform.position, _target.position + vector, ref _moveVelocity, 0.15f);
				y = Mathf.SmoothDampAngle(_transform.eulerAngles.y, y, ref _rotationVelocity.y, 0.15f);
				float z2 = Mathf.SmoothDampAngle(_transform.eulerAngles.z, _rotateZ, ref _rotationVelocity.z, 0.3f);
				if (!float.IsNaN(y))
				{
					_transform.eulerAngles = new Vector3(_transform.eulerAngles.x, y, z2);
				}
			}
		}

		public void SetRotateZ(float percent)
		{
			_rotateZ = 0f - _maxRotateZ * percent;
		}

		public void SetTarget(Transform target)
		{
			_target = target;
		}
	}
}
