using System.Collections.Generic;

namespace AppsMinistry.Core.Command
{
	public class AsyncTask<T> where T : MonoCommand
	{
		private List<Responder<T>> _responders;

		private T _command;

		public AsyncTask(T command)
		{
			_responders = new List<Responder<T>>();
			_command = command;
		}

		public void AddResponder(Responder<T> responder)
		{
			_responders.Add(responder);
		}

		public void OnFinish(bool finish = true)
		{
			foreach (Responder<T> responder in _responders)
			{
				if (finish)
				{
					responder.OnSuccess(_command);
				}
				else
				{
					responder.OnFailed(_command);
				}
			}
		}
	}
}
