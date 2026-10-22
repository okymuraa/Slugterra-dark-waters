using AppsMinistry.Core.Command;
using UnityEngine;

namespace AppsMinistry.Core.State
{
	[SingletonCommand]
	public class MonoState : MonoCommand<MonoState>
	{
		private StateMachine _stateMachine;

		private float _scheduleUpdateTime;

		private float _nextScheduleUpdate;

		private bool _isScheduleLoop;

		private bool _isScheduleUpdate;

		public StateMachine StateMachine
		{
			get
			{
				return _stateMachine;
			}
			set
			{
				_stateMachine = value;
			}
		}

		protected new virtual void OnUpdate()
		{
		}

		protected virtual void OnScheduleUpdate()
		{
		}

		private void Update()
		{
			OnUpdate();
			if (_isScheduleUpdate && _nextScheduleUpdate <= Time.time)
			{
				OnScheduleUpdate();
				if (_isScheduleLoop)
				{
					_nextScheduleUpdate = Time.time + _scheduleUpdateTime;
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
			_nextScheduleUpdate = Time.time + _scheduleUpdateTime;
			_isScheduleUpdate = true;
		}
	}
}
