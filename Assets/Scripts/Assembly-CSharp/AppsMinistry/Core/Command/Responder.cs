using System;

namespace AppsMinistry.Core.Command
{
	public class Responder<T> where T : MonoCommand
	{
		private Action<T> _success;

		private Action<T> _failed;

		public Responder(Action<T> success)
		{
			_success = success;
			_failed = success;
		}

		public Responder(Action<T> success, Action<T> failed)
		{
			_success = success;
			_failed = failed;
		}

		public void OnSuccess(T command)
		{
			if (_success != null)
			{
				_success(command);
			}
		}

		public void OnFailed(T command)
		{
			if (_failed != null)
			{
				_failed(command);
			}
		}
	}
}
