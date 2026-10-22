using System.Collections.Generic;
using Newtonsoft.Json;

namespace AppsMinistry.Domain.Observer
{
	public class Observable
	{
		private List<IObserver> _observers;

		private bool _commited;

		private List<IObserver> _removeObservers;

		[JsonIgnore]
		public bool Commited
		{
			get
			{
				return _commited;
			}
		}

		public Observable()
		{
			_observers = new List<IObserver>();
			_removeObservers = new List<IObserver>();
			_commited = true;
		}

		public void AddObserver(IObserver observer)
		{
			if (!_observers.Contains(observer))
			{
				_observers.Add(observer);
			}
		}

		public void RemoveObserver(IObserver observer)
		{
			if (_observers.Contains(observer))
			{
				if (_commited)
				{
					_observers.Remove(observer);
				}
				else
				{
					_removeObservers.Add(observer);
				}
			}
		}

		public void Commit()
		{
			_commited = true;
		}

		public void Notify()
		{
			_commited = false;
			for (int i = 0; i < _observers.Count; i++)
			{
				_observers[i].OnObjectChanged(this);
			}
			OnRemoveObserver();
		}

		private void OnRemoveObserver()
		{
			if (_removeObservers.Count <= 0)
			{
				return;
			}
			foreach (IObserver removeObserver in _removeObservers)
			{
				if (_observers.Contains(removeObserver))
				{
					_observers.Remove(removeObserver);
				}
			}
			_removeObservers.Clear();
		}

		public void CommitAndNotify()
		{
			Notify();
			Commit();
		}
	}
}
