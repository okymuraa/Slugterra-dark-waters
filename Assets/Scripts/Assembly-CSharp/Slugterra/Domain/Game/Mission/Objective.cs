using AppsMinistry.Domain;

namespace Slugterra.Domain.Game.Mission
{
	public class Objective : DomainObject
	{
		private bool _isComplete;

		private bool _isActive;

		private bool _isVisible;

		private ObjectiveType _type;

		public bool IsComplete
		{
			get
			{
				return _isComplete;
			}
			set
			{
				_isComplete = value;
				CommitAndNotify();
			}
		}

		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;
				CommitAndNotify();
			}
		}

		public bool IsVisible
		{
			get
			{
				return _isVisible;
			}
			set
			{
				_isVisible = value;
				CommitAndNotify();
			}
		}

		public ObjectiveType Type
		{
			get
			{
				return _type;
			}
		}

		public Objective()
		{
			_isComplete = false;
			_isActive = true;
			_isVisible = false;
		}

		public Objective(ObjectiveType type, bool isActive)
		{
			_type = type;
			_isComplete = false;
			_isActive = isActive;
			_isVisible = false;
		}
	}
}
