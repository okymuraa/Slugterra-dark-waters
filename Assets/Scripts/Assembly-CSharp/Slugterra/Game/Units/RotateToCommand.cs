using AppsMinistry.Core.Command;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Game.Units
{
	[SingletonCommand]
	public sealed class RotateToCommand : MonoCommand<RotateToCommand>
	{
		private float _currentAngle;

		private Transform _baseTarnsform;

		private Transform _rotateTransform;

		[SerializeField]
		private Transform _targetTransform;

		public float Angle
		{
			get
			{
				return _currentAngle;
			}
		}

		protected override void OnStart(object[] args)
		{
			_baseTarnsform = (Transform)args[0];
			_rotateTransform = (Transform)args[1];
			_targetTransform = (Transform)args[2];
		}

		public void ReplaseTarget(Transform newTarget)
		{
			if (newTarget != null)
			{
				_targetTransform = newTarget;
			}
		}

		private void Update()
		{
			if (_targetTransform == null)
			{
				Finish();
			}
			else if (_baseTarnsform != _rotateTransform)
			{
				Vector3 vector = _targetTransform.position - _rotateTransform.position;
				_currentAngle = MathUtil.GetAngle(new Vector2(0f, 1f), new Vector2(vector.normalized.x, vector.normalized.z));
				float num = ((!(_baseTarnsform != null)) ? 0f : (0f - _baseTarnsform.localEulerAngles.y));
				_currentAngle += num;
				_rotateTransform.localEulerAngles = new Vector3(0f, _currentAngle, 0f);
			}
			else
			{
				_rotateTransform.LookAt(_targetTransform);
				_rotateTransform.localEulerAngles = new Vector3(0f, _rotateTransform.localEulerAngles.y, 0f);
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (_rotateTransform != null && _baseTarnsform != _rotateTransform)
			{
				_rotateTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
		}
	}
}
