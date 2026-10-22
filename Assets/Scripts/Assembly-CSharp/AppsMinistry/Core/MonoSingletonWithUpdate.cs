using UnityEngine;

namespace AppsMinistry.Core
{
	public class MonoSingletonWithUpdate<T> : MonoSingleton<T> where T : MonoSingleton<T>
	{
		private float _updateTime;

		private float _nextUpdate;

		private float _scheduleUpdateTime;

		private float _nextScheduleUpdate;

		private bool _isScheduleLoop;

		private bool _isScheduleUpdate;

		protected float CurrentTime
		{
			get
			{
				return Time.time;
			}
		}

		public MonoSingletonWithUpdate(float updateTime)
		{
			_updateTime = updateTime;
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnScheduleUpdate()
		{
		}

		protected sealed override void Update()
		{
			base.Update();
			if (_nextUpdate <= CurrentTime)
			{
				OnUpdate();
				_nextUpdate = CurrentTime + _updateTime;
			}
			if (_isScheduleUpdate && _nextScheduleUpdate <= CurrentTime)
			{
				OnScheduleUpdate();
				if (_isScheduleLoop)
				{
					_nextScheduleUpdate = CurrentTime + _scheduleUpdateTime;
				}
				else
				{
					_isScheduleUpdate = false;
				}
			}
		}

		public void ScheduleUpdate(float time, bool loop = false)
		{
			_scheduleUpdateTime = time;
			_isScheduleLoop = loop;
			_nextScheduleUpdate = CurrentTime + _scheduleUpdateTime;
			_isScheduleUpdate = true;
		}
	}
}
