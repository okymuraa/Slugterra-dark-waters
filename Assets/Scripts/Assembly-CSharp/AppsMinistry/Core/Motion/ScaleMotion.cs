using UnityEngine;

namespace AppsMinistry.Core.Motion
{
	public class ScaleMotion : BaseMotion
	{
		[SerializeField]
		private Vector3 _targetScale;

		private Vector3 _baseScale;

		public Vector3 TargetScale
		{
			set
			{
				_targetScale = value;
			}
		}

		public Vector3 BaseScale
		{
			get
			{
				return _baseScale;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_baseScale = Transform.localScale;
		}

		protected override void OnMotion()
		{
			if (Iteration % 2 == 0)
			{
				Transform.localScale = Vector3.Lerp(_baseScale, _targetScale, CurrentTime / Time);
			}
			else if (Loop)
			{
				Transform.localScale = Vector3.Lerp(_targetScale, _baseScale, CurrentTime / Time);
			}
		}
	}
}
