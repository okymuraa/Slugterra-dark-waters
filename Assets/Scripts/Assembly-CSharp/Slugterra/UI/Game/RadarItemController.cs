using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class RadarItemController : UIController<RadarItemView>
	{
		private Transform _baseTransform;

		private float _updateTime;

		private float _currentProgress;

		protected override void OnCreate()
		{
			base.OnCreate();
			_baseTransform = transform;
		}

		protected override void OnUpdate()
		{
			_currentProgress -= Time.deltaTime;
			if (_currentProgress <= 0f)
			{
				_currentProgress = 0f;
			}
			View.ApplyModel(_currentProgress / _updateTime);
		}

		public void UpdatePosition(Vector3 position, float updateTime = 1f)
		{
			if (!(_currentProgress / _updateTime > 0.5f))
			{
				_baseTransform.localPosition = position;
				_updateTime = updateTime;
				_currentProgress = _updateTime;
			}
		}
	}
}
