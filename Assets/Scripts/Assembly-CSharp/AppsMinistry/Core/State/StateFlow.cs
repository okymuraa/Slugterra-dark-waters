using System;

namespace AppsMinistry.Core.State
{
	public class StateFlow
	{
		private Type _state;

		private Type _nextState;

		private Type _failState;

		public Type State
		{
			get
			{
				return _state;
			}
		}

		public Type NextState
		{
			get
			{
				return _nextState;
			}
		}

		public Type FailState
		{
			get
			{
				if (_failState != null)
				{
					return _failState;
				}
				return _state;
			}
		}

		public StateFlow(Type state, Type nextState, Type failState = null)
		{
			_state = state;
			_nextState = nextState;
			_failState = failState;
		}
	}
}
