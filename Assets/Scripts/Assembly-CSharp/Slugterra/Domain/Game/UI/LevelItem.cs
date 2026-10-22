using System;
using System.Collections.Generic;
using AppsMinistry.Domain;

namespace Slugterra.Domain.Game.UI
{
	public class LevelItem : DomainObject
	{
		public int Order;

		public Type Scene;

		public LevelType Type;

		private LevelStatus _status;

		public static Dictionary<int, int> NewScores = new Dictionary<int, int>();

		public Dictionary<int, int> _scores = new Dictionary<int, int>();

		private static int _current;

		private static int _last = -1;

		private string _name;

		public LevelStatus Status
		{
			get
			{
				return _status;
			}
			set
			{
				if (_status != value)
				{
					_status = value;
					CommitAndNotify();
				}
			}
		}

		public static int Current
		{
			get
			{
				return _current;
			}
			set
			{
				_current = value;
			}
		}

		public static int Last
		{
			get
			{
				return _last;
			}
			set
			{
				_last = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public LevelItem(int order, Type scene, LevelType type, LevelStatus status, string name)
		{
			Order = order;
			Scene = scene;
			Type = type;
			_status = status;
			_scores = new Dictionary<int, int>();
			_name = name;
		}

		public static void SetNewObjectiveStatus(int objectiveIndex, int state)
		{
			if (NewScores.ContainsKey(objectiveIndex))
			{
				NewScores[objectiveIndex] = state;
			}
			else
			{
				NewScores.Add(objectiveIndex, state);
			}
		}

		public static int GetNewObjectiveStatus(int objectiveIndex)
		{
			if (NewScores.ContainsKey(objectiveIndex))
			{
				return NewScores[objectiveIndex];
			}
			return 0;
		}

		public void SetObjectiveStatus(int objectiveIndex, int state)
		{
			if (_scores.ContainsKey(objectiveIndex))
			{
				_scores[objectiveIndex] = state;
			}
			else
			{
				_scores.Add(objectiveIndex, state);
			}
			CommitAndNotify();
		}

		public int GetObjectiveStatus(int objectiveIndex)
		{
			if (_scores.ContainsKey(objectiveIndex))
			{
				return _scores[objectiveIndex];
			}
			return 0;
		}

		public bool IsDaily()
		{
			return Name == "daily";
		}
	}
}
